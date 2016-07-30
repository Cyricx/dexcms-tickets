define([
    'angular',
    './tickets.settings'
], function (angular, settings) {
    var globalTicketsApp = angular.module('dexCMSGlobalsTickets', []);

    globalTicketsApp.constant('dexCMSGlobalsTicketsSettings', settings);

    return globalTicketsApp;
});