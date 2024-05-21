using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyGuitarShopData;

[Index("LastName", "FirstName", Name = "IXFirstNameLastName")]
[Index("LastName", "FirstName", Name = "IX_Customers_LastName_FirstName")]
[Index("EmailAddress", Name = "UQ__Customer__49A147404D8F6404", IsUnique = true)]
public partial class Customer
{
    [Key]
    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string EmailAddress { get; set; } = null!;

    [NotMapped]
    [DataType(DataType.Password)]
    public string Password { get; set; } // Non-mapped password used for form submissions.

    [StringLength(255)]
    [Unicode(false)]
    public string PasswordHash { get; set; } = null!; // Hashed password stored in the database.

    [Column(TypeName = "varbinary(16)")]
    public byte[] Salt { get; set; } // Salt stored in the database.

    [StringLength(60)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [StringLength(60)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [Column("ShippingAddressID")]
    public int? ShippingAddressId { get; set; }

    [Column("BillingAddressID")]
    public int? BillingAddressId { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}