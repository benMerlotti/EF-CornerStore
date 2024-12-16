using System.ComponentModel.DataAnnotations;

namespace CornerStore.Models.DTO;

public class CategoryDTO
{
    public int Id { get; set; }
    public string CategoryName { get; set; }

    // NAV PROPS
    public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
}