using DexCMS.Core.Extensions;
using DexCMS.Core.Globals;
using DexCMS.Core.Models;
using DexCMS.Tickets.Contexts;
using System.Linq;

namespace DexCMS.Tickets.Mvc.Initializers
{
    class SettingInitializer : DexCMSInitializer<IDexCMSTicketsContext>
    {
        public SettingInitializer(IDexCMSTicketsContext context) : base(context)
        {
        }
        public override void Run(bool addDemoContent = true)
        {
            int TicketsGroup = Context.SettingGroups.Where(x => x.SettingGroupName == "Tickets").Select(x => x.SettingGroupID).Single();
            int BoolDataType = Context.SettingDataTypes.Where(x => x.Name == "Bool").Select(x => x.SettingDataTypeID).Single();

            Context.Settings.AddIfNotExists(x => x.Name,
                new Setting { Name = "ShowPublicEvents", Value = "true", SettingDataTypeID = BoolDataType, SettingGroupID = TicketsGroup }
            );
            Context.SaveChanges();
        }
    }
}
