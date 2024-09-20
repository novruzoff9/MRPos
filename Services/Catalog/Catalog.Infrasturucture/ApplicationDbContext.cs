using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrasturucture;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }
}

//For MongoDb
/*public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMongoDatabase _database;

    public ApplicationDbContext(IMongoClient client)
    {
        _database = client.GetDatabase("CatalogDb");
    }

    public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
}*/
