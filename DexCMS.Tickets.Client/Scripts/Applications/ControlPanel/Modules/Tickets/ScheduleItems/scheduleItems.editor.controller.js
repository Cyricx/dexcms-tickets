define([
   'controlpanel-app',
   '../events/events.navigation.service',
   '../venueschedulelocations/venueschedulelocations.service',
   '../schedulestatuses/schedulestatuses.service',
   '../scheduletypes/scheduletypes.service',
], function (app) {
    app.controller('scheduleItemsEditorCtrl', [
        '$scope',
        'ScheduleItems',
        '$stateParams',
        '$state',
        'EventsNavigation',
        'VenueScheduleLocations',
        'ScheduleStatuses',
        'ScheduleTypes',
        function ($scope, ScheduleItems, $stateParams, $state, EventsNavigation,
            VenueScheduleLocations, ScheduleStatuses, ScheduleTypes) {
            EventsNavigation.getNavigation($stateParams.id, function (data) {
                $scope.navigation = data;
                EventsNavigation.setActive($state.current.name);
            });

            var id = $stateParams.siID || null;

            $scope.eventID = $stateParams.id;

            $scope.subtitle = (id == null ? "Add " : "Edit ") + "Schedule Item";

            $scope.currentItem = { eventID: $scope.eventID, isAllDay: false };

            VenueScheduleLocations.getListByEvent($scope.eventID).then(function (response) {
                $scope.venueScheduleLocations = response;
            });
            ScheduleStatuses.getList().then(function (response) {
                $scope.scheduleStatuses = response;
            });
            ScheduleTypes.getList().then(function (response) {
                $scope.scheduleTypes = response;
            });
            if (id != null) {
                ScheduleItems.getItem(id).then(function (response) {
                    $scope.currentItem = response.data;
                });
            }

            $scope.save = function (item) {
                $scope.isProcessing = false;
                if ($scope.currentItem.isAllDay) {
                    $scope.currentItem.endDate = null;
                }
                if (item.scheduleItemID) {
                    ScheduleItems.updateItem(item.scheduleItemID, item).then(function (response) {
                        $scope.isProcessing = true;

                        $state.go('scheduleitems/:id', { id: $scope.eventID });
                    });
                } else {
                    ScheduleItems.createItem(item).then(function (response) {
                        $scope.isProcessing = true;
                        $state.go('scheduleitems/:id', { id: $scope.eventID });
                    });
                }
            }
        }
    ]);
});