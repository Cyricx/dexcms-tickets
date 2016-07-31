module.exports = function (appPath, overrides) {

    return {
        name: 'ApplicationsControlPanelNavigation',
        dest: appPath + '/applications/controlpanel/config/dexcms.controlpanel.navigation.json',
        options: [
            {
                "title": "Tickets",
                "icon": "fa-ticket",
                "subLinks": [
                    {
                        "title": "Events",
                        href: overrides.events || "events"
                    },
                    {
                        "title": "Event Series",
                        href: overrides.eventseries || "eventseries"
                    },
                    {
                        "title": "Orders",
                        href: overrides.orders || "orders"
                    },
                    {
                        "title": "Schedule Statuses",
                        href: overrides.schedulestatuses || "schedulestatuses"
                    },
                    {
                        "title": "Schedule Types",
                        href: overrides.scheduletypes || "scheduletypes"
                    },
                    {
                        "title": "Ticket Assignments",
                        href: overrides.ticketassignments || "ticketassignments"
                    },
                    {
                        "title": "Venues",
                        href: overrides.venues || "venues"
                    }
                ]
            }
        ]
    };
};