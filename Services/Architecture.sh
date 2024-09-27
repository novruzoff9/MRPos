#!/bin/bash

# Kullanıcıdan proje adını al
echo "Lütfen proje adını girin:"
read projectName

# Proje klasörünü oluştur
mkdir $projectName
cd $projectName

# Class Library projelerini oluştur
dotnet new classlib -n "$projectName.Domain"
dotnet new classlib -n "$projectName.Infrastructure"
dotnet new classlib -n "$projectName.Application"

# Web API projesini controller'larla birlikte oluştur
dotnet new webapi -n "$projectName.WebAPI" --no-https

# Domain klasörü ve içindekiler
cd "$projectName.Domain"
mkdir -p Common Constants Entities Enums Events Exceptions ValueObjects
cd ..

# Infrastructure klasörü ve içindekiler
cd "$projectName.Infrastructure"
mkdir -p Data/Configurations Data/Interceptors
touch DependencyInjection.cs GlobalUsings.cs

# ApplicationDbContext sınıfını oluştur ve DbContext'ten miras al
cat <<EOL > Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;

namespace $projectName.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

}
EOL

cd ..

# Application klasörü ve içindekiler
cd "$projectName.Application"
mkdir -p Common/Behaviors Common/Exceptions Common/Interfaces Common/Mapping Common/Models
touch DependencyInjection.cs GlobalUsings.cs
cd ..

# Entity Framework paketlerini tüm projelere yükle
dotnet add "$projectName.Domain/$projectName.Domain.csproj" package Microsoft.EntityFrameworkCore
dotnet add "$projectName.Infrastructure/$projectName.Infrastructure.csproj" package Microsoft.EntityFrameworkCore
dotnet add "$projectName.Application/$projectName.Application.csproj" package Microsoft.EntityFrameworkCore

# Web API projesine Entity Framework paketlerini yükle
dotnet add "$projectName.WebAPI/$projectName.WebAPI.csproj" package Microsoft.EntityFrameworkCore
dotnet add "$projectName.WebAPI/$projectName.WebAPI.csproj" package Microsoft.EntityFrameworkCore.SqlServer
dotnet add "$projectName.WebAPI/$projectName.WebAPI.csproj" package Microsoft.EntityFrameworkCore.Tools

# Proje yapısını tamamla
echo "Proje yapısı başarıyla oluşturuldu ve Entity Framework Core paketleri yüklendi."
