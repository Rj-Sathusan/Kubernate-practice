
# 🚀 DevPro - ASP.NET Core Project

DevPro is an ASP.NET Core application using **PostgreSQL** as the database, built with clean architecture and modular design principles. This guide provides a clear workflow for managing database migrations using **Entity Framework Core**.

---

## 📦 Technologies Used

* **ASP.NET Core**
* **Entity Framework Core**
* **PostgreSQL**
* **EF Core Tools (CLI)**

---

## ▶️ Run the Application

Use the following commands based on your environment:

### 🌐 Production (HTTPS)

```bash
dotnet run --launch-profile "Production (https)" --project DevPro.Api
```

### 🧪 Development

```bash
dotnet run --launch-profile "Development" --project DevPro.Api
```

### 🐞 Local Testing

```bash
dotnet run --launch-profile "Local" --project DevPro.Api
```

Make sure the corresponding launch profiles are correctly configured in `launchSettings.json` under the `DevPro.Api` project.

---

## ⚙️ EF Core Migration Workflow

### ➕ Add New Migration

```bash
dotnet ef migrations add <MigrationName> --project DevPro.Database --startup-project DevPro.Api
```

### 🧱 Example: First Migration

```bash
dotnet ef migrations add InitialCreate --project DevPro.Database --startup-project DevPro.Api
dotnet ef database update --project DevPro.Database --startup-project DevPro.Api
```

---

### 🔄 Update Database

```bash
dotnet ef database update --project DevPro.Database --startup-project DevPro.Api
```

---

### 🆕 Example: Add Feature Migration

```bash
dotnet ef migrations add Add<FeatureName>Tables --project DevPro.Database --startup-project DevPro.Api
dotnet ef database update --project DevPro.Database --startup-project DevPro.Api
```

---

### ❌ Remove Last Migration (Before Applying)

```bash
dotnet ef migrations remove --project DevPro.Database --startup-project DevPro.Api
```

---

## 💣 Danger Zone: Reset Database Completely

### ⬅️ Rollback All Migrations

```bash
dotnet ef database update 0 --project DevPro.Database --startup-project DevPro.Api
```

### 🧹 Remove All Migrations

```bash
dotnet ef migrations remove --project DevPro.Database --startup-project DevPro.Api
```

### 🧼 Recreate Fresh Migration

```bash
dotnet ef migrations add InitialCreateV2 --project DevPro.Database --startup-project DevPro.Api
dotnet ef database update --project DevPro.Database --startup-project DevPro.Api
```

---

## 👨‍💻 Developed By

**Diranujan**
📧 Email: [diranujan2000@gmail.com](mailto:diranujan2000@gmail.com) | [jdiranujan@microwe.net](mailto:jdiranujan@microwe.net)
