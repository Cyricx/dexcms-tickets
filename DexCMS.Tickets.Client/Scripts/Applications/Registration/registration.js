define([
    'angular',
    './directives/index',
    './services/index',
    'ngStorage',
    'angular-resource',
    'angular-sanitize',
    'angular-ui-router',
    'tg-angular-validator',
    'ngtoast',
    'oclazyload',
    'dexcms-globals-shared',
    'dexcms-globals-base',
    'dexcms-globals-tickets',
], function (angular, directivesBuilder, servicesBuilder) {
    'use strict';
    var regApp = angular.module('dexCMSRegistrationApp', [
        'dexCMSGlobalsShared',
        'dexCMSGlobalsBase',
        'dexCMSGlobalsTickets',
        'angularValidator', 'ngToast', 'ngStorage']);

    directivesBuilder(regApp);
    servicesBuilder(regApp);

    regApp.controller('RegistrationCtrl', ['$scope', 'EventInfo', 'Registrations',
        function ($scope, EventInfo, Registrations) {
        $scope.display = 'addTicket';
        $scope.$on('addedTickets', function () {
            $scope.display = 'chooseOptions';
        });
            $scope.eventSegment = EventInfo.segment;
            $scope.hasCheckedRegistration = false;
            $scope.registrationOpen = false;
            Registrations.checkEventDisabled($scope.eventSegment).then(function (response) {
                $scope.hasCheckedRegistration = true;
                $scope.registrationOpen = true;
                $scope.registrationDisabledMessage = null;
            }, function (error) {
                $scope.hasCheckedRegistration = true;
                $scope.registrationOpen = false;
                $scope.registrationDisabledMessage = error.data.message;
            });
    }]);

    return regApp;
});