using IBSYS.PPS.Models;
using System.Threading.Tasks;

namespace IBSYS.PPS.Services
{
    public class DataService
    {
        public async Task InsertDataInFreshDb(IbsysDatabaseContext _db)
        {
            await SeedData.Initialize(_db);
        }
    }
}
