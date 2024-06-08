using Pickfc.DAL.Infrastructure;
using Pickfc.DAL.Interfaces.IDB;
using Pickfc.Model.Context;

namespace Pickfc.DAL.WorkUnits
{
    public class PickfcWorkUnit : WorkUnit<PickfcContext>
    {
        public PickfcWorkUnit(IDBFactory<PickfcContext> dBFactory) : base(dBFactory) { }
    }
}