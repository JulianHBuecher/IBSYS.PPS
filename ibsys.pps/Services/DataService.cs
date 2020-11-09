using IBSYS.PPS.Models;

namespace IBSYS.PPS.Services
{
    public class DataService
    {
        public void InsertDataInFreshDb(IbsysDatabaseContext _db)
        {
            SeedData.Initialize(_db);
        }
    }
}
