// Models/Roll.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.API.Models
{
    public class Roll
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public double Length { get; set; }
    
    [Required]
    public double Weight { get; set; }
    
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;  // ← УБРАТЬ .Date
    
    public DateTime? RemovedDate { get; set; }
    
    public bool IsRemoved => RemovedDate.HasValue;
}
}