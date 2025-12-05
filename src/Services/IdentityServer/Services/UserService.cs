using FluentValidation;
using IdentityServer.Context;
using IdentityServer.DTOs;
using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;

namespace IdentityServer.Services;

public interface IUserService
{
    Task<UserShowDto> CreateUser(CreateUserDto request);
    Task<UserShowDto> UpdateUser(string userId, UpdateUserDto requet);
    Task<UserDetailedDto> GetUserByEmailAsync(string email);
    Task<UserDetailedDto> GetUserByIdAsync(string id);
    Task<IdentityRole> GetUserRole(string userId);
    Task<List<UserShowDto>> GetUsersAync();
    Task<bool> ChangePassword(ChangePasswordDto request);
}
public class UserService(
    IdentityDbContext context, 
    IValidator<CreateUserDto> createUserValidator, 
    IValidator<UpdateUserDto> updateUserValidator
    ) : IUserService
{
    public async Task<UserShowDto> CreateUser(CreateUserDto request)
    {
        var valResult = createUserValidator.Validate(request);
        if (!valResult.IsValid)
            throw new ValidationException(valResult.Errors);
        
        var user = new IdentityUser(request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.Password, request.CompanyId, request.BranchId);
        var existingUser = await context.Users.AnyAsync(u => u.NormalizedEmail == user.NormalizedEmail);
        if (existingUser)
            throw new ConflictException("Bu email istifadə olunub");
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        var userDto = new UserShowDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Roles = user.Roles.Select(r => r.Role.RoleName).ToList()
        };
        return userDto;
    }

    public async Task<UserDetailedDto> GetUserByEmailAsync(string email)
    {
        var user = await context.Users
            .Include(u => u.Roles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.NormalizedEmail == email.Trim().ToLower());
        if (user == null)
            throw new NotFoundException("İstifadəçi tapılmadı");
        var userDto = new UserDetailedDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            HashedPassword = user.HashedPassword,
            Roles = user.Roles.Select(r => r.Role.RoleName).ToList()
        };
        return userDto;
    }

    public async Task<IdentityRole> GetUserRole(string userId)
    {
        var userRole = await context.UserRoles
            .Include(ur => ur.Role)
            .FirstOrDefaultAsync(ur => ur.UserId == userId);
        if (userRole == null)
            throw new Exception("İstifadəçinin rolu tapılmadı");
        return userRole!.Role!;
    }

    public async Task<List<UserShowDto>> GetUsersAync()
    {
        var users = await context.Users
            .Include(u => u.Roles)
            .ThenInclude(ur => ur.Role)
            .ToListAsync();

        return users.Select(u => new UserShowDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            Roles = u.Roles.Select(r => r.Role.RoleName).ToList()
        }).ToList();
    }

    public async Task<UserShowDto> UpdateUser(string userId, UpdateUserDto request)
    {
        var valResult = updateUserValidator.Validate(request);
        if (!valResult.IsValid)
        {
            throw new ValidationException(valResult.Errors);
        }
        if(userId != request.UserId)
        {
            throw new Exception("Göndərilən Id dəyəri uyğun deyil");
        }
        var user = await context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new Exception("İstifadəçi tapılmadı");
        }
        else
        {
            if (user.Email != request.Email)
            {
                var existingUser = await context.Users
                    .AnyAsync(u => u.NormalizedEmail.Equals(request.Email.Trim(), StringComparison.CurrentCultureIgnoreCase));
                if (existingUser)
                    throw new Exception($"Dəyişdirmək istədiyiniz email istifadə olunub");
            }
            user.Update(request.FirstName, request.LastName, request.Email, request.PhoneNumber);
            await context.SaveChangesAsync();
            var userDto = new UserShowDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = user.Roles.Select(r => r.Role.RoleName).ToList()
            };
            return userDto;
        }
    }

    public async Task<bool> ChangePassword(ChangePasswordDto request)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
        if (user == null)
            throw new NotFoundException("İstifadəçi tapılmadı");
        var pass = BCrypt.Net.BCrypt.Verify(request.OldPassword, user.HashedPassword);
        if (!pass)
            throw new ValidationException("Köhnə şifrə yanlışdır");
        user.ChangePassword(request.NewPassword);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<UserDetailedDto> GetUserByIdAsync(string id)
    {
        var user = await context.Users
            .Include(u => u.Roles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            throw new NotFoundException("İstifadəçi tapılmadı");
        var userDto = new UserDetailedDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            HashedPassword = user.HashedPassword,
            Roles = user.Roles.Select(r => r.Role.RoleName).ToList()
        };
        return userDto;
    }
}
