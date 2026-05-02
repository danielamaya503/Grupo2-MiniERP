using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechCore.Models;

[Table("rol")]
public partial class Rol
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int IdRol { get; set; }
    [StringLength(100)]
    public string NombreRol { get; set; } = null!;

    public bool? Habilitado { get; set; } = true;

    public List<User> Users { get; set; } = [];
}
