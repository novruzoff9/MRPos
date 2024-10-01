using Microsoft.EntityFrameworkCore;
using Organization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Table> Tables { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
