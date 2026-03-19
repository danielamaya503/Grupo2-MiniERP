namespace TechCore.Models.DTO.Categoria
{
    public class CategoriaListDTO
    {
        public int CodCategoria { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
