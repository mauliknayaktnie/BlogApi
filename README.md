Blog API Backend (ASP.NET Core)

This is a backend API built using .NET Core for an Android app.
It includes authentication, user profile management, and article CRUD operations.

Features:
- User authentication using JWT
- User registration & login
- Fetch user profile (requires authentication)
- CRUD operations for articles
- Authorization for protected routes

Technologies Used:
- .NET Core 7
- Entity Framework Core (Database-First)
- SQL Server
- JWT Authentication
- Swagger

Installation & Setup:
1. Clone the repository:
   git clone https://github.com/your-repo/blog-api.git
   cd blog-api

2. Configure Database:
   - Update appsettings.json with database connection string and JWT key.
   - Run:
     dotnet ef dbcontext scaffold "Server=YOUR_SERVER;Database=BlogDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models

3. Run the Application:
   dotnet run
   (API will be available at http://localhost:5098)

Thank you :)
