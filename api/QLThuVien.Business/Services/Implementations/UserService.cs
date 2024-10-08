﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QLThuVien.Business.Exceptions;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Models;
using System.Data;
using System.Linq.Expressions;

namespace QLThuVien.Business.Services.Implementations;

public class UserService
    (
        IUnitOfWork unitOfWork,
        ILogger<UserService> logger,
        UserManager<User> userManager,
        RoleManager<Role> roleManager
    ) 
    : DataService<User>(unitOfWork, logger), IUserService 
{
    public async Task<IEnumerable<UserVm>> GetAllAsyncVms()
    {
        return (await unitOfWork.GetRepository<User>()
                    .GetQuery().Include(user => user.Roles)
                    .ToListAsync()).Select(user => ToUserVm(user));
    }

    public async Task CreateAsync(UserCreateVm userCreateVm)
    {
        if ((await userManager.FindByEmailAsync(userCreateVm.Email)) != null)
            throw new BadRequestException("Email already exists");

        var role = await roleManager.FindByNameAsync(userCreateVm.Role)
            ?? throw new BadRequestException($"Role {userCreateVm.Role} does not exist");

        var newUser = new User()
        {
            UserName = userCreateVm.Email,
            Email = userCreateVm.Email,
            DateOfBirth = userCreateVm.DateOfBirth,
            FullName = userCreateVm.FullName,
            Gender = userCreateVm.Gender,
            Roles = [role],
        };

        var result = await userManager.CreateAsync(newUser, userCreateVm.Password);

        if (!result.Succeeded)
        {
            var err = string.Concat(result.Errors.Select(e => e.Description + '\n').ToList());
            throw new BadRequestException($"Can't create new user\n{err}");
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var result = await userManager.DeleteAsync(
            await userManager.FindByIdAsync(id.ToString()!)
            ?? throw new BadRequestException("Id not found")
            );

        if (!result.Succeeded)
            throw new BadRequestException("Can't delete user");
    }

    public async Task UpdateAsync(Guid id, UserEditVm userEditVm)
    {
        var user = await userManager.FindByIdAsync(id.ToString()) 
            ?? throw new BadRequestException("id not found");

        user.Id = id;
        user.UserName = userEditVm.Email;
        user.Email = userEditVm.Email;
        user.DateOfBirth = userEditVm.DateOfBirth;
        user.FullName = userEditVm.FullName;
        user.Gender = userEditVm.Gender;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new BadRequestException(string.Concat(
                result.Errors.Select(err => err.Description + '\n')
                ));
    }

    public async Task ChangePasswordAsync(Guid id, string currentPassword, string newPassword)
    {
        var result = await userManager.ChangePasswordAsync(
            await userManager.FindByIdAsync(id.ToString())
                ?? throw new NotFoundException("Id not found"),
            currentPassword,
            newPassword
            );

        if (!result.Succeeded)
            throw new BadRequestException("Can't change password");
    }

    public async Task<UserVm> GetByIdAsyncVm(Guid id)
    {
        var user = await userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new NotFoundException("Id not found");
        return new UserVm()
        {
            Id = user.Id,
            Email = user.Email!,
            DateOfBirth = user.DateOfBirth,
            FullName = user.FullName,
            Gender = user.Gender,
            Role = user.Roles.First().Name!
        };
    }

    public virtual async Task<PaginatedResult<UserVm>> GetAsyncVm(
        int pageIndex = 1,
        int pageSize = 10,
        string includeProperties = "",
        Expression<Func<User, bool>>? filter = null,
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null
        )
    {
        var query = userManager.Users;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrWhiteSpace(includeProperties))
        {
            foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        query = (orderBy != null ? orderBy(query) : query);

        return await PaginatedResult<UserVm>.CreateAsync(
            query.Select(
                user => new UserVm()
                {
                    Id = user.Id,
                    Email = user.Email,
                    DateOfBirth = user.DateOfBirth,
                    FullName = user.FullName,
                    Gender = user.Gender,
                    Role = user.Roles.First().Name!
                })
            , pageIndex, pageSize);
    }

    public async Task AddFavoriteCategoryAsync(Guid userId, Guid categoryId)
    {
        unitOfWork.GetRepository<FavoriteCategory>().Add(new FavoriteCategory()
        {
            UserId = userId,
            CategoryId = categoryId
        });
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteFavoriteCategoryAsync(Guid userId, Guid categoryId)
    {
        unitOfWork.GetRepository<FavoriteCategory>().Delete(userId, categoryId);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<Category>> GetFavoriteCategoriesAsync(Guid userId)
    {
        return await unitOfWork.GetRepository<FavoriteCategory>()
            .Get(fc => fc.UserId == userId, null, "Category")
            .Select(fc => fc.Category).ToListAsync();
    }

    public async Task UpdateFavoriteCategoriesAsync(Guid userId, Guid[] favCategoryIds)
    {
        var repo = unitOfWork.GetRepository<FavoriteCategory>();
        var oldFavCategories = await GetFavoriteCategoriesAsync(userId);

        foreach (var newCatId in favCategoryIds.Except(oldFavCategories.Select(c => c.Id)))
            repo.Add(new FavoriteCategory()
            {
                UserId = userId,
                CategoryId = newCatId,
            });

        foreach (var deletedFavCat in oldFavCategories.ExceptBy(favCategoryIds, c => c.Id))
            repo.Delete(userId, deletedFavCat.Id);

        await unitOfWork.SaveChangesAsync();
    }

    public UserVm ToUserVm(User user)
    {
        return new UserVm()
        {
            Id = user.Id,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            FullName = user.FullName,
            Gender = user.Gender,
            Role = user.Roles.First().Name!
        };
    }
}
