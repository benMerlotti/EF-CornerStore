
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CornerStore.Models.DTO;

public class OrderDTO
{
    public int Id { get; set; }
    public int CashierId { get; set; }
    [NotMapped]
    public DateTime? PaidOnDate { get; set; }
    public decimal Total { get; set; }

    // NAV PROPERTIES
    public Cashier Cashier { get; set; }
    public List<ProductDTO> Products { get; set; }

}

public class OrderDetailsDTO
{
    public int Id { get; set; }
    public int CashierId { get; set; }
    public string CashierName { get; set; }
    public DateTime? PaidOnDate { get; set; }
    public decimal Total { get; set; }

    // NAV PROPERTIES
    public Cashier Cashier { get; set; }
    public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();

}