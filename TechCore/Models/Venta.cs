using System;
using System.Collections.Generic;

namespace TechCore.Models;

public partial class Venta
{
    public string Norden { get; set; } = null!;

    public int OrdenN { get; set; }

    public string Codclien { get; set; } = null!;

    public int Codvend { get; set; }

    public DateTime? Fecha { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Iva { get; set; }

    public decimal Total { get; set; }

    public string? TipoPago { get; set; }

    public int? Meses { get; set; }

    public decimal? TasaInteres { get; set; }

    public decimal Saldo { get; set; }

    public bool? Nula { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<AbonosVenta> AbonosVenta { get; set; } = new List<AbonosVenta>();

    public virtual Cliente CodclienNavigation { get; set; } = null!;

    public virtual User CodvendNavigation { get; set; } = null!;

    public virtual ICollection<PlanPago> PlanPagos { get; set; } = new List<PlanPago>();

    public virtual ICollection<VentasDetalle> VentasDetalles { get; set; } = new List<VentasDetalle>();
}
