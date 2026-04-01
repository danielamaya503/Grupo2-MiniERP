namespace TechCore.Models.DTO.Cliente
{
    public class ClienteListDTO
    {
        public string Codclien { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public bool Estado { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int IdCreador { get; set; }
        public string NombreCreador { get; set; } = string.Empty;
    }
}
