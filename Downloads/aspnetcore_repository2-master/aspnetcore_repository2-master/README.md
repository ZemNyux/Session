# ASP.NET Core Repository Pattern

A simple educational ASP.NET Core MVC application demonstrating how to implement the **Repository Pattern** in a typical .NET project.

The project was created as a learning example to practice working with ASP.NET Core MVC, Entity Framework Core, dependency injection, and separation of data-access logic from application logic.

## ✨ Features

* ASP.NET Core MVC
* Entity Framework Core
* Repository Pattern
* Dependency Injection
* CRUD operations
* Razor Views
* Separation of concerns
* Database integration

## 🛠️ Technologies

* **C#**
* **ASP.NET Core**
* **Entity Framework Core**
* **Razor**
* **SQL Server**
* **Visual Studio**

## 📁 Project Structure

The application follows a simple MVC structure with an additional repository layer:

```text
├── Controllers/
├── Models/
├── Views/
├── Repositories/
├── Data/
├── wwwroot/
└── Program.cs
```

The repository layer is responsible for data-access operations, while controllers handle application requests and views provide the user interface.

## 🚀 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/sunmeat/aspnetcore_repository.git
cd aspnetcore_repository
```

### 2. Configure the database

Update the connection string in `appsettings.json` according to your local SQL Server configuration.

### 3. Apply migrations

If migrations are included in the project, run:

```bash
dotnet ef database update
```

### 4. Run the application

```bash
dotnet run
```

Then open the URL displayed in the terminal.

## 🎯 Purpose

This is a **learning project**, created to understand the Repository Pattern and practice building a basic ASP.NET Core MVC application.

It is intentionally kept relatively simple and is not intended to be a production-ready application.

## 📄 License

This project is available for educational and personal use.
