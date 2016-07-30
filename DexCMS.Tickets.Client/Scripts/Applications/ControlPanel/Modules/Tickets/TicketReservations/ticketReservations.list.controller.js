define([
   'controlpanel-app',
   '../events/events.navigation.service',
], function (app) {
    app.controller('ticketReservationsListCtrl', [
        '$scope',
        'TicketReservations',
        '$stateParams',
        '$state',
        'EventsNavigation',
        function ($scope, TicketReservations, $stateParams, $state, EventsNavigation) {

            $scope.configObject = {};


            $scope.eventID = $stateParams.id;
            var id = $stateParams.tdID || null;
            $scope.discountID = id;
            $scope.buildSubTitle = function () {
                if ($scope.currentItem) {
                    return "Manage Reservations for " + $scope.currentItem.name;

                } else {
                    return "Loading...";
                }
            };

            EventsNavigation.getNavigation($scope.eventID, function (data) {
                $scope.navigation = data;
                EventsNavigation.setActive($state.current.name);
            });

            if (id != null) {
                TicketReservations.getListByDiscount(id).then(function (response) {
                    $scope.currentItem = response.data;
                    $scope.configObject.referenceId = $scope.currentItem.ticketDiscountID;
                    $scope.configObject.maximumAvailable = $scope.currentItem.maximumAvailable;
                    $scope.configObject.totalReservations = $scope.currentItem.totalReservations;
                });
            }
        }
    ]);
});