using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Enums;

public enum ProductStatus
{
    None = 0,
    New = 1,
    Popular = 2,
    NewPopular = New | Popular
}
