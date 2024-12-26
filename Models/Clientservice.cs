using System;
using System.Collections.Generic;

namespace demo3.Models;

public partial class Clientservice
{
    public int Id { get; set; }

    public int Clientid { get; set; }

    public int Serviceid { get; set; }

    public DateTime Starttime { get; set; }

    public string? Comment { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<Documentbyservice> Documentbyservices { get; set; } = new List<Documentbyservice>();

    public virtual ICollection<Productsale> Productsales { get; set; } = new List<Productsale>();

    public virtual Service Service { get; set; } = null!;
}
