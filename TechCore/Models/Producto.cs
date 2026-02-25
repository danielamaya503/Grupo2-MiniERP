using System;
using System.Collections.Generic;

namespace TechCore.Models;

public partial class Producto
{
    public string Codprod { get; set; } = null!;

    public int? CodCategoria { get; set; }

    public string? Descripcion { get; set; }

    public decimal PrecioCompra { get; set; }

    public decimal PrecioVenta { get; set; }

    public int? Stock { get; set; }

    public int? StockMinimo { get; set; }

    public bool? Estado { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Categorium? CodCategoriaNavigation { get; set; }

    public virtual ICollection<ComprasDetalle> ComprasDetalles { get; set; } = new List<ComprasDetalle>();

    public virtual ICollection<VentasDetalle> VentasDetalles { get; set; } = new List<VentasDetalle>();
}
