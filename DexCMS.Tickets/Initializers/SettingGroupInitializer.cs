using DexCMS.Core.Infrastructure.Contexts;
using DexCMS.Core.Infrastructure.Globals;
using DexCMS.Core.Infrastructure.Models;
using System.Data.Entity.Migrations;

namespace DexCMS.Tickets.Initializers
{
    class SettingGroupInitializer : DexCMSInitializer<IDexCMSCoreContext>
    {
        public SettingGroupInitializer(IDexCMSCoreContext context) : base(context)
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
