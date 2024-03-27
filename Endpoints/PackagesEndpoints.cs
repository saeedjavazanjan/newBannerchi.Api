using System.Security.Claims;
using NewBannerchi.Entities;
using NewBannerchi.Repository;

namespace NewBannerchi.Endpoints;

public static class PackagesEndpoints
{
    private const  string SearchPackage="Search";
    private const  string Category="Category";
    public static RouteGroupBuilder MapPackagesEndpoints(this IEndpointRouteBuilder routes)
    {



        var group = routes.MapGroup("/packages").WithParameterValidation();
        
        
        //old version
        routes.MapGet("/show_news.php", async (IRepository repository, string occasion)
            =>
        {
            if (occasion == "همه")
            {
                return (await repository.GetAllAsync())
                    .Select(package => package.AsDto());  
            }
            return (await repository.GetWithCategoryAsync(occasion))
                .Select(package=>package.AsDto());
        });
        
        
        routes.MapGet("/download_counting.php",
            async (IRepository repository, 
                    int id,
                    int count)
            =>
        {
            Package? existedPackage = await repository.GetAsync(id);


            if (existedPackage != null)
            {
                existedPackage.DownloadCount =(Int32.Parse(existedPackage.DownloadCount)+1).ToString();
                await  repository.UpdateAsync(existedPackage);
                return Results.Ok();
            }

            return Results.NotFound(new{error="پکیج مورد نظر یافت نشد"}); 
 
        });
        routes.MapGet("/search.php", async (
                IRepository repository,
                string search)
            =>
        {
            IEnumerable<Package> searchedPackages = await repository.SearchAsync(search);
            return searchedPackages is not null ? Results.Ok(searchedPackages
              .Select(post=>post.AsDto())):Results.NotFound();
        });
        

        //.
        
        
        group.MapGet("/posters", async (IRepository repository,int pageNumber,int pageSize) 
            => (await repository.GetPostersAsync())
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(package=>package.AsDto()));
 
        group.MapGet("/temps", async (IRepository repository,int pageNumber,int pageSize) 
            => (await repository.GetTempsAsync())
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(package=>package.AsDto()));


        group.MapGet("/category", async (
                IRepository repository,
                string category,
                int pageNumber,
                int pageSize)
            =>
        {
            IEnumerable<Package> categoryPackages = await repository.GetWithCategoryAsync(category);
            return categoryPackages is not null ? Results.Ok(categoryPackages
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(package=>package.AsDto())):Results.NotFound();
        }).WithName(Category);
         
        group.MapGet("/search", async (
                IRepository repository,
                string query,
                int pageNumber,
                int pageSize)
            =>
        {
            IEnumerable<Package> searchedPosts = await repository.SearchAsync(query);
            return searchedPosts is not null ? Results.Ok(searchedPosts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).Select(post=>post.AsDto())):Results.NotFound();
        }).WithName(SearchPackage);
        
        
           group.MapPost("/uploadPackage",async (
            IRepository iRepository,
             AddPackageDto addPackageDto,
            ClaimsPrincipal? user
            )=>{
          var userId= user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          var userPhone= user?.FindFirst(ClaimTypes.Email)?.Value;

          User? currentUser = await iRepository.GetUserAsync(Int32.Parse(userId));
          
          if(currentUser==null){
              return Results.NotFound(new{error="کاربر یافت نشد."}); 
          }
          var userName = currentUser.Name;
          if (userPhone=="09193480263")
          {
             
              
              
              Package package= new (){
                  Name= addPackageDto.Name,
                  Designer = addPackageDto.Designer,
                  Type= addPackageDto.Type,
                  DownloadCount = addPackageDto.DownloadCount,
                  Samples = addPackageDto.Samples,
                  HeaderUrl = addPackageDto.HeaderUrl,
                  PackageUrl = addPackageDto.PackageUrl,
                  Price = addPackageDto.Price,
                  Category = addPackageDto.Category

              };
              await iRepository.CreateAsync(package);

              return Results.Ok();
          }
          return Results.Conflict( new { error="شما دسترسی ندارید"});



        }).RequireAuthorization().DisableAntiforgery();

        group.MapPut("update/{id}",async (
                IRepository repository,
                int id, 
                AddPackageDto updatePackageDto,
                ClaimsPrincipal? user)=>
            {
                var userPhone= user?.FindFirst(ClaimTypes.Email)?.Value;
                
                Package? existedPackage = await repository.GetAsync(id);

                if(existedPackage==null){
                    return Results.NotFound(new{error="پست مورد نظر یافت نشد"}); 
                }

                if (userPhone == "09193480263")
                {
                    existedPackage.Name=updatePackageDto.Name;
                    existedPackage.Category = updatePackageDto.Category;
                    existedPackage.Designer =updatePackageDto.Designer ;
                    existedPackage.Type=updatePackageDto.Type;
                    existedPackage.DownloadCount=updatePackageDto.DownloadCount;
                    existedPackage.Samples=updatePackageDto.Samples;
                    existedPackage.HeaderUrl=updatePackageDto.HeaderUrl;
                    existedPackage.PackageUrl=updatePackageDto.PackageUrl;
                    existedPackage.Price=updatePackageDto.Price;
                
                    await  repository.UpdateAsync(existedPackage);
                    return Results.Ok("با موفقیت به روز رسانی شد");   
                }
                return Results.Conflict( new { error="شما دسترسی ندارید"});

             

            }
        ).RequireAuthorization();

        group.MapDelete("/{id}",async (
            IRepository repository,
            int id,
            ClaimsPrincipal? user
            )=>
        {
            var userPhone= user?.FindFirst(ClaimTypes.Email)?.Value;
            if (userPhone == "09193480263")
            {
                var package =await repository.GetAsync(id);
                if (package is null) return Results.NoContent();
                await repository.DeleteAsync(id);
                return Results.Ok();
            }

            return Results.Conflict( new { error="شما دسترسی ندارید"});



        }).RequireAuthorization();

        
        
        return group;
    }
    
    
}