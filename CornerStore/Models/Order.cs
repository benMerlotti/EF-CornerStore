
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CornerStore.Models;

public class Order
{
    public int Id { get; set; }
    [Required]
    public int CashierId { get; set; }
    public DateTime? PaidOnDate { get; set; }

    // NAV PROPERTIES
    public Cashier Cashier { get; set; }
    public List<Product> Products { get; set; }
    // CALCULATED
    public decimal Total
    {
        get
        {
            return Products?.Sum(p => p.Price) ?? 0;
        }
    }

}