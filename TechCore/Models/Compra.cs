using System;
using System.Collections.Generic;

namespace TechCore.Models;

public partial class Compra
{
    public string Norden { get; set; } = null!;

    public int OrdenN { get; set; }

    public string Codprov { get; set; } = null!;

    public int Codusu { get; set; }

    public DateTime? Fecha { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Iva { get; set; }

    public decimal Total { get; set; }

    public int? Estado { get; set; }

    public virtual Proveedore CodprovNavigation { get; set; } = null!;

    public virtual User CodusuNavigation { get; set; } = null!;

    public virtual ICollection<ComprasDetalle> ComprasDetalles { get; set; } = new List<ComprasDetalle>();
}
