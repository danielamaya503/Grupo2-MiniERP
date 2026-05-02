using TechCore.Models.DTO.Dashboards;

namespace TechCore.Services.Interfaces.Dashboard
{
    public interface IBodegaDashboard
    {
        Task<BodegaDashboardDTO> ObtenerDashboardAsync();
    }
}
