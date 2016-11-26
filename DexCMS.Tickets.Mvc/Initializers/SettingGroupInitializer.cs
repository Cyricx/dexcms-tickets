using DexCMS.Core.Infrastructure.Globals;
using DexCMS.Core.Infrastructure.Models;
using DexCMS.Tickets.Contexts;
using System.Data.Entity.Migrations;

namespace DexCMS.Tickets.Mvc.Initializers
{
    class SettingGroupInitializer: DexCMSInitializer<IDexCMSTicketsContext>
    {
        public SettingGroupInitializer(IDexCMSTicketsContext context): base (context)
        {

        }

        public override void Run()
        {
            Context.SettingGroups.AddOrUpdate(x => x.SettingGroupName,
                new SettingGroup { SettingGroupName = "Tickets" }
            );
            Context.SaveChanges();
        }
    }
}
