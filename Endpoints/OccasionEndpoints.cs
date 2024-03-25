using System.Security.Claims;
using NewBannerchi.Entities;
using NewBannerchi.Repository;

namespace NewBannerchi.Endpoints;

public static class OccasionEndpoints
{
        public static RouteGroupBuilder MapOccasionEndpoints(this IEndpointRouteBuilder routes)
    {



        var group = routes.MapGroup("/occasions").WithParameterValidation();
        
        
        
        
        group.MapGet("/", async (IRepository repository)
            => (await repository.GetAllOccasionsAsync())
            .Select(post => post.AsDto()));

  
        group.MapPost("/addOccasion",async (
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
                
                Occasion occasion= new (){
                    Name= name

                };
                await iRepository.CreateOccasionAsync(occasion);

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
                var occasion =await repository.GetOccasionAsync(id);
                if (occasion is null) return Results.NoContent();
                await repository.DeleteOccasionAsync(id);
                return Results.Ok();
            }

            return Results.Conflict( new { error="شما دسترسی ندارید"});



        }).RequireAuthorization();
        


        return group;
    }

}