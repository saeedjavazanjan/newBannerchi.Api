using System.Security.Claims;
using NewBannerchi.Entities;
using NewBannerchi.Repository;

namespace NewBannerchi.Endpoints;

public static class CategoryEndpoints
{
    public static RouteGroupBuilder MapCategoriesEndpoints(this IEndpointRouteBuilder routes)
    {



        var group = routes.MapGroup("/categories").WithParameterValidation();

        //old version
        routes.MapGet("/titles.php", async (IRepository repository)
            => (await repository.GetAllCategoriesAsync())
            .Select(post => post.AsDto()));
        
        
        
        group.MapGet("/", async (IRepository repository)
            => (await repository.GetAllCategoriesAsync())
            .Select(post => post.AsDto()));

  
        group.MapPost("/addCategory",async (
            IRepository iRepository,
            String name,
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
             
              
              
                Category category= new (){
                    Name= name

                };
                await iRepository.CreateCategoryAsync(category);

                return Results.Ok();
            }
            return Results.Conflict( new { error="شما دسترسی ندارید"});



        }).RequireAuthorization().DisableAntiforgery();

        group.MapDelete("/{id}",async (
            IRepository repository,
            int id,
            ClaimsPrincipal? user
        )=>
        {
            var userPhone= user?.FindFirst(ClaimTypes.Email)?.Value;
            if (userPhone == "09193480263")
            {
                var category =await repository.GetCategoryAsync(id);
                if (category is null) return Results.NoContent();
                await repository.DeleteCategoryAsync(id);
                return Results.Ok();
            }

            return Results.Conflict( new { error="شما دسترسی ندارید"});



        }).RequireAuthorization();
        


        return group;
    }
}