using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyGuitarShopData;

public partial class Promotion
{
    [Key]
    [Column("PromoID")]
    public int PromoId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PromoName { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime EndDate { get; set; }
}
