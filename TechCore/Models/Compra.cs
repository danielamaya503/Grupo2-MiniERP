using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechCore.Models;

[Table("Compras")]
public partial class Compra
{
    [Key]
    [MaxLength(50)]
    public string Norden { get; set; } = string.Empty;
    public int OrdenN { get; set; }
    [MaxLength(50)]
    public string Codprov { get; set; } = string.Empty!;
    public int Codusu { get; set; }

    public DateTime? Fecha { get; set; } = DateTime.UtcNow;

    public decimal Subtotal { get; set; }
    [Precision(18,2)]
    public decimal Iva { get; set; }
    [Precision(18, 2)]
    public decimal Total { get; set; }
    [Precision(18, 2)]
    public int? Estado { get; set; } = 1;

    public  Proveedore? CodprovNavigation { get; set; } 

    public  User? CodusuNavigation { get; set; }

    public virtual List<ComprasDetalle> ComprasDetalles { get; set; } = [];

}
