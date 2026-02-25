using System;
using System.Collections.Generic;

namespace TechCore.Models;

public partial class Proveedore
{
    public string Codprovee { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? Direccion { get; set; }

    public int? Estado { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
