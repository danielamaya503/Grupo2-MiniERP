using System;
using System.Collections.Generic;

namespace TechCore.Models;

public partial class VentasDetalle
{
    public int Id { get; set; }

    public string Norden { get; set; } = null!;

    public string Codprod { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal Pventa { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Producto CodprodNavigation { get; set; } = null!;

    public virtual Venta NordenNavigation { get; set; } = null!;
}
