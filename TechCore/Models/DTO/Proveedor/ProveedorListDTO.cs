namespace TechCore.Models.DTO.Proveedor
{
    public class ProveedorListDTO
    {
        public string CodProvee { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public int Estado { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
