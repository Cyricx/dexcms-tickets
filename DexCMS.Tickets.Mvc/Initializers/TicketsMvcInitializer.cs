using DexCMS.Core.Infrastructure.Globals;
using DexCMS.Tickets.Contexts;

namespace DexCMS.Tickets.Mvc.Initializers
{
    public class TicketsMvcInitializer: DexCMSInitializer<IDexCMSTicketsContext>
    {
        public TicketsMvcInitializer(IDexCMSTicketsContext context) : base(context)
        {

        }

        public override void Run()
        {
            (new SettingGroupInitializer(Context)).Run();
            (new SettingInitializer(Context)).Run();
        }
    }
}
