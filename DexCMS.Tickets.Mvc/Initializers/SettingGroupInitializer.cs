using DexCMS.Core.Globals;
using DexCMS.Core.Models;
using DexCMS.Tickets.Contexts;
using System.Data.Entity.Migrations;

namespace DexCMS.Tickets.Mvc.Initializers
{
    class SettingGroupInitializer: DexCMSInitializer<IDexCMSTicketsContext>
    {
        public SettingGroupInitializer(IDexCMSTicketsContext context): base (context)
        {

        }

        public override void Run(bool addDemoContent = true)
        {
            Context.SettingGroups.AddOrUpdate(x => x.SettingGroupName,
                new SettingGroup { SettingGroupName = "Tickets" }
            );
            Context.SaveChanges();
        }
    }
}
