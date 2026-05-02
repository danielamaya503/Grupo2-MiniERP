using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechCore.Models;

[Table("proveedores")]
public partial class Proveedore
{
    [Key]
    [MaxLength(50)]
    public string Codprovee { get; set; } = string.Empty;
    [MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;
    [Phone]
    [MaxLength(15)]
    public string? Telefono { get; set; }
    [EmailAddress]
    [MaxLength(200)]
    public string? Email { get; set; }
    [MaxLength(300)]
    public string? Direccion { get; set; }

    public int Estado { get; set; } = 1;

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    public virtual List<Compra> Compras { get; set; } = [];
}
