using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.Services;

public  class GenerateOrganizationId
{
    private readonly IApplicationDbContext _context;
    private static readonly Random _random = new Random();
    private static readonly string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public GenerateOrganizationId(IApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<string> GenerateUniqueCompanyIdAsync()
    {
        string newId;

        do
        {
            newId = GenerateRandomId(4);
        }
        while (await _context.Companies.AnyAsync(c => c.Id == newId));

        return newId;
    }

    public async Task<string> GenerateUniqueBranchIdAsync()
    {
        string newId;

        do
        {
            newId = GenerateRandomId(6);
        }
        while (await _context.Branches.AnyAsync(c => c.Id == newId));

        return newId;
    }

    private string GenerateRandomId(int length)
    {
        return new string(Enumerable.Repeat(_chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}
