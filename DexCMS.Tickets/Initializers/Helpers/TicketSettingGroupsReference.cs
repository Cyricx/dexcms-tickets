using DexCMS.Core.Infrastructure.Contexts;
using DexCMS.Core.Infrastructure.Initializers.Helpers;
using System.Linq;

namespace DexCMS.Tickets.Initializers.Helpers
{
    class TicketSettingGroupsReference:SettingGroupsReference
    {
        public int Tickets { get; set; }
        public TicketSettingGroupsReference(IDexCMSCoreContext context): base(context)
        {
            Tickets = context.SettingGroups.Where(x => x.SettingGroupName == "Tickets").Select(x => x.SettingGroupID).Single();
        }
    }
}
