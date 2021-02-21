using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestWithASPNET5.Model.Base;

namespace RestWithASPNET5.Model
{
  [Table("Book")]
  public class Book : BaseEntity
  {
    [Column("Title")]
    public string Title { get; set; }

    [Column("Author")]
    public string Author { get; set; }

    [Column("Price", TypeName = "decimal(18,4)")]

    public decimal Price { get; set; }

    [Column("LaunchDate")]
    public DateTime LaunchDate { get; set; }
  }
}
