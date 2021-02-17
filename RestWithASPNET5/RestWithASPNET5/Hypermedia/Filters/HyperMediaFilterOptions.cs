using System.Collections.Generic;
using RestWithASPNET5.Hypermedia.Abstract;

namespace RestWithASPNET5.Hypermedia.Filters
{
    public class HyperMediaFilterOptions
    {
        public List<IResponseEnricher> ContentResponseEnricherList { get; set; } = new List<IResponseEnricher>();
    }
}
