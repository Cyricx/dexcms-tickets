var configs = require('./Configurations/globals.tickets.settings');

module.exports = function (appPath) {
    return configs(appPath);
};