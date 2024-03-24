using System.ComponentModel.DataAnnotations;

namespace NewBannerchi.Entities;

public class User
{
    public int Id { get; set; }
    [MaxLength(20)]
    [Required]
    public required string Name { get; set; }
    
    [MaxLength(15)]
    [Required]
    public required string PhoneNumber { get; set; }
   
    [MaxLength(20)]
    [Required]
    public required string TypeOfPage { get; set; }
  
    [MaxLength(20)]
    public required string JobTitle { get; set; }
    
   
}