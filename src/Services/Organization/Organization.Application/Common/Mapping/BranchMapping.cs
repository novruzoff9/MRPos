using AutoMapper;
using Organization.Application.Common.Models.Branch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.Mapping;

public class BranchMapping : Profile
{
    public BranchMapping()
    {
        CreateMap<Branch, BranchSummaryDto>().ReverseMap();
    }
}
