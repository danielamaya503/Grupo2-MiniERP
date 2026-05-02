namespace TechCore.Models.DTO.Dashboards
{
    public class BodegaDashboardDTO
    {
        public int TotalProductos { get; set; }
        public decimal ValorInventario { get; set; }
        public int ItemsStockBajo { get; set; }
        public List<StockAlertaDTO> AlertasStock { get; set; } = [];
        public List<CompraResumenDTO> ComprasRecientes { get; set; } = [];
    }

    public class StockAlertaDTO
    {
        public string CodProd { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public int Faltantes => StockMinimo - Stock;
    }

    public class CompraResumenDTO
    {
        public string NOrden { get; set; } = string.Empty;
        public int OrdenN { get; set; }
        public string Proveedor { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public int Estado { get; set; }
    }
}
