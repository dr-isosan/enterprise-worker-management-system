
# 🏢 Enterprise Worker Management System- BİL309 Database Management System Project

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-8.0-green.svg)](https://docs.microsoft.com/en-us/ef/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-red.svg)](https://www.microsoft.com/en-us/sql-server)
[![Windows Forms](https://img.shields.io/badge/Windows%20Forms-.NET%208.0-lightblue.svg)](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](http://makeapullrequest.com)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)](https://github.com/yourusername/worker-management-system)
[![Code Quality](https://img.shields.io/badge/code%20quality-A+-brightgreen.svg)](https://github.com/yourusername/worker-management-system)

A **professional-grade Employee and Project Management System** built with modern technologies. This enterprise-level desktop application showcases advanced .NET development patterns, Entity Framework Core implementation, and sophisticated UI design with comprehensive project tracking, task management, and employee performance analytics.

## 🌟 Key Features & Architecture

- **🏗️ Modern Architecture**: Built with .NET 8.0 and Entity Framework Core 8.0
- **🎨 Intuitive UI**: Professional Windows Forms interface with dynamic theme system  
- **📊 Real-time Analytics**: Advanced employee performance tracking and project progress monitoring
- **🗄️ Database-First Approach**: Code-first EF Core implementation with SQL Server


## 🚀 Feature Showcase

### 👥 Employee Management

- ✅ **Complete CRUD Operations**: Create, read, update, and delete employee records
- ✅ **Performance Tracking**: Monitor completed and overdue tasks per employee
- ✅ **Project Assignment**: Flexible employee-project relationship management
- ✅ **Performance Analytics**: Visual dashboards for productivity metrics

### 📋 Project Management

- ✅ **Project Lifecycle Management**: Full project creation and tracking capabilities
- ✅ **Timeline Tracking**: Start/end date monitoring with delay analysis
- ✅ **Delay Analytics**: Automated delay calculation and reporting
- ✅ **Resource Allocation**: Advanced employee-project assignment system

### 📝 Task Management

- ✅ **Task Creation & Assignment**: Intuitive task creation with employee assignment
- ✅ **Status Tracking**: Real-time status updates (Active, Completed, Delayed)
- ✅ **Man-Day Calculation**: Sophisticated workload calculation system
- ✅ **Timeline Planning**: Date-based task scheduling and planning

### 🎨 Modern User Interface

- ✅ **Dynamic Themes**: Customizable color themes with instant switching
- ✅ **Responsive Design**: Adaptive layout for different screen sizes
- ✅ **Intuitive Navigation**: User-friendly navigation with modern UX patterns
- ✅ **Advanced Data Views**: Professional DataGridView implementations

## 🏗️ Technology Stack

- **Framework:** .NET 8.0 Windows Forms
- **ORM:** Entity Framework Core 8.0
- **Database:** SQL Server / LocalDB
- **Architecture:** Code First Approach with Repository Pattern
- **Design Patterns:** MVVM, Dependency Injection, Factory Pattern
- **Testing:** NUnit, Moq, Entity Framework In-Memory Provider

## 📁 Project Architecture

```text
WorkerApp/
├── 📁 Model/                    # Data models and DbContext
│   ├── AppDBContext.cs          # Entity Framework DbContext
│   ├── Calisan.cs              # Employee model
│   ├── Proje.cs                # Project model
│   ├── Gorev.cs                # Task model
│   └── ProjeCalisan.cs         # Employee-Project relationship model
├── 📁 Migrations/              # Entity Framework migrations
├── 📁 DBAccess/               # Data access layer
├── 📁 Services/               # Business logic services
├── 📁 Forms/                  # UI Forms
├── 📁 Tests/                  # Unit and integration tests
├── 📁 Properties/             # Application resources
├── Form1.cs                   # Main form
├── Program.cs                 # Application entry point
└── ThemeColor.cs              # Theme color manager
```

## ⚙️ Installation & Setup

### Prerequisites

- **.NET 8.0 SDK** or later
- **SQL Server** / SQL Server Express / LocalDB
- **Visual Studio 2022** (recommended) or Visual Studio Code
- **Git** for version control

### Quick Start

1. **Clone the repository:**

   ```bash
   git clone https://github.com/yourusername/worker-management-system.git
   cd worker-management-system
   ```

2. **Restore dependencies:**

   ```bash
   dotnet restore
   ```

3. **Update database connection:**

   Update the connection string in `Model/AppDBContext.cs` to match your SQL Server instance

4. **Create and migrate database:**

   ```bash
   dotnet ef database update
   ```

5. **Run the application:**

   ```bash
   dotnet run --project WorkerApp
   ```



## 📊 Database Schema

### Core Tables

#### 👥 Employees (Calisanlar)

| Field | Type | Description |
|-------|------|-------------|
| CalisanNo | int (PK) | Unique employee identifier |
| CalisanAdi | string(100) | Employee first name |
| CalisanSoyadi | string(100) | Employee last name |
| TamamlananGorevSayisi | int? | Number of completed tasks |
| GecikenGorevSayisi | int? | Number of overdue tasks |

#### 📋 Projects (Projeler)

| Field | Type | Description |
|-------|------|-------------|
| ProjeNo | int (PK) | Unique project identifier |
| ProjeAdi | string(100) | Project name |
| BaslangicTarihi | DateTime | Start date |
| BitisTarihi | DateTime | End date |
| GecikmeMiktari | int? | Delay amount (days) |

#### 📝 Tasks (Gorevler)

| Field | Type | Description |
|-------|------|-------------|
| GorevNo | int (PK) | Unique task identifier |
| GorevAdi | string(100) | Task name |
| BaslangicTarihi | DateTime | Start date |
| BitisTarihi | DateTime | End date |
| AdamGunDegeri | int | Man-day value |
| Durum | string(20) | Task status |
| ProjeNo | int (FK) | Related project |
| CalisanNo | int (FK) | Assigned employee |

## 🔧 Configuration

### Database Connection

Update the connection string in `AppDBContext.cs`:

```csharp
optionsBuilder.UseSqlServer("YOUR_CONNECTION_STRING_HERE");
```

### Theme Customization

Customize theme colors in `ThemeColor.cs`:

```csharp
public static readonly List<string> ColorList = new List<string>()
{
    "#FF6B6B", "#4ECDC4", "#45B7D1", "#96CEB4", "#FFEAA7"
};
```


## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guidelines](CONTRIBUTING.md) for details.

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/AmazingFeature`)
3. **Commit** your changes (`git commit -m 'Add some AmazingFeature'`)
4. **Push** to the branch (`git push origin feature/AmazingFeature`)
5. **Open** a Pull Request

## 📝 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.







---

⭐ **If you found this project helpful, please consider giving it a star!**

---

*This project demonstrates professional-level software development practices including clean architecture, comprehensive testing, modern UI design, and enterprise-grade code quality.*
>>>>>>> 95eb805a5889fa3c744d6b7456db3a6748af3552
