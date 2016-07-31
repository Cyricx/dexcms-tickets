module.exports = function (appPath) {
    return {
        name: 'GlobalsTicketsSettings',
        dest: appPath + '/globals/tickets/config/dexcms.globals.tickets.settings.json',
        options: {
            startingRoute: '../../../' + appPath + '/globals/tickets/'
        }
    };
};