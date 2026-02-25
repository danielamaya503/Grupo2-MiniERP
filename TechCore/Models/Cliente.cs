using System;
using System.Collections.Generic;

namespace TechCore.Models;

public partial class Cliente
{
    public string Codclien { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? Direccion { get; set; }

    public bool? Estado { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
