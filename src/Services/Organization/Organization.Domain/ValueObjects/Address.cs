using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.ValueObjects;

public class Address : ValueObject
{
    public string? GoogleMapsLocation { get; set; }
    public string? GoogleMapsEmbed { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? Street { get; set; }
}
