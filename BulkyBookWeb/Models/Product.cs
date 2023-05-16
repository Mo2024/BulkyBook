using System.ComponentModel.DataAnnotations;

public class Product
{
    [Key]
    public int ProductID { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Type is required")]
    [StringLength(50, ErrorMessage = "Type cannot exceed 50 characters")]
    public string Type { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be a non-negative number")]
    public int StockQuantity { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative number")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "Image data is required")]
    public byte[] ImageData { get; set; }

}
