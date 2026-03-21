using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechCore.Models;

[Table("abonosVentas")]
public partial class AbonosVenta
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int IdAbonosVenta { get; set; }

    [MaxLength(50)]
    public string Norden { get; set; } = string.Empty;

    public DateTime? Fecha { get; set; } = DateTime.UtcNow;
    [Precision(18,2)]
    public decimal Monto { get; set; }
    public int NumeroCuota { get; set; }

    public Venta? NordenNavigation { get; set; }
}
