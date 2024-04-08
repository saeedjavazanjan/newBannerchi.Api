using System.ComponentModel.DataAnnotations;

namespace NewBannerchi.Entities;

public class DownLoadDetail
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(30)]
    public required string PackName { get; set; }
    
    public required int PackId { get; set; }
    
    public required int UserId { get; set; }
    
    public required string UserPurchaseToken { get; set; }
    
    public DateTime Time { get; set; }
}