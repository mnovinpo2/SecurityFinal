using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyGuitarShopData;

public partial class Administrator
{
    [Key]
    [Column("AdminID")]
    public int AdminId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string EmailAddress { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;
}
