﻿using System.Security.Claims;
using NewBannerchi.Entities;
using NewBannerchi.Repository;

namespace NewBannerchi.Endpoints;

public static class PackagesEndpoints
{
    private const  string SearchPost="Search";
    private const  string Category="Category";
    public static RouteGroupBuilder MapPackagesEndpoints(this IEndpointRouteBuilder routes)
    {



        var group = routes.MapGroup("/packages").WithParameterValidation();

        /*group.MapGet("/", async (IRepository repository,int pageNumber,int pageSize) 
            => (await repository.GetAllAsync())
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(package=>package.AsDto()));*/
        
        
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
                string occasion,
                int pageNumber,
                int pageSize)
            =>
        {
            IEnumerable<Package> categoryPackages = await repository.GetWithCategoryAsync(occasion);
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
            IEnumerable<Package
            > searchedPosts = await repository.SearchAsync(query);
            return searchedPosts is not null ? Results.Ok(searchedPosts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).Select(post=>post.AsDto())):Results.NotFound();
        }).WithName(SearchPost);
        
           /*
           group.MapPost("/uploadPackage",async (
            IRepository iRepository,
             AddPackageDto addPackageDto,
            ClaimsPrincipal? user
            )=>{
          var userId= user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          User? currentUser = await iRepository.GetUserAsync(Int32.Parse(userId));
          if(currentUser==null){
              return Results.NotFound(new{error="کاربر یافت نشد."}); 
          }
          var userName = currentUser.UserName;
          if (userId.Equals(currentUser.UserId.ToString()))
          {
             
              
              
              Post post=new (){
                  Title= postDto.Title,
                  Category = postDto.Category,
                  PostType= postDto.PostType,
                  Author= userName,
                  AuthorId= int.Parse(userId),
                  AuthorAvatar = currentUser.Avatar,
                  FeaturedImages= [],
                  Like = 0,
                  Video = "",
                  Description = postDto.Description,
                  DataAdded = DateTime.Now,
                  LongDataAdded = postDto.LongDataAdded,
                  HaveProduct = postDto.HaveProduct

              };
              await iRepository.CreateAsync(post);

              Post? existedPost = await iRepository.GetAsync(post.Id);
              
              if (postDto.Video != null && postDto.PostType=="video")
              {
                  var fileResult =
                      iFileService.SavePostVideo(postDto.Video,post.Id.ToString());
                  if (fileResult.Item1 == 1)
                  {
                      existedPost!.Video = fileResult.Item2; 
                  }
                  else
                  {
                      return Results.Conflict(new { error = fileResult.Item2});
                  }

              }

              if (postDto.FeaturedImages != null && 
                  postDto.PostType == "image" &&
                  postDto.FeaturedImages.Count>0

                  )
              {
                  var fileResult = 
                      iFileService.SavePostImages(postDto.FeaturedImages, post.Id.ToString());
                  if (fileResult.Item1==1)
                  {
                      existedPost!.FeaturedImages = fileResult.Item2;
                  }
              }
              

              await iRepository.UpdateAsync(existedPost!);

              return Results.CreatedAtRoute(GetPostEndPointName,new{post.Id},post);
              
              
          }

          return Results.Conflict( new { error="شما دسترسی به این کاربر ندارید"});



        }).RequireAuthorization().DisableAntiforgery();

        group.MapPut("update/{id}",async (
                IRepository repository,
                int id, 
                UpdatePostDto updatePostDto,
                ClaimsPrincipal? user)=>
            {
                var userId= user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                User? currentUser = await repository.GetUserAsync(Int32.Parse(userId));

                Post? existedPost = await repository.GetAsync(id);

                if(existedPost==null){
                    return Results.NotFound(new{error="پست مورد نظر یافت نشد"}); 
                }

                if (existedPost.AuthorId != Int32.Parse(userId))
                {
                    return Results.Unauthorized();
                }

                existedPost.Title=updatePostDto.Title;
                existedPost.Category = updatePostDto.Category;
                existedPost.AuthorAvatar = currentUser!.Avatar;
                existedPost.Description=updatePostDto.Description;
                existedPost.DataAdded=DateTime.Now;
                existedPost.LongDataAdded=updatePostDto.LongDataAdded;
                existedPost.HaveProduct=updatePostDto.HaveProduct;
                
                await  repository.UpdateAsync(existedPost);
                return Results.Ok("با موفقیت به روز رسانی شد");   

            }
        ).RequireAuthorization();

        group.MapDelete("/{id}",async (
            IFileService fileService,
            IRepository repository,
            int id,
            ClaimsPrincipal? user
            )=>
        {
            var userId= user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Post? post =await repository.GetAsync(id);

            if (post.AuthorId == Int32.Parse(userId))
            {
                if(post is not null){
                    if (post.PostType == "image")
                    {
                        fileService.DeletePostImage(id.ToString());
                    }
                    else if(post.PostType == "video")
                    {
                        fileService.DeletePostVideo(id.ToString());
                    }
                    await repository.DeleteAsync(id);

                    
                    
                    return Results.Ok();
                }
                return Results.NoContent();   

            }

            return Results.Unauthorized();


        }).RequireAuthorization();
        */

        
        
        return group;
    }
    
    
}