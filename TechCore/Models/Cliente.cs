using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechCore.Models;

[Table("clientes")]
public partial class Cliente
{
    [Key]
    [MaxLength(50)]
    public string Codclien { get; set; } = string.Empty;
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

    public bool Estado { get; set; } = true;

    public DateTime? CreatedDate { get; set; }

    public List<Venta> Venta { get; set; } = [];
}
