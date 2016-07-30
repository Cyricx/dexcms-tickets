define([
   'controlpanel-app'
], function (app) {
    app.controller('eventSeriesEditorCtrl', [
        '$scope',
        'EventSeries',
        '$stateParams',
        '$state',
        function ($scope, EventSeries, $stateParams, $state) {

            var id = $stateParams.id || null;

            $scope.title = (id == null ? "Add " : "Edit ") + "EventSeries";

            $scope.currentItem = { isActive: true};

            if (id != null) {
                EventSeries.getItem(id).then(function (response) {
                    $scope.currentItem = response.data;
                });
            }

            $scope.save = function (item) {
                if (item.eventSeriesID) {
                    EventSeries.updateItem(item.eventSeriesID, item).then(function (response) {
                        $state.go('eventseries');
                    });
                } else {
                    EventSeries.createItem(item).then(function (response) {
                        $state.go('eventseries');
                    });
                }
            }
        }
    ]);
});