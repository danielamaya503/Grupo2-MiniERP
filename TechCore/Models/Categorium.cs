using System;
using System.Collections.Generic;

namespace TechCore.Models;

public partial class Categorium
{
    public int CodCategoria { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool? Estado { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
