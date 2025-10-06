# ecommerce

Scalable, multi-tier C#, ASP.NET Core service with RESTful APIs for an e-commerce platform.

## Solution Structure

- **API**  
  ASP.NET Core Web API project exposing endpoints for authentication, product management, cart operations, and admin features.

- **Services**  
  Contains business logic and service classes (e.g., `AuthService`, `CartService`, `ProductService`, `JwtProvider`, `LoggingService`).

- **Core**  
  Shared DTOs, interfaces, and entity models used across the solution.

- **Data**  
  Entity Framework Core repositories and database context (`AppDbContext`) for data access.

- **Tests**  
  NUnit test projects for unit testing services and controllers.

## Build Instructions

1. **Prerequisites**
   - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - [Docker](https://www.docker.com/products/docker-desktop) (for containerization)
   - [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (local or via Docker)

2. **Restore Dependencies**
    dotnet restore

3. **Build Solution**
	dotnet build

4. **Run Locally**
    dotnet run --project API/API.proj

The API will be available at `http://localhost:5146` (or as configured).

5. **Run Tests**

## Containerization

To run the API and SQL Server in Docker containers:

1. **Build and Start Containers**
   dotnet test

2. **Environment Variables**
   - You can override database name and password using `.env` file or environment variables:
     ```
     DB_NAME=ECommerceDb
     DB_PASSWORD=Your_password123
     ```

3. **Access API**
   - The API will be available at `http://localhost:8080`
   - SQL Server at `localhost:1433`

## Deploying to AWS

You can deploy the solution to AWS using [Elastic Beanstalk](https://aws.amazon.com/elasticbeanstalk/) for the API and [Amazon RDS](https://aws.amazon.com/rds/) for SQL Server.

### Steps

1. **Publish the API**
    dotnet publish API/API.csproj -c Release -o ./publish
1. **Create a Docker Image**
   - Navigate to the `src` folder and run:
     ```sh
     docker build -t ecommerce-api .
     ```

2. **Create an Elastic Beanstalk Environment**
   - Use the AWS Management Console or AWS CLI:
     ```sh
     eb init -p "dotnet-core" ecommerce-api
     eb create ecommerce-api-env
     ```

3. **Configure Environment Variables**
   - Set connection string and other secrets in Elastic Beanstalk environment configuration.

4. **Provision SQL Server on Amazon RDS**
   - Create a SQL Server instance.
   - Update your connection string in Elastic Beanstalk to point to the RDS endpoint.

5. **Deploy**
   eb deploy
   - Use the AWS Management Console or AWS CLI to deploy the Docker image to Elastic Beanstalk.
   - Monitor the deployment process and ensure the application is running smoothly.

6. **Migrate Database**
   - Run EF Core migrations against the RDS instance if needed:
     ```sh
     dotnet ef database update --connection "Server=<RDS-endpoint>;Database=<DB_NAME>;User Id=<username>;Password=<password>;"
     ```

## Additional Notes

- All configuration values (connection strings, secrets) should be managed via environment variables or AWS Secrets Manager.
- For production, ensure HTTPS is enabled and sensitive data is secured.
- Scale out using AWS Auto Scaling and load balancers as needed.

---

**For more details, see individual project documentation and source code.**
