using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Common;

public class BaseEntity
{
    [MaxLength(6)]
    public string Id { get; set; }
}
