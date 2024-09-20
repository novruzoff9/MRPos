using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Exceptions;

public class UnsoppertedNameException : Exception
{
    public UnsoppertedNameException(string name) : base($"Category Name {name} is unsupported")
    {
        
    }
}
