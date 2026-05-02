using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechCore.Models;

[Table("users")]
public partial class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int IdUser { get; set; }
    [MaxLength(10)]
    public string Code { get; set; } = string.Empty;
    [MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;
    [MaxLength(200)]
    public string Username { get; set; } = string.Empty;
    [MinLength(6, ErrorMessage = "La contraseña debe de contener minimo 6 caracteres")]
    [MaxLength(200)]
    [Column("Pwd")]
    public string password { get; set; } = string.Empty;
    [Phone]
    [MaxLength(15)]
    public string? Phone { get; set; }
    public int Idrol { get; set; }
    [EmailAddress]
    [MaxLength(200)]
    public string? Email { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    public List<Compra> Compras { get; set; } = [];

    public virtual Rol IdrolNavigation { get; set; } = null!;

    public List<Venta> Venta { get; set; } = [];

    public List<Cliente> ClientesCreados { get; set; } = [];
}
