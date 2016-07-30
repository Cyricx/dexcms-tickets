define([
    'json!./config/dexcms.globals.tickets.settings.json'
], function (settings) {
    settings = settings || {};

    settings.startingRoute = settings.startingRoute || '../../../scripts/dexcmsapps/globals/tickets/';

    return settings;

});