using Microsoft.Extensions.Configuration;
using Pickfc.DAL.Infrastructure;
using Pickfc.DAL.Interfaces.IDB;
using Pickfc.Model.Context;

namespace Pickfc.DAL.Factories
{
    public class PickfcDbFactory : Disposable, IDBFactory<PickfcContext>
    {
        private readonly string[] connect = {"Local", "Azure"};
        private readonly IConfiguration configuration;
        public PickfcContext dbContext;

        public PickfcDbFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public PickfcContext Init() 
        {
            return dbContext ?? (dbContext = new PickfcContext(configuration.GetConnectionString(connect[1])));
        }

        protected override void DisposeCore()
        {
            if(dbContext != null)
                dbContext.Dispose();
        }
    }
}
