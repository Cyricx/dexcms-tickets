module.exports = function (appPath) {

    return {
        name: 'ApplicationsControlPanelRoutes',
        dest: appPath + '/applications/controlpanel/config/dexcms.controlpanel.routes.json',
        options: [
            {
                "name": "events",
                "module": "tickets",
                "routes": [
                    {
                        "viewType": "editor",
                        "path": "/new"
                    },
                    {
                        "viewType": "editor",
                        "path": "/:id"
                    },
                    {
                        "viewType": "list",
                        "path": ""
                    }
                ]
            },
            {
                "name": "eventSeries",
                "module": "tickets"
            },
            {
                "name": "scheduleTypes",
                "module": "tickets"
            },
            {
                "name": "scheduleStatuses",
                "module": "tickets"
            },
            {
                "name": "venues",
                "module": "tickets"
            },
            {
                "name": "eventFaqCategories",
                "module": "tickets",
                "routes": [
                    {
                        "viewType": "list",
                        "path": "/:id"
                    }
                ]
            },
            {
                "name": "eventDashboards",
                "module": "tickets",
                "routes": [
                    {
                        "viewType": "list",
                        "path": "/:id"
                    }
                ]
            },
            {
                "name": "eventTickets",
                "module": "tickets",
                "routes": [
                    {
                        "viewType": "list",
                        "path": "/:id"
                    }
                ]
            },
            {
                "name": "orders",
                "module": "tickets",
                "routes": [
                    {
                        "viewType": "list",
                        "path": ""
                    },
                    {
                        "viewType": "details",
                        "path": "/details/:id"
                    }
                ]
            },
            {
                "name": "ticketCutoffs",
                "module": "tickets",
                "routes": [
                    {
                        "viewType": "list",
                        "path": "/:id"
                    }
                ]
            },
            {
                "name": "ticketReservations",
                "module": "tickets",
                "routes": [
                    {
                        "viewType": "list",
                        "path": "/:id/:tdID"
                    }
                ]
            },
            {
                "name": "ticketDiscounts",
                "module": "tickets",
                "routes": [
                    {
                        "viewType": "list",
                        "path": "/:id"
                    },
                    {
                        "viewType": "editor",
                        "path": "/:id/new"
                    },
                    {
                        "viewType": "editor",
                        "path": "/:id/:tdID"
                    }
                ]
            },
            {
                "name": "ticketOptions",
                "module": "tickets",
                "routes": [
                    {
                        "viewType": "list",
                        "path": "/:id"
                    },
                    {
                        "viewType": "editor",
                        "path": "/:id/new"
                    },
                    {
                        "viewType": "editor",
                        "path": "/:id/:toID"
                    }
                ]
            },
            {
                "name": "ticketAssignments",
                "module": "tickets",
                "routes": [
                    {
                        "viewType": "list",
                        "path": ""
                    },
                    {
                        "viewType": "editor",
                        "path": "/:id"
                    }
                ]
            },
            {
                "name": "venueScheduleLocations",
                "module": "tickets",
                "routes": [
                    {
                        "viewType": "list",
                        "path": "/:id"
                    },
                    {
                        "viewType": "editor",
                        "path": "/:id/new"
                    },
                    {
                        "viewType": "editor",
                        "path": "/:id/:vslID"
                    }
                ]
            },
            {
                "name": "ticketOptionChoices",
                "module": "tickets",
                "routes": [
                    {
                        "viewType": "editor",
                        "path": "/:id/:toID/new"
                    },
                    {
                        "viewType": "editor",
                        "path": "/:id/:toID/:tocID"
                    }
                ]
            },
            {
                "name": "scheduleItems",
                "module": "tickets",
                "routes": [
                    {
                        "viewType": "list",
                        "path": "/:id"
                    },
                    {
                        "viewType": "editor",
                        "path": "/:id/new"
                    },
                    {
                        "viewType": "editor",
                        "path": "/:id/:siID"
                    }
                ]
            },
            {
                "name": "eventAgeGroups",
                "module": "tickets",
                "routes": [
                    {
                        "viewType": "list",
                        "path": "/:id"
                    },
                    {
                        "viewType": "editor",
                        "path": "/:id/new"
                    },
                    {
                        "viewType": "editor",
                        "path": "/:id/:eageID"
                    }
                ]
            },
            {
                "name": "reportingTickets",
                "module": "tickets",
                "routes": [
                    {
                        "viewType": "list",
                        "path": "/:id"
                    }
                ]
            }
        ]
    };
};