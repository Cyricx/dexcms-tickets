using DexCMS.Core.Infrastructure.Extensions;
using DexCMS.Core.Infrastructure.Globals;
using DexCMS.Core.Infrastructure.Initializers.Helpers;
using DexCMS.Tickets.Contexts;
using DexCMS.Tickets.Venues.Models;

namespace DexCMS.Tickets.Initializers
{
    class VenueInitializer : DexCMSInitializer<IDexCMSTicketsContext>
    {
        private StatesReference States;

        public VenueInitializer(IDexCMSTicketsContext context) : base(context) {
            States = new StatesReference(context);
        }

        public override void Run(bool addDemoContent = true)
        {
            if (addDemoContent)
            {
                Context.Venues.AddIfNotExists(x => x.Name,
                    new Venue { Name = "Example Venue", Address = "123 Street", City = "Some City", StateID = States.KS, ZipCode = "12345" }
                );
                Context.SaveChanges();
            }
        }
    }
}
