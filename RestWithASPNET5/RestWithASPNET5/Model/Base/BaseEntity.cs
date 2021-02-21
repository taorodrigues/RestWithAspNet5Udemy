using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestWithASPNET5.Model.Base
{
  public class BaseEntity
  {
    [Column("Id")]
    public Guid Id { get; set; }
  }
}
