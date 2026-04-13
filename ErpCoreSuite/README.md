# 🏢 ERP Core Suite

> A full-stack ERP demo system built with **ASP.NET Core 6 Web API**, **MS SQL Server**, and **Bootstrap 5**.  
> Developed to showcase real-world enterprise patterns used across 12+ years of ERP delivery.

![.NET](https://img.shields.io/badge/.NET-6.0-purple) ![SQL Server](https://img.shields.io/badge/SQL%20Server-2014%2B-blue) ![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-teal) ![License](https://img.shields.io/badge/license-MIT-green)

---

## 📦 Modules

| Module | Features |
|---|---|
| **Inventory** | Products, Categories, Stock In/Out, Low Stock Alerts |
| **Payroll** | Employees, Salary Calculation, Allowances, Deductions, Payslip |
| **Accounting** | Chart of Accounts, Journal Entries, Ledger, Trial Balance |
| **Auth** | JWT Login, Role-based Access (Admin / HR / Accountant) |
| **Reporting** | HTML printable reports rendered via C# engine |

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Backend | C# · ASP.NET Core 6 Web API |
| Database | MS SQL Server · Stored Procedures · Views · Query Optimisation |
| Frontend | HTML5 · Bootstrap 5.3 · JavaScript (ES6+) · jQuery · AJAX |
| Auth | JWT Bearer Token · Role Claims |
| Reporting | HTML/C# Rendering Engine (print-ready) |

---

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2022+
- .NET 6 SDK
- SQL Server 2014+

### Setup

```bash
# 1. Clone the repo
git clone https://github.com/syed-sulaiman-dev/erp-core-suite.git

# 2. Run the SQL setup scripts
#    Open SQL Server Management Studio
#    Run: ErpCoreSuite.Data/Scripts/01_CreateDatabase.sql
#    Run: ErpCoreSuite.Data/Scripts/02_SeedData.sql

# 3. Update connection string
#    Edit: ErpCoreSuite.API/appsettings.json

# 4. Run the API
cd ErpCoreSuite.API
dotnet run
```

### Default Login
| Role | Username | Password |
|---|---|---|
| Admin | admin | Admin@123 |
| HR | hruser | Hr@123 |
| Accountant | accountant | Acc@123 |

---

## 📁 Project Structure

```
ErpCoreSuite/
├── ErpCoreSuite.API/          # ASP.NET Core Web API
│   ├── Controllers/           # Inventory, Payroll, Accounting, Auth
│   ├── Models/                # Request/Response models
│   ├── Middleware/            # JWT, Exception handling
│   └── appsettings.json
├── ErpCoreSuite.Core/
│   ├── Entities/              # Domain entities
│   └── Interfaces/            # Repository contracts
├── ErpCoreSuite.Data/
│   ├── Repositories/          # SQL Server data access
│   └── Scripts/               # DB creation + seed scripts
└── ErpCoreSuite.Web/
    └── wwwroot/               # Bootstrap 5 frontend
        ├── pages/             # Inventory, Payroll, Accounting pages
        ├── css/               # Custom styles
        └── js/                # API integration scripts
```

---

## 🖥️ Screenshots

> Dashboard → Inventory → Payroll → Reporting

*(Add screenshots here after running locally)*

---

## 👤 Author

**K. Syed Sulaiman** — Senior Software Developer · ERP Specialist  
12+ years delivering enterprise ERP systems across Kuwait and India.

📧 Sulaimanjaila91@gmail.com  
🌐 [github.com/syed-sulaiman-dev](https://github.com/syed-sulaiman-dev)

---

## 📄 License

MIT License — free to use for learning and portfolio purposes.
