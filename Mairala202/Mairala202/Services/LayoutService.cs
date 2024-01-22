using Mairala202.DAL;
using Microsoft.EntityFrameworkCore;

namespace Mairala202.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _db;

        public LayoutService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Dictionary<string,string>> GetSettingsAsync()
        {
            Dictionary<string, string> settings = await _db.Settings.ToDictionaryAsync(x => x.Key , x=>x.Value) ;
            return settings ;
        }
    }
}
