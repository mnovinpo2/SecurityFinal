using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyGuitarShopData;

[Keyless]
public partial class CustomerAddress
{
    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string EmailAddress { get; set; } = null!;

    [StringLength(60)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [StringLength(60)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [StringLength(60)]
    [Unicode(false)]
    public string BillLine1 { get; set; } = null!;

    [StringLength(60)]
    [Unicode(false)]
    public string? BillLine2 { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string BillCity { get; set; } = null!;

    [StringLength(2)]
    [Unicode(false)]
    public string BillState { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string BillZip { get; set; } = null!;

    [StringLength(60)]
    [Unicode(false)]
    public string ShipLine1 { get; set; } = null!;

    [StringLength(60)]
    [Unicode(false)]
    public string? ShipLine2 { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string ShipCity { get; set; } = null!;

    [StringLength(2)]
    [Unicode(false)]
    public string ShipState { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string ShipZip { get; set; } = null!;
}
