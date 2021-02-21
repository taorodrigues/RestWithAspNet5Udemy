using System;
using System.Collections.Generic;
using RestWithASPNET5.Hypermedia;
using RestWithASPNET5.Hypermedia.Abstract;

namespace RestWithASPNET5.Data.VO
{
  public class BookVO : ISupportsHyperMedia
  {
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Author { get; set; }

    public decimal Price { get; set; }

    public DateTime LaunchDate { get; set; }
    public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();

  }
}