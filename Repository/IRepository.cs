using NewBannerchi.Entities;

namespace NewBannerchi.Repository;

public interface IRepository
{
    Task CreateAsync(Package package);
    Task DeleteAsync(int id);

    Task<Package?> GetAsync(int id);

    Task<IEnumerable<Package>> GetAllAsync();
   // Task<List<OldPackages>> GetOldAllAsync();
    
    Task<IEnumerable<Package>> GetPostersAsync();
    Task<IEnumerable<Package>> GetTempsAsync();
    
    Task UpdateAsync(Package updatedPackage);

    Task<IEnumerable<Package>> SearchAsync(string query);

    Task<IEnumerable<Package>> GetWithCategoryAsync(string category);

    Task<IEnumerable<Package>> GetFilteredPackagesAsync(
        string searchTerm,
        string packageType
    );
    
    //users
    Task<User?> GetUserAsync(int id);
    Task<User?> GetRegesteredPhoneNumberAsync(string phoneNumber);
    Task AddUser (User user);
    Task DeleteUser(int id);

    Task AddUserOtp ( UserOtp userOtp);
    Task<UserOtp?> GetUserOtpAsync(string userPhoneNumber);
    Task UpdateUserOtpAsync(UserOtp userOtp);
    Task DeleteUserOtpAsync(int id);


    
    //categories
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task CreateCategoryAsync(Category category);
    Task DeleteCategoryAsync(int id);
    Task<Category?> GetCategoryAsync(int id);
 
    //occasions
    Task<IEnumerable<Occasion>> GetAllOccasionsAsync();
    Task CreateOccasionAsync(Occasion occasion);
    Task DeleteOccasionAsync(int id);
    Task<Occasion?> GetOccasionAsync(int id);


    
}