using Catalog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities;

public class Product : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public ProductStatus Status { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
