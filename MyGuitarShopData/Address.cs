using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyGuitarShopData;

public partial class Address
{
    [Key]
    [Column("AddressID")]
    public int AddressId { get; set; }

    [Column("CustomerID")]
    public int? CustomerId { get; set; }

    [StringLength(60)]
    [Unicode(false)]
    public string Line1 { get; set; } = null!;

    [StringLength(60)]
    [Unicode(false)]
    public string? Line2 { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string City { get; set; } = null!;

    [StringLength(2)]
    [Unicode(false)]
    public string State { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string ZipCode { get; set; } = null!;

    [StringLength(12)]
    [Unicode(false)]
    public string Phone { get; set; } = null!;

    public int Disabled { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Addresses")]
    public virtual Customer? Customer { get; set; }
}
