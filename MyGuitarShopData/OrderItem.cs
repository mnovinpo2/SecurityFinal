using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyGuitarShopData;

public partial class OrderItem
{
    [Key]
    [Column("ItemID")]
    public int ItemId { get; set; }

    [Column("OrderID")]
    public int? OrderId { get; set; }

    [Column("ProductID")]
    public int? ProductId { get; set; }

    [Column(TypeName = "money")]
    public decimal ItemPrice { get; set; }

    [Column(TypeName = "money")]
    public decimal DiscountAmount { get; set; }

    public int Quantity { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderItems")]
    public virtual Order? Order { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("OrderItems")]
    public virtual Product? Product { get; set; }
}
