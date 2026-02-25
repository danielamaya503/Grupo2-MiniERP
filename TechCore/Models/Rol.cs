using System;
using System.Collections.Generic;

namespace TechCore.Models;

public partial class Rol
{
    public int Id { get; set; }

    public string NombreRol { get; set; } = null!;

    public bool? Habilitado { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
