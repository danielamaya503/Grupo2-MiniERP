using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechCore.Models;

[Table("categoria")]
public partial class Categorium
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CodCategoria { get; set; }
    [MaxLength(20)]
    public string Codigo { get; set; } = string.Empty;
    [StringLength(150)]
    public string Nombre { get; set; } = string.Empty;
    [MaxLength(300)]
    public string? Descripcion { get; set; }

    public bool Estado { get; set; } = true;

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    public  List<Producto> Productos { get; set; } = [];
}
