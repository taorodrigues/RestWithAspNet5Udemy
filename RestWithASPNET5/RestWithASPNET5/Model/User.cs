using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestWithASPNET5.Model
{
  [Table("Users")]
  public class User
  {
    [Key]
    [Column("Id")]
    public long Id { get; set; }

    [Column("UserName")]
    public string UserName { get; set; }

    [Column("FullName")]
    public string FullName { get; set; }

    [Column("Password")]
    public string Password { get; set; }

    [Column("RefreshToken")]
    public string RefreshToken { get; set; }

    [Column("RefreshTokenExpiryTime")]
    public DateTime RefreshTokenExpiryTime { get; set; }
  }
}
