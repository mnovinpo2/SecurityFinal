using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyGuitarShopData;

[Index("CategoryName", Name = "UQ__Categori__8517B2E0386F2A7A", IsUnique = true)]
public partial class Category
{
    [Key]
    [Column("CategoryID")]
    public int CategoryId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string CategoryName { get; set; } = null!;

    [InverseProperty("Category")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
