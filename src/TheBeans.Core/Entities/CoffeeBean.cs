using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using TheBeans.Core.Common;
using TheBeans.Core.Entities;

public class CoffeeBean : BaseEntity
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string Origin { get; set; } // Matches `Country`
    [Required]
    public string RoastLevel { get; set; } // Matches `colour`
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; } // Converted from `Cost`
    [Required]
    public string Currency {get;set;}
    [Required]
    public string ImageUrl { get; set; }

    // Navigation Property (not stored in DB)
    public virtual BeanOfTheDay BeanOfTheDay { get; set; }
}