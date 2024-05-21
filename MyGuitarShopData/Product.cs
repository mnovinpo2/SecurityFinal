using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyGuitarShopData;

[Index("ProductCode", Name = "UQ__Products__2F4E024FCC63028D", IsUnique = true)]
public partial class Product
{
    [Key]
    [Column("ProductID")]
    public int ProductId { get; set; }

    [Column("CategoryID")]
    public int? CategoryId { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string ProductCode { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string ProductName { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Description { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal ListPrice { get; set; }

    [Column(TypeName = "money")]
    public decimal DiscountPercent { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateAdded { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category? Category { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
