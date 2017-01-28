using System;
using System.Collections.Generic;
using DexCMS.Core.Globals;
using DexCMS.Tickets.Contexts;

namespace DexCMS.Tickets.Mvc.Initializers
{
    public class TicketsMvcInitializer: DexCMSLibraryInitializer<IDexCMSTicketsContext>
    {
        public TicketsMvcInitializer(IDexCMSTicketsContext context) : base(context)
        {

        }

        public override List<Type> Initializers
        {
            get
            {
                return new List<Type>
                {
                    typeof(SettingGroupInitializer),
                    typeof(SettingInitializer)
                };
            }
        }

    }
}
