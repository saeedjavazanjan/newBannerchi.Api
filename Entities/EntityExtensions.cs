namespace NewBannerchi.Entities;

public static class EntityExtensions
{

    public static PackageDto AsDto(this Package package)
    {
        return new PackageDto(
            package.Id,
            package.Name,
            package.Designer,
            package.Type,
            package.DownloadCount,
            package.Samples,
            package.HeaderUrl,
            package.PackageUrl,
            package.Price,
            package.Category
        );


    } 
    public static UserDto AsDto(this User user)
    {
        return new UserDto(
            user.Id,
            user.Name,
            user.PhoneNumber,
            user.TypeOfPage,
            user.JobTitle
        );


    }
    public static OccasionDto AsDto(this Occasion occasion)
    {
        return new OccasionDto(
            occasion.Id,
            occasion.Name
            
        );


    } 
    public static CategoryDto AsDto(this Category category)
    {
        return new CategoryDto(
            category.Id,
            category.Name
          
        );


    } 
    public static UserOtpDto AsDto(this UserOtp userOtp)
    {
        return new UserOtpDto(
            userOtp.Id,
            userOtp.UserName,
            userOtp.OtpPassword,
            userOtp.PhoneNumber,
            userOtp.Time
          
        );


    }

    public static DownloadDetailDto AsDto(this DownLoadDetail downLoadDetail)
    {
        return new DownloadDetailDto(
            downLoadDetail.Id,
            downLoadDetail.PackName,
            downLoadDetail.PackId,
            downLoadDetail.UserId,
            downLoadDetail.UserPurchaseToken,
            downLoadDetail.Time
        );

    }
}