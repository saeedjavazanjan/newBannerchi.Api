using System.ComponentModel.DataAnnotations;

namespace NewBannerchi;

public record PackageDto(
    int Id,
    string Name,
    string Designer,
    string Type,
    string DownloadCount,
    string Samples,
    string HeaderURL,
    string PackageURL,
    int  Price,
    string Category
   
);


public record AddPackageDto(
    string Name,
    string Designer,
    string Type,
    string DownloadCount,
    string Samples,
    string HeaderUrl,
    string PackageUrl,
    int  Price,
    string Category
   
);
public record UserDto(
    int Id,
    string Name,
    string PhoneNumber,
    string TypeOfPage,
    string JobTitle
);
public record RegisterUserDto(
    [Required][StringLength(20)] string Name,
    [Required][StringLength(12)] string PhoneNumber,
    [Required][StringLength(20)] string TypeOfPage,
    [StringLength(20)] string JobTitle
);

public record AddUserDto(
    [StringLength(20)] string UserName,
    [Required] [StringLength(4)] string Password,
    [Required] [StringLength(12)] string PhoneNumber,
    [Required][StringLength(20)] string TypeOfPage,
    [StringLength(20)] string JobTitle
);
public record SignInUserDto(
    [Required] [StringLength(12)] string PhoneNumber
);


public record CategoryDto(
    int Id,
    string Name
    
);
public record OccasionDto(
    int Id,
    string Name
);
public record UserOtpDto(
    int Id,
    string UserName,
    string OtpPassword,
    string UserPhoneNumber,
    long Time
);