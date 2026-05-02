using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace TechCore.Models;

[Keyless]
public partial class VwCuotasPorVencer
{
    public string Norden { get; set; } = string.Empty;

    public string Cliente { get; set; } = string.Empty;

    public int NumeroCuota { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    public decimal MontoCuota { get; set; }
}
