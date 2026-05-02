using TechCore.Models.DTO.Dashboards;

namespace TechCore.Services.Interfaces
{
    public interface IBodegaDashboard
    {
        Task<BodegaDashboardDTO> ObtenerDashboardAsync();
    }
}
