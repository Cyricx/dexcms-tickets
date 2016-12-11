using DexCMS.Core.Infrastructure.Contexts;
using DexCMS.Core.Infrastructure.Extensions;
using DexCMS.Core.Infrastructure.Globals;
using DexCMS.Core.Infrastructure.Initializers.Helpers;
using DexCMS.Core.Infrastructure.Models;
using DexCMS.Tickets.Initializers.Helpers;

namespace DexCMS.Tickets.Initializers
{
    class SettingsInitializer : DexCMSInitializer<IDexCMSCoreContext>
    {
        private SettingDataTypesReference DataTypes;
        private TicketSettingGroupsReference Groups;

        public SettingsInitializer(IDexCMSCoreContext context) : base(context)
        {
            DataTypes = new SettingDataTypesReference(Context);
            Groups = new TicketSettingGroupsReference(Context);
        }


        public override void Run()
        {
            Context.Settings.AddIfNotExists(x => x.Name,
                 new Setting { Name = "ShowPublicEvents", Value = "True", SettingDataTypeID = DataTypes.Bool, SettingGroupID = Groups.Tickets }
            );
            Context.SaveChanges();
        }
    }
}
