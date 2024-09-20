using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Category> Categories { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}


//For MongoDb
/*public interface IApplicationDbContext
{
    IMongoCollection<Category> Categories { get; }
}*/
