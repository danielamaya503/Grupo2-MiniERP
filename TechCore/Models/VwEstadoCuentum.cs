using System;
using System.Collections.Generic;

namespace TechCore.Models;

public partial class VwEstadoCuentum
{
    public string Norden { get; set; } = null!;

    public string Cliente { get; set; } = null!;

    public decimal Total { get; set; }

    public decimal Saldo { get; set; }

    public int? Meses { get; set; }

    public decimal? TotalAbonado { get; set; }
}
