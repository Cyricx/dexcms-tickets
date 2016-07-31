var cpNavigation = require('./applications.controlpanel.tickets.navigation'),
    cpRoutes = require('./applications.controlpanel.tickets.routes'),
    secureRoutes = require('./applications.secure.tickets.routes'),
    globalsSettings = require('./globals.tickets.settings');

module.exports = function (appPath, overrides) {
    overrides = overrides || {};
    overrides.navigation = overrides.navigation || {};

    var settings = [];
    settings.push(cpNavigation(appPath, overrides.navigation));
    settings.push(cpRoutes(appPath));
    settings.push(secureRoutes(appPath));
    settings.push(globalsSettings(appPath));
    return settings;
};