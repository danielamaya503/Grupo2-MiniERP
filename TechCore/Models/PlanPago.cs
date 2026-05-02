using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechCore.Models;

[Table("planPagos")]
public partial class PlanPago
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int IdPlanPago { get; set; }
    [MaxLength(50)]
    public string Norden { get; set; } = string.Empty;
    public int NumeroCuota { get; set; }
    public DateOnly FechaVencimiento { get; set; }
    [Precision(18,2)]
    public decimal MontoCuota { get; set; }

    public bool? Pagada { get; set; }

    public virtual Venta NordenNavigation { get; set; } = null!;
}
