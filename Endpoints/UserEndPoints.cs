using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NewBannerchi.Authentication;
using NewBannerchi.Entities;
using NewBannerchi.KavehNegar;
using NewBannerchi.Repository;

namespace NewBannerchi.Endpoints;

public static class UserEndPoints
{
     private const  string CreatedUser="User";
    private const string GetUser = "getUser";

    
    public static RouteGroupBuilder MapUsersEndPoints(this IEndpointRouteBuilder routes){
        var group=routes.MapGroup("/users").WithParameterValidation();
        string generatedPassword  = null;
        
        group.MapGet("/currentUser",async (
                IRepository repository,
                ClaimsPrincipal user
                )=> 
            {
                var userId= user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    User? currentUser = await repository.GetUserAsync(Int32.Parse(userId));
                    return currentUser is not null ? 
                        Results.Ok(currentUser.AsDto()) : Results.NotFound(new{error="کاربر یافت نشد."});
                }
                return Results.Conflict(new{error="کاربر یافت نشد."});
 
            }
        ).WithName(GetUser).RequireAuthorization();
        
        
        group.MapPost("/signUp",async (
            IRepository iRepository , 
            RegisterUserDto registerUserDto)=>
        {
            generatedPassword = GenerateRandomNo();

           
            
            User? existedUser = await iRepository.GetRegesteredPhoneNumberAsync(registerUserDto.PhoneNumber);
            if (existedUser is not null )
            {
                return Results.Conflict(new{error="با این شماره قبلا ثبت نام صورت گرفته است."});
            }
            
            else
            {
                UserOtp userOtp = new()
                {
                    UserName = registerUserDto.Name,
                    OtpPassword = generatedPassword,
                    PhoneNumber = registerUserDto.PhoneNumber,
                    Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                };
                UserOtp? existedUserOtp = await iRepository.GetUserOtpAsync(registerUserDto.PhoneNumber);
                if (existedUserOtp is not null)
                {
                    existedUserOtp.OtpPassword = generatedPassword;
                    existedUserOtp.Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    await iRepository.UpdateUserOtpAsync(existedUserOtp);
                }
                else if(existedUserOtp is null)
                {
                    await iRepository.AddUserOtp(userOtp);

                }

                String result= await SendSms.SendSMS.SendSMSToUser(generatedPassword,registerUserDto.PhoneNumber);

              //  if (result.Equals("ارسال موفق"))
            //    {
                    return Results.Ok(result);
           //    }
               // else
            //    {
               //    return Results.Json(result);
           //    }
                
            }
        }) .RequireRateLimiting("fixed");
        
        group.MapPost("/signIn", async (
            IRepository iRepository,
            SignInUserDto signInUserDto) =>
              {
                  User? regesterdUser = await iRepository.
                      GetRegesteredPhoneNumberAsync(signInUserDto.PhoneNumber);

                  UserOtp? userOtp = await iRepository.GetUserOtpAsync(signInUserDto.PhoneNumber);

                  if (regesterdUser is not null && userOtp is not null)
                  {
                      
                     // generatedPassword = "1234";
                       generatedPassword =  GenerateRandomNo();

                       /*UserOtp updatedUserOtp = new()
                       {
                           OtpPassword = generatedPassword,
                           PhoneNumber = userOtp.PhoneNumber,
                           UserName = userOtp.UserName,
                           Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                       };*/
                       userOtp.OtpPassword = generatedPassword;
                       userOtp.Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                       await iRepository.UpdateUserOtpAsync(userOtp);
                      String result= await SendSms.SendSMS.
                          SendSMSToUser(generatedPassword,
                              signInUserDto.PhoneNumber);
                      //  if (result.Equals("ارسال موفق"))
                      // {
                      return Results.Ok(result);
                      //  }
                  
                      //   else
                      //   {
                      return Results.Json(result);
                      //   }
                  }
                  return Results.NotFound(new { error="شما ثبت نام نکرده اید."});
                  
              }) .RequireRateLimiting("fixed");
      
        group.MapPost("/signInPasswordCheck", async (
            IJwtProvider iJwtProvider,
            IRepository iRepository,
            SignInUserDto signInUserDto
            ) =>
        {
           UserOtp? userOtp= await iRepository.GetUserOtpAsync(signInUserDto.PhoneNumber);
           long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

           long timeDistance = (currentTime/1000) - (userOtp!.Time/1000);

           if (timeDistance > 120)
           {
               return Results.Conflict(new { error="پسورد منقضی شده"});

           }

            if (signInUserDto.Password == userOtp!.OtpPassword)
            {
                
                User? regesterdeUser = await iRepository.GetRegesteredPhoneNumberAsync(signInUserDto.PhoneNumber);

                if (regesterdeUser is not null)
                {
                    var token=  await iJwtProvider.Generate(regesterdeUser);
                    // var userData=Results.CreatedAtRoute(GetUser,new {user.UserId},user);
                    return Results.Ok(token);
                    
                }

                return Results.NotFound(new { error="شما ثبت نام نکرده اید."});

            }
            else
            {
                return Results.Conflict(new { error="رمز اشتباه است"});
            }
        });
        
        
        group.MapPost("/signUpPasswordCheck",async (
            IJwtProvider iJwtProvider,
            IRepository iRepository,
            AddUserDto addUserDto
            )=>{
            UserOtp? userOtp= await iRepository.GetUserOtpAsync(addUserDto.PhoneNumber);
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if (userOtp ==null)
            {
                return Results.NotFound(new { error="یوزر یافت نشد"});

            }
            
            long timeDistance = currentTime/1000 - ((userOtp!.Time)/1000);

            if (timeDistance > 120)
            {
                return Results.Conflict(new { error="پسورد منقضی شده"});

            }

            if (addUserDto.Password == userOtp!.OtpPassword )
            {

                User user = new()
                {
                    Name = addUserDto.UserName,
                    PhoneNumber = addUserDto.PhoneNumber,
                    TypeOfPage = addUserDto.TypeOfPage,
                    JobTitle = addUserDto.JobTitle
                   
                };

                User? regesterdeUser = await iRepository.GetRegesteredPhoneNumberAsync(addUserDto.PhoneNumber);

                if (regesterdeUser is not null)
                {
                   
                    return Results.Conflict(new{error="با این شماره قبلا ثبت نام صورت گرفته است."});

                }
                else
                {
                    await iRepository.AddUser(user);
                 var token=  await iJwtProvider.Generate(user);
                    // var userData=Results.CreatedAtRoute(GetUser,new {user.UserId},user);
                  return Results.Ok(token);
                }
            }
            else
            {
                return Results.Conflict(new { error="رمز اشتباه است"});

            }
        });
        
        group.MapDelete("/{id}",async (IRepository repository,int id)=>
        {
            User? user =await repository.GetUserAsync(id);

            if(user is not null){
                await repository.DeleteUser(id); 
            }
            return Results.NoContent();   
        });
        group.MapDelete("userOtp/{id}",async (IRepository repository,int id)=>
        {
                await repository.DeleteUserOtpAsync(id); 
            
            return Results.NoContent();   
        });

        /*group.MapPut("/updateUser/{aus}", async (
            IRepository iRepository,
            IFileService iFileService,
            ClaimsPrincipal? user,
            string AUS,
          [FromForm] UserUpdateDto userUpdateDto
            ) => {
            var userId= user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId!= null)
            {
                User? currentUser = await iRepository.GetUserAsync(Int32.Parse(userId));
                if(currentUser==null){
                    return Results.NotFound(new{error="کاربر یافت نشد."}); 
                }

                if (AUS == "yes" && userUpdateDto.AvatarFile!=null)
                {
                    
                    iFileService.DeleteAvatar(userId);
                    var fileResult = iFileService.SaveAvatar(userUpdateDto.AvatarFile ,userId);
                    if (fileResult.Item1 == 1)
                    {
                        currentUser.Avatar = fileResult.Item2; 
                    }
                   
                }

                currentUser.UserName = userUpdateDto.UserName;
                currentUser.Bio = userUpdateDto.Bio;
                await iRepository.UpdateUserAsync(currentUser);
                return Results.Ok();


            }

            return Results.Conflict(new{error="کاربر یافت نشد."});


        }).RequireAuthorization().DisableAntiforgery();*/
        
     
        return group;
    }
    
   
    
    private static string GenerateRandomNo()
    {
        int _min = 1000;
        int _max = 9999;
        Random _rdm = new Random();
        return _rdm.Next(_min, _max).ToString();
    }
}