namespace TechCore.Models.DTO.Movimiento
{
    public class MovimientoListDTO
    {
        public string Tipo { get; set; }
        public int Referencia { get; set; } 
        public string CodProd { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
        public DateTime Fecha { get; set; }
        public string Origen { get; set; }  
        public string UsuarioNombre { get; set; }
    }
}
