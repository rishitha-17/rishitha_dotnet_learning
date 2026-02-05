# WebAPI Practice - Product Management API


## 🏗️ Architecture

```
├── Controllers/          # API Controllers (Presentation Layer)
├── Services/            # Business Logic Layer
│   ├── Interfaces/      # Service contracts
│   └── ProductService.cs
├── Repositories/        # Data Access Layer
│   ├── Interfaces/      # Repository contracts
│   └── ProductRepository.cs
├── Models/
│   ├── Entities/        # Database entities
│   └── DTOs/           # Data Transfer Objects
├── Data/               # Database context
└── Program.cs          # Application entry point
```

## 🛠️ Technologies Used

- **.NET 10.0** - Latest .NET framework
- **ASP.NET Core Web API** - Web framework
- **Entity Framework Core** - ORM
- **PostgreSQL** - Database
- **Npgsql** - PostgreSQL provider for EF Core
- **Swagger/OpenAPI** - API documentation

## 📦 Dependencies

```xml
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="10.0.2" />
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="10.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="10.1.1" />
```

## 🗄️ Database Schema

### Products Table
```sql
CREATE TABLE products (
    id SERIAL PRIMARY KEY,
    name VARCHAR NOT NULL,
    price DECIMAL NOT NULL,
    category VARCHAR NOT NULL
);
```


## 🚀 Getting Started

### Prerequisites
- .NET 10.0 SDK
- PostgreSQL database server
- Visual Studio 2024 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd webapi-practice
   ```


2. **Update database connection**
   - Modify `appsettings.json` with your PostgreSQL credentials

3. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access Swagger UI**
   - Navigate to `https://localhost:5001/swagger` or `http://localhost:5000/swagger`

## 📋 API Endpoints

### Products

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/products` | Get all products |



### Running Tests
```bash
dotnet test
```
