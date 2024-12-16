using System.ComponentModel.DataAnnotations;

namespace CornerStore.Models.DTO;

public class ProductDTO
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public string Brand { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }

    // NAV PROPERTIES
    public CategoryDTO Category { get; set; }
    public List<OrderDTO> Orders { get; set; }
}

public class ProductQueryDTO
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public string Brand { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }

    // NAV PROPERTIES
    public CategoryDTO Category { get; set; }
    public List<OrderDTO> Orders { get; set; }
}