using System.ComponentModel.DataAnnotations.Schema;
using RestWithASPNET5.Model.Base;

namespace RestWithASPNET5.Model
{
  [Table("Person")]
  public class Person : BaseEntity
  {
    [Column("FirstName")]
    public string FirstName { get; set; }

    [Column("LastName")]
    public string LastName { get; set; }

    [Column("Address")]
    public string Address { get; set; }

    [Column("Gender")]
    public string Gender { get; set; }

    [Column("Enabled")]
    public bool Enabled { get; set; }
  }
}
