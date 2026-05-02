using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechCore.Models;

[Table("comprasDetalle")]
public partial class ComprasDetalle
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int IdComprasDetalle { get; set; }
    [MaxLength(50)]
    public string Norden { get; set; } = string.Empty;
    [MaxLength(50)]
    public string Codprod { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    [Precision(18,2)]
    public decimal Precio { get; set; }
    [Precision(18, 2)]
    public decimal Subtotal { get; set; }

    public Producto? CodprodNavigation { get; set; }

    public Compra? NordenNavigation { get; set; }
}
