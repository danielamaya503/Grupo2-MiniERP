using System;
using System.Collections.Generic;

namespace TechCore.Models;

public partial class AbonosVenta
{
    public int Id { get; set; }

    public string Norden { get; set; } = null!;

    public DateTime? Fecha { get; set; }

    public decimal Monto { get; set; }

    public int NumeroCuota { get; set; }

    public virtual Venta NordenNavigation { get; set; } = null!;
}
