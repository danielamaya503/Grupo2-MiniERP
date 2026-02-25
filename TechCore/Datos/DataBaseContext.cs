using Microsoft.EntityFrameworkCore;

namespace TechCore.Datos
{
    public class DataBaseContext: DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }
       
    }
}
