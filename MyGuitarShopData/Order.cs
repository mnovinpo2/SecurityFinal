using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyGuitarShopData;

public partial class Order
{
    [Key]
    [Column("OrderID")]
    public int OrderId { get; set; }

    [Column("CustomerID")]
    public int? CustomerId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    [Column(TypeName = "money")]
    public decimal ShipAmount { get; set; }

    [Column(TypeName = "money")]
    public decimal TaxAmount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ShipDate { get; set; }

    [Column("ShipAddressID")]
    public int ShipAddressId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string CardType { get; set; } = null!;

    [StringLength(16)]
    [Unicode(false)]
    public string CardNumber { get; set; } = null!;


    [StringLength(7)]
    [Unicode(false)]
    [DefaultValue("01/99")] // Set a default value for CardExpires
    public string CardExpires { get; set; } = "01/99";


    [Column("BillingAddressID")]
    public int BillingAddressId { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Orders")]
    public virtual Customer? Customer { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
