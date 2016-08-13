define([
   'controlpanel-app',
   '../events/events.navigation.service'
], function (app) {
    app.controller('eventDashboardsListCtrl', [
        '$scope',
        'EventDashboards',
        '$stateParams',
        '$state',
        'EventsNavigation',
        function ($scope, EventDashboards, $stateParams, $state, EventsNavigation) {
            $scope.title = "View EventDashboards";
            EventsNavigation.getNavigation($stateParams.id, function (data) {
                $scope.navigation = data;
                EventsNavigation.setActive($state.current.name);
            });
        }
    ]);
});