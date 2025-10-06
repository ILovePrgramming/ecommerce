using ECommerce.Core.DTOs;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using ECommerce.Data.Repositories;
using ECommerce.Services.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

[TestFixture]
public class CartServiceTests
{
    private Mock<CartRepository> _repoMock;
    private Mock<ProductRepository> _productRepoMock;
    private Mock<ILogger<CartService>> _loggerMock;
    private Mock<IPaymentService> _paymentServiceMock;
    private CartService _service;

    [SetUp]
    public void Setup()
    {
        _repoMock = new Mock<CartRepository>(null);
        _productRepoMock = new Mock<ProductRepository>(null);
        _loggerMock = new Mock<ILogger<CartService>>();
        _paymentServiceMock = new Mock<IPaymentService>();
        _service = new CartService(_repoMock.Object, _productRepoMock.Object, _loggerMock.Object, _paymentServiceMock.Object);
    }

    [Test]
    public void GetCart_ValidUserId_ReturnsCartItems()
    {
        var userId = "user1";
        var cartItems = new List<CartItem> { new CartItem { ProductId = 1, Quantity = 2, UserId = userId } };
        _repoMock.Setup(r => r.GetCart(userId)).Returns(cartItems);

        var result = _service.GetCart(userId).ToList();

        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].ProductId, Is.EqualTo(1));
        Assert.That(result[0].Quantity, Is.EqualTo(2));
    }

    [Test]
    public void AddToCart_InvalidUserId_ThrowsException()
    {
        Assert.Throws<System.ArgumentException>(() => _service.AddToCart("", new CartDto { ProductId = 1, Quantity = 1 }));
    }

    [Test]
    public void AddToCart_InvalidCartDto_ThrowsException()
    {
        Assert.Throws<System.ArgumentException>(() => _service.AddToCart("user1", new CartDto { ProductId = 0, Quantity = 0 }));
    }

    [Test]
    public void RemoveFromCart_InvalidProductId_ThrowsException()
    {
        Assert.Throws<System.ArgumentException>(() => _service.RemoveFromCart("user1", 0));
    }

    [Test]
    public void ClearCart_ValidUserId_CallsRepoClear()
    {
        var userId = "user1";
        _service.ClearCart(userId);
        _repoMock.Verify(r => r.Clear(userId), Times.Once);
        _repoMock.Verify(r => r.Save(), Times.Once);
    }

    [Test]
    public void Checkout_EmptyCart_ReturnsFalse()
    {
        var userId = "user1";
        _repoMock.Setup(r => r.GetCart(userId)).Returns(new List<CartItem>());
        var result = _service.Checkout(userId);
        Assert.That(result, Is.False);
    }

    [Test]
    public void Checkout_PaymentSuccess_ClearsCart()
    {
        var userId = "user1";
        var cartItems = new List<CartItem> { new CartItem { ProductId = 1, Quantity = 2, UserId = userId } };
        _repoMock.Setup(r => r.GetCart(userId)).Returns(cartItems);
        _paymentServiceMock.Setup(p => p.ProcessPayment(It.IsAny<OrderDto>())).Returns(true);

        var result = _service.Checkout(userId);

        Assert.That(result, Is.True);
        _repoMock.Verify(r => r.Clear(userId), Times.Once);
        _repoMock.Verify(r => r.Save(), Times.Once);
    }

    [Test]
    public void Checkout_PaymentFails_DoesNotClearCart()
    {
        var userId = "user1";
        var cartItems = new List<CartItem> { new CartItem { ProductId = 1, Quantity = 2, UserId = userId } };
        _repoMock.Setup(r => r.GetCart(userId)).Returns(cartItems);
        _paymentServiceMock.Setup(p => p.ProcessPayment(It.IsAny<OrderDto>())).Returns(false);

        var result = _service.Checkout(userId);

        Assert.That(result, Is.False);
        _repoMock.Verify(r => r.Clear(userId), Times.Never);
    }

    [Test]
    public void AddToCart_InsufficientStock_ThrowsException()
    {
        var userId = "user1";
        var dto = new CartDto { ProductId = 1, Quantity = 10 };
        var product = new Product { Id = 1, Stock = 5 };
        _productRepoMock.Setup(p => p.GetById(dto.ProductId)).Returns(product);

        var ex = Assert.Throws<InvalidOperationException>(() => _service.AddToCart(userId, dto));
        Assert.That(ex.Message, Is.EqualTo("Insufficient stock."));
    }

    [Test]
    public void AddToCart_ProductNotFound_ThrowsException()
    {
        var userId = "user1";
        var dto = new CartDto { ProductId = 99, Quantity = 1 };
        _productRepoMock.Setup(p => p.GetById(dto.ProductId)).Returns((Product)null);

        var ex = Assert.Throws<InvalidOperationException>(() => _service.AddToCart(userId, dto));
        Assert.That(ex.Message, Is.EqualTo("Insufficient stock."));
    }

    [Test]
    public void UpdateCartItem_ValidInput_UpdatesQuantity()
    {
        var userId = "user1";
        var productId = 1;
        var newQuantity = 3;
        var product = new Product { Id = productId, Stock = 10 };
        _productRepoMock.Setup(p => p.GetById(productId)).Returns(product);

        _service.UpdateCartItem(userId, productId, newQuantity);

        _repoMock.Verify(r => r.UpdateQuantity(userId, productId, newQuantity), Times.Once);
        _repoMock.Verify(r => r.Save(), Times.Once);
    }

    [Test]
    public void UpdateCartItem_InsufficientStock_ThrowsException()
    {
        var userId = "user1";
        var productId = 1;
        var newQuantity = 10;
        var product = new Product { Id = productId, Stock = 5 };
        _productRepoMock.Setup(p => p.GetById(productId)).Returns(product);

        var ex = Assert.Throws<InvalidOperationException>(() => _service.UpdateCartItem(userId, productId, newQuantity));
        Assert.That(ex.Message, Is.EqualTo("Insufficient stock."));
    }

    [Test]
    public void UpdateCartItem_ProductNotFound_ThrowsException()
    {
        var userId = "user1";
        var productId = 99;
        var newQuantity = 1;
        _productRepoMock.Setup(p => p.GetById(productId)).Returns((Product)null);

        var ex = Assert.Throws<InvalidOperationException>(() => _service.UpdateCartItem(userId, productId, newQuantity));
        Assert.That(ex.Message, Is.EqualTo("Insufficient stock."));
    }

    [Test]
    public void UpdateCartItem_InvalidInput_ThrowsException()
    {
        Assert.Throws<System.ArgumentException>(() => _service.UpdateCartItem("", 1, 1));
        Assert.Throws<System.ArgumentException>(() => _service.UpdateCartItem("user1", 0, 1));
        Assert.Throws<System.ArgumentException>(() => _service.UpdateCartItem("user1", 1, 0));
    }
}