﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace NewBannerchi.Entities;

public class Package
{
    public int Id { get; set; }
    [MaxLength(20)]
    [Required]
    public required string Name { get; set; }
    
    [MaxLength(30)]
    [Required]
    public required string Designer { get; set; }
   
    [MaxLength(2)]
    [Required]
    public required string Type { get; set; }
  
    [MaxLength(6)]
    [Required]
    public required string DownloadCount { get; set; }
    
    [MaxLength(2000)]
    [Required]
    public required string Samples { get; set; }
  
    [Url]
    [Required]
    [MaxLength(100)]
    public required string HeaderUrl { get; set; }
  
    [Url]
    [MaxLength(100)]
    [Required]
    public required string PackageUrl { get; set; }
   
    [Required]
    public int Price { get; set; }
  
    [MaxLength(20)]
    [Required]
    public required List<string> Category { get; set; }
}