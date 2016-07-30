define([
   'controlpanel-app',
   '../events/events.navigation.service'
], function (app) {
    app.controller('ticketOptionsEditorCtrl', [
        '$scope',
        'TicketOptions',
        '$stateParams',
        '$state',
        'EventsNavigation',
        function ($scope, TicketOptions, $stateParams, $state, EventsNavigation){

            var id = $stateParams.toID || null;

            $scope.eventID = $stateParams.id;

            $scope.subtitle = (id == null ? "Add " : "Edit ") + "Ticket Option";

            $scope.currentItem = { eventID: $scope.eventID, cbEventAges: []};

            EventsNavigation.getNavigation($scope.eventID, function (data) {
                $scope.navigation = data;
                EventsNavigation.setActive($state.current.name);
            });

            if (id != null) {
                TicketOptions.getItem(id).then(function (response) {
                    $scope.currentItem = response.data;
                });
            }

            $scope.saveAndStay = function (item) {
                $scope.isProcessing = true;
                if (item.ticketOptionID) {
                    TicketOptions.updateItem(item.ticketOptionID, item).then(function (response) {
                        $scope.isProcessing = false;
                    });
                } else {
                    TicketOptions.createItem(item).then(function (response) {
                        $scope.isProcessing = false;
                        $scope.currentItem.ticketOptionID = response.ticketOptionID;
                    });
                }
            }

            $scope.save = function (item) {
                $scope.isProcessing = true;
                if (item.ticketOptionID) {
                    TicketOptions.updateItem(item.ticketOptionID, item).then(function (response) {
                        $scope.isProcessing = false;
                        $state.go('ticketoptions/:id', { id: $scope.eventID });
                    });
                } else {
                    TicketOptions.createItem(item).then(function (response) {
                        $scope.isProcessing = false;
                        $state.go('ticketoptions/:id', { id: $scope.eventID });
                    });
                }
            };

        }
    ]);
});