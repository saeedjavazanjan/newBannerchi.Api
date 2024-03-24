using System.ComponentModel.DataAnnotations;

namespace NewBannerchi.Entities;

public class Category
{
    public int Id { get; set; }
    [MaxLength(20)]
    [Required]
    public required string Name { get; set; }
}