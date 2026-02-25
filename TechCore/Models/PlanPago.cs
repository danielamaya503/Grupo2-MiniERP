using System;
using System.Collections.Generic;

namespace TechCore.Models;

public partial class PlanPago
{
    public int Id { get; set; }

    public string Norden { get; set; } = null!;

    public int NumeroCuota { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    public decimal MontoCuota { get; set; }

    public bool? Pagada { get; set; }

    public virtual Venta NordenNavigation { get; set; } = null!;
}
