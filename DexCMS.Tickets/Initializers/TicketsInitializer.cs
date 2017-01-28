using System;
using System.Collections.Generic;
using DexCMS.Core.Globals;
using DexCMS.Tickets.Contexts;

namespace DexCMS.Tickets.Initializers
{
    public class TicketsInitializer: DexCMSLibraryInitializer<IDexCMSTicketsContext>
    {
        public TicketsInitializer(IDexCMSTicketsContext context) : base(context) { }

        public override List<Type> Initializers
        {
            get
            {
                return new List<Type>
                {
                    typeof(VenueInitializer),
                    typeof(VenueAreaInitializer),
                    typeof(VenueSectionInitializer),
                    typeof(VenueRowInitializer),
                    typeof(VenueScheduleLocationInitializer),
                    typeof(ScheduleStatusInitializer),
                    typeof(ScheduleTypeInitializer)
                };
            }
        }
    }
}
