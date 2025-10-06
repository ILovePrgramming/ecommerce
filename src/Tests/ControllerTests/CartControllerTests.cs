using ECommerce.API.Controllers;
using ECommerce.Core.DTOs;
using ECommerce.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

[TestFixture]
public class CartControllerTests
{
    private Mock<ICartService> _serviceMock;
    private Mock<ILogger<CartController>> _loggerMock;
    private CartController _controller;

    [SetUp]
    public void Setup()
    {
        _serviceMock = new Mock<ICartService>();
        _loggerMock = new Mock<ILogger<CartController>>();
        _controller = new CartController(_serviceMock.Object, _loggerMock.Object);
    }

    [Test]
    public void Get_ValidUserId_ReturnsOk()
    {
        var userId = "user1";
        _serviceMock.Setup(s => s.GetCart(userId)).Returns(new List<CartDto> { new CartDto { ProductId = 1, Quantity = 2 } });
        var result = _controller.Get(userId);
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public void Get_InvalidUserId_ReturnsBadRequest()
    {
        var result = _controller.Get("");
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public void AddToCart_ValidInput_ReturnsOk()
    {
        var userId = "user1";
        var dto = new CartDto { ProductId = 1, Quantity = 2 };
        var result = _controller.AddToCart(userId, dto);
        Assert.That(result, Is.InstanceOf<OkResult>());
    }

    [Test]
    public void AddToCart_InvalidUserId_ReturnsBadRequest()
    {
        var dto = new CartDto { ProductId = 1, Quantity = 2 };
        var result = _controller.AddToCart("", dto);
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public void Remove_ValidInput_ReturnsNoContent()
    {
        var result = _controller.Remove("user1", 1);
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public void Remove_InvalidUserId_ReturnsBadRequest()
    {
        var result = _controller.Remove("", 1);
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public void Remove_InvalidProductId_ReturnsBadRequest()
    {
        var result = _controller.Remove("user1", 0);
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public void Clear_ValidUserId_ReturnsNoContent()
    {
        var result = _controller.Clear("user1");
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public void Clear_InvalidUserId_ReturnsBadRequest()
    {
        var result = _controller.Clear("");
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
}