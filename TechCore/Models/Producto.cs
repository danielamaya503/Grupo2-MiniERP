using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace TechCore.Models;

[Table("productos")]
public partial class Producto
{
    [Key]
    [MaxLength(50)]
    public string Codprod { get; set; } = string.Empty;
    public int? CodCategoria { get; set; }
    [MaxLength(500)]
    public string? Descripcion { get; set; }
    [Precision(18,2)]
    public decimal PrecioCompra { get; set; }
    [Precision(18, 2)]
    public decimal PrecioVenta { get; set; }

    public int? Stock { get; set; }

    public int? StockMinimo { get; set; }

    public bool Estado { get; set; } = true;

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    public virtual Categorium? CodCategoriaNavigation { get; set; }

    public List<ComprasDetalle> ComprasDetalles { get; set; } = [];

    public List<VentasDetalle> VentasDetalles { get; set; } = [];
}
