define([
   'controlpanel-app'
], function (app) {
    app.controller('venueScheduleLocationsEditorCtrl', [
        '$scope',
        'VenueScheduleLocations',
        '$stateParams',
        '$state',
        function ($scope, VenueScheduleLocations, $stateParams, $state) {

            var _venueID = $stateParams.id;
            $scope.venueID = _venueID;
            var id = $stateParams.vslID;
            $scope.title = (id == null ? "Add " : "Edit ") + "Venue Schedule Location";

            $scope.currentItem = { venueID: _venueID, isActive: true};

            if (id != null) {
                VenueScheduleLocations.getItem(id).then(function (response) {
                    $scope.currentItem = response.data;
                });
            }

            $scope.save = function (item) {
                $scope.isProcessing = true;
                if (item.venueScheduleLocationID) {
                    VenueScheduleLocations.updateItem(item.venueScheduleLocationID, item).then(function (response) {
                        $state.go('venueschedulelocations/:id', { id: _venueID });
                        $scope.isProcessing = false;
                    });
                } else {
                    VenueScheduleLocations.createItem(item).then(function (response) {
                        $state.go('venueschedulelocations/:id', { id: _venueID });
                        $scope.isProcessing = false;
                    });
                }
            }
        }
    ]);
});