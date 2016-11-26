using DexCMS.Core.Infrastructure.Globals;
using DexCMS.Tickets.Contexts;

namespace DexCMS.Tickets.Mvc.Initializers
{
    public class TicketsInitializer: DexCMSInitializer<IDexCMSTicketsContext>
    {
        public TicketsInitializer(IDexCMSTicketsContext context) : base(context)
        {

        }

        public override void Run()
        {
            (new SettingGroupInitializer(Context)).Run();
            (new SettingInitializer(Context)).Run();
        }
    }
}
