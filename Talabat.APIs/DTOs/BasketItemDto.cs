using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs;
public class BasketItemDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string productName { get; set; }
    [Required]
    public string PictureUrl { get; set; }
    [Required]
    public string Brand { get; set; }
    [Required]
    public string Type { get; set; }
    [Required]
    [Range(0.1, double.MaxValue , ErrorMessage = "Price Can't Be Zero")]
    public decimal Price { get; set; }
    [Required]
    [Range(1, int.MaxValue , ErrorMessage = " Quantity Must be One Item At Least")]
    public int Quantity { get; set; }
}