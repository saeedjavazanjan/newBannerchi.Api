using Microsoft.EntityFrameworkCore;
using NewBannerchi.Data;
using NewBannerchi.Entities;

namespace NewBannerchi.Repository;

public class EntityFrameWorkRepository(NewBannerchiContext dbContext) : IRepository
{
    public Task CreateAsync(Package package)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Package?> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Package>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

   

    public async Task<IEnumerable<Package>> GetPostersAsync()
    {
        return await dbContext.Packages.Where(package => 
                package.Type=="0" ||
                package.Type=="1")
            .ToListAsync();
        
    }
   public async Task<IEnumerable<Package>> GetTempsAsync()
    {
        return await dbContext.Packages.Where(package => package.Type=="3")
            .ToListAsync();
        
    }

   
    
    public Task UpdateAsync(Package updatedPackage)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Package>> SearchAsync(string query)
    {
        IQueryable<Package> data = dbContext.Packages;

        return await data.Where(data =>
                data.Name.Contains(query) ||
                data.Category.Contains(query) ||
                data.Designer.Contains(query))

            .ToListAsync();    }

    public async Task<IEnumerable<Package>> GetWithCategoryAsync(string category)
    {
        IQueryable<Package> data = dbContext.Packages;

        return await data.Where(data => data.Category.Contains(category)).ToListAsync();
        
    }
    
    
    //Users
    public async Task<User?> GetUserAsync(int id)
    {
        return await dbContext.Users.FindAsync(id);

    }
    public async Task<User?> GetRegesteredPhoneNumberAsync(string phoneNumber)
    {
        try
        {
            return await dbContext.Users.FirstOrDefaultAsync(user => user.PhoneNumber == phoneNumber);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task AddUser(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteUser(int id)
    {
        await dbContext.Users.Where(user => user.Id == id)
            .ExecuteDeleteAsync();
    }
}
