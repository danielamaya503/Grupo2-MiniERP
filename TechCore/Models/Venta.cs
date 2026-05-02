using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechCore.Models;

[Table("ventas")]
public partial class Venta
{
    [Key]
    [MaxLength(50)]
    public string Norden { get; set; } = string.Empty;

    public int OrdenN { get; set; }
    [MaxLength(50)]
    public string Codclien { get; set; } = string.Empty;

    public int Codvend { get; set; }

    public DateTime? Fecha { get; set; } = DateTime.UtcNow;
    [Precision(18,2)]
    public decimal Subtotal { get; set; }
    [Precision(18, 2)]
    public decimal Iva { get; set; }
    [Precision(18, 2)]
    public decimal Total { get; set; }
    [MaxLength(20)]
    public string? TipoPago { get; set; }

    public int? Meses { get; set; }
    [Precision(5, 2)]
    public decimal? TasaInteres { get; set; } = 0;
    [Precision(18, 2)]
    public decimal Saldo { get; set; }

    public bool? Nula { get; set; } = false;

    public bool? Estado { get; set; } = true;

    public List<AbonosVenta> AbonosVenta { get; set; } = [];

    public virtual Cliente CodclienNavigation { get; set; } = null!;

    public virtual User CodvendNavigation { get; set; } = null!;

    public List<PlanPago> PlanPagos { get; set; } = [];

    public List<VentasDetalle> VentasDetalles { get; set; } = [];
}
