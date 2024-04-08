using Azure.Core;
using Microsoft.EntityFrameworkCore;
using NewBannerchi.Data;
using NewBannerchi.Entities;

namespace NewBannerchi.Repository;

public class EntityFrameWorkRepository(NewBannerchiContext dbContext) : IRepository
{
    public async Task CreateAsync(Package package)
    {
        dbContext.Packages.Add(package);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await dbContext.Packages.Where(package => package.Id == id)
            .ExecuteDeleteAsync();
        
    }

    public async Task<Package?> GetAsync(int id)
    {
        return await dbContext.Packages.FindAsync(id);
    }

    public async Task<IEnumerable<Package>> GetAllAsync()
    {
        return await dbContext.Packages.AsNoTracking().ToListAsync();
    }

    /*public async Task<List<OldPackages>> GetOldAllAsync()
    {
        List<OldPackages> packages=new List<OldPackages>();
        await foreach (var dbContextPackage in dbContext.Packages)
        {
            OldPackages pack = new OldPackages()
            {
                Name = dbContextPackage.Name,
                Designer = dbContextPackage.Designer,
                Type = dbContextPackage.Type,
                DownloadCount = dbContextPackage.DownloadCount,
                Samples = dbContextPackage.Samples,
                HeaderUrl = dbContextPackage.HeaderUrl,
                PackageUrl = dbContextPackage.PackageUrl,
                Price = dbContextPackage.Price,
                Category = dbContextPackage.Category[0]
            };
            packages.Add(pack);
        }

        return  packages;

    }
    */
    

    public async Task<IEnumerable<Package>> GetPostersAsync()
    {
         return await dbContext.Packages.Where(package =>
            package.Type == "0" ||
            package.Type == "1").ToListAsync();

         /*return await PaginatedList<Package>.CreateAsync(dbContext.Packages.Where(package =>
                 package.Type == "0" ||
                 package.Type == "1").AsNoTracking(),
             pageNumber ,
             pageSize);*/
    }
   public async Task<IEnumerable<Package>> GetTempsAsync()
    {
        return await dbContext.Packages.Where(package => package.Type=="3")
            .ToListAsync();
        
    }

   
    
    public async Task UpdateAsync(Package updatedPackage)
    {
        dbContext.Update(updatedPackage);
        await dbContext.SaveChangesAsync();
        
    }

    public async Task<IEnumerable<Package>> SearchAsync(string query)
    {
        IQueryable<Package> data = dbContext.Packages;

        return await data.Where(data =>
                data.Name.Contains(query) )
            .ToListAsync();    }

    public async Task<IEnumerable<Package>> GetWithCategoryAsync(string category)
    {
        IQueryable<Package> data = dbContext.Packages;

        return await data.Where(data => data.Category.Contains(category)).ToListAsync();
        
    }

    public async Task<IEnumerable<Package>> GetFilteredPackagesAsync(  string searchTerm,string packageType)
    {
        searchTerm = searchTerm.Trim().ToLower();
        packageType = packageType.Trim().ToLower();

        IQueryable<Package> data = dbContext.Packages;
        if (packageType.Equals("poster"))
        {
            if (searchTerm.Equals("همه"))
            {
                return await data.Where(
                    data => 
                        data.Type.Equals("0")||
                        data.Type.Equals("1")
                ).ToListAsync(); 
            }
            else
            {
                return await data.Where(
                    data => 
                       ( data.Type.Equals("0")||
                        data.Type.Equals("1"))&&
                        (data.Category.Contains(searchTerm)||
                            data.Name.Contains(searchTerm)||
                            data.Designer.Contains(searchTerm))
                          
                            ).ToListAsync();
            }
            
        }
        else 
        {
            if (searchTerm.Equals("همه"))
            {
                return await data.Where(
                    data => 
                        data.Type.Equals("3")
                ).ToListAsync(); 
            }
            else
            {
                return await data.Where(
                    data =>
                        data.Type.Equals("3")&&
                        (data.Category.Contains(searchTerm)||
                            data.Name.Contains(searchTerm)||
                            data.Designer.Contains(searchTerm))
                ).ToListAsync();
            }
        }
       
        
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

    public async Task AddUserOtp(UserOtp userOtp)
    {
        dbContext.UsersOtp.Add(userOtp);
        await dbContext.SaveChangesAsync();
        
        
    }

    public async Task<UserOtp?> GetUserOtpAsync(string userPhoneNumber)
    {
        try
        {
            return await dbContext.UsersOtp.FirstOrDefaultAsync(userOtp => userOtp.PhoneNumber == userPhoneNumber);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteUser(int id)
    {
        await dbContext.Users.Where(user => user.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task UpdateUserOtpAsync(UserOtp userOtp)
    {
        dbContext.Update(userOtp);
        await dbContext.SaveChangesAsync();
        
    }

    public async Task DeleteUserOtpAsync(int id)
    {
        await dbContext.UsersOtp.Where(user => user.Id == id)
            .ExecuteDeleteAsync();
        
    }


    //categories
    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await dbContext.Categories.AsNoTracking().ToListAsync();
    }

    public async Task CreateCategoryAsync(Category category)
    {
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        await dbContext.Categories.Where(category => category.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<Category?> GetCategoryAsync(int id)
    {
        return await dbContext.Categories.FindAsync(id);
    }

    
    //occasion
    public async Task<IEnumerable<Occasion>> GetAllOccasionsAsync()
    {
        return await dbContext.Occasions.AsNoTracking().ToListAsync();
    }

    public async Task CreateOccasionAsync(Occasion occasion)
    {
        dbContext.Occasions.Add(occasion);
        await dbContext.SaveChangesAsync();
        
    }

    public async Task DeleteOccasionAsync(int id)
    {
        await dbContext.Occasions.Where(occasion => occasion.Id == id)
            .ExecuteDeleteAsync();    }

    public async Task<Occasion?> GetOccasionAsync(int id)
    {
        return await dbContext.Occasions.FindAsync(id);
    }

    
    
    //downloadDetail
    public async Task AddDownloadDetail(DownLoadDetail downLoadDetail)
    {
        dbContext.DownLoadDetail.Add(downLoadDetail);
        await dbContext.SaveChangesAsync();
        
    }
}
