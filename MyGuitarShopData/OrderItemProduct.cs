using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyGuitarShopData;

[Keyless]
public partial class OrderItemProduct
{
    [Column("OrderID")]
    public int OrderId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    [Column(TypeName = "money")]
    public decimal TaxAmount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ShipDate { get; set; }

    [Column(TypeName = "money")]
    public decimal ItemPrice { get; set; }

    [Column(TypeName = "money")]
    public decimal DiscountAmount { get; set; }

    [Column(TypeName = "money")]
    public decimal? FinalPrice { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "money")]
    public decimal? ItemTotal { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string ProductName { get; set; } = null!;
}
