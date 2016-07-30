define([
    'require',
    'angular',
    'registration-app',
], function (require, angular) {
    'use strict';
    require(['registration-app'], function () {
        angular.bootstrap(document, ['dexCMSRegistrationApp']);
    });
});