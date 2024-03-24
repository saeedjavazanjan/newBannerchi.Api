using System.ComponentModel.DataAnnotations;

namespace NewBannerchi.Entities;

public class Occasion
{
    public int Id { get; set; }
    [MaxLength(20)]
    [Required]
    public required string Name { get; set; }
}