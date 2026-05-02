using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechCore.Models;

public partial class VentasDetalle
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int IdVentasDetalle { get; set; }
    [MaxLength(50)]
    public string Norden { get; set; } = string.Empty;
    [MaxLength(50)]
    public string Codprod { get; set; } = string.Empty;

    public int Cantidad { get; set; }
    [Precision(18,2)]
    public decimal Pventa { get; set; }
    [Precision(18, 2)]
    public decimal Subtotal { get; set; }

    public virtual Producto CodprodNavigation { get; set; } = null!;

    public virtual Venta NordenNavigation { get; set; } = null!;
}
