module.exports = function () {
    return [
        {
            "name": "checkout",
            "module": "tickets",
            "routes": [
                {
                    "viewType": "list",
                    "path": ""
                }
            ]
        },
        {
            "name": "complete",
            "module": "tickets",
            "routes": [
                {
                    "viewType": "details",
                    "path": "/:id"
                }
            ]
        },
        {
            "name": "ticketHolders",
            "module": "tickets",
            "routes": [
                {
                    "viewType": "editor",
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
                }
            ]
        },
        {
            "name": "payment",
            "module": "tickets",
            "routes": [
                {
                    "viewType": "details",
                    "path": "/:id"
                }
            ]
        },
        {
            "name": "invoice",
            "module": "tickets",
            "routes": [
                {
                    "viewType": "details",
                    "path": "/:id"
                }
            ]
        },
        {
            "name": "checkIns",
            "module": "tickets",
            "routes": [
                {
                    "viewType": "list",
                    "path": "/:segment"
                }
            ]
        }
    ];
};