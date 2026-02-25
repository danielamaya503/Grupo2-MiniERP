using System;
using System.Collections.Generic;

namespace TechCore.Models;

public partial class VwCuotasPorVencer
{
    public string Norden { get; set; } = null!;

    public string Cliente { get; set; } = null!;

    public int NumeroCuota { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    public decimal MontoCuota { get; set; }
}
