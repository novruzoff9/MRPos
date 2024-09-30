using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public string CompanyId { get; set; }
    public List<Product>? Products { get; set; }
}
