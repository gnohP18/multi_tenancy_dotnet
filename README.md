# Multi tenancy project
## Author: Shubanoname - Nguyen Hoang Phong 
If you interested in this project, please give me a star ⭐️ on github
[gnohP18 - Github](https://github.com/gnohP18)<br>
[Linkedin](https://www.linkedin.com/in/nguyen-hoang-phong-shuba/)<br>
[nhphong1804@gmail.com ✉️](nhphong1804@gmail.com)

## Table content
- [Multi tenancy project](#multi-tenancy-project)
  - [Author: Shubanoname - Nguyen Hoang Phong](#author-shubanoname---nguyen-hoang-phong)
  - [Table content](#table-content)
  - [What is Multi tenancy](#what-is-multi-tenancy)
  - [Project requirement](#project-requirement)
  - [How to install](#how-to-install)
    - [1. Clone the project](#1-clone-the-project)
    - [2. Build project](#2-build-project)
    - [3. Migrate project](#3-migrate-project)
    - [4. Run project](#4-run-project)
    - [5. Follow these step to migrate tenants by using swagger](#5-follow-these-step-to-migrate-tenants-by-using-swagger)
  - [Run with `Docker compose`](#run-with-docker-compose)
    - [Run command below](#run-command-below)
  - [Command for migrate](#command-for-migrate)
    - [1 Create migration for landlord](#1-create-migration-for-landlord)
    - [2 To update migration for landlord](#2-to-update-migration-for-landlord)
    - [3 Create migration for tenant](#3-create-migration-for-tenant)
    - [4 Remove last migration for tenant](#4-remove-last-migration-for-tenant)

## What is Multi tenancy 
![Multi Tenant](/Description/multi_tenancy.png "Multi Tenant")

## Project requirement
| Service  | Version | Port   | Used for |
| :------- |:----| :----: | :-------- |
| .NET |8.0| 5105 | Main project, .NET |
| Mysql |8.4.0| 3306 | Contains tenants, database for tenant and landlord|
| Redis |7.4.1| 6379 | Check tenant domain|

## How to install
### 1. Clone the project
```bash
git clone git@github.com:gnohP18/multi_tenancy_dotnet.git
```

### 2. Build project
```bash
dotnet build
```

### 3. Migrate project 
```bash
dotnet ef database update --context LandlordContext
```

### 4. Run project
```bash
dotnet watch run
```

### 5. Follow these step to migrate tenants by using swagger
  - Go to mysql database and get `id` of the tenant
  - Access the swagger Landlord stage by link [Landlord]("http://localhost:5105/swagger/index.html?urls.primaryName=Landlord+API+V1")
  - Paste `id` of the tenant into the API `/tenants/migrate/{id}` to migrate tenant 

## Run with `Docker compose`
### Run command below
Note: You must stay in root folder to run this command
```bash
docker-compose up -d
```

## Command for migrate
### 1 Create migration for landlord
```bash
dotnet ef migrations add {migration_name} --context LandlordContext
```

### 2 To update migration for landlord
```bash
dotnet ef database update --context LandlordContext
```

### 3 Create migration for tenant
```bash
dotnet ef migrations add {migration_name} -o Migrations/Tenants --context CreatingContext
```

### 4 Remove last migration for tenant
```bash
dotnet ef migrations remove --context CreatingContext
```