using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace TechCore.Models;

[Keyless]
public partial class VwEstadoCuentum
{
    public string Norden { get; set; } = string.Empty;

    public string Cliente { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public decimal Saldo { get; set; }

    public int? Meses { get; set; }

    public decimal? TotalAbonado { get; set; }
}
