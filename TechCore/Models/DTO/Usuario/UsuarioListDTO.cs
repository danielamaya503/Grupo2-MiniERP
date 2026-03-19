namespace TechCore.Models.DTO.Usuario
{
    public class UsuarioListDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
    }
}
