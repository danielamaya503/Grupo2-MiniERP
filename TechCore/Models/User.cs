using System;
using System.Collections.Generic;

namespace TechCore.Models;

public partial class User
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Pwd { get; set; } = null!;

    public string? Phone { get; set; }

    public int Idrol { get; set; }

    public string? Email { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual Rol IdrolNavigation { get; set; } = null!;

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
