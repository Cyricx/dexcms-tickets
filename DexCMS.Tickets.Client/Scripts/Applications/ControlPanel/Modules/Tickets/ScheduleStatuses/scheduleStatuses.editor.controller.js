define([
   'controlpanel-app'
], function (app) {
    app.controller('scheduleStatusesEditorCtrl', [
        '$scope',
        'ScheduleStatuses',
        '$stateParams',
        '$state',
        function ($scope, ScheduleStatuses, $stateParams, $state) {

            var id = $stateParams.id || null;

            $scope.title = (id == null ? "Add " : "Edit ") + "Schedule Status";

            $scope.currentItem = {isActive:true};

            if (id != null) {
                ScheduleStatuses.getItem(id).then(function (response) {
                    $scope.currentItem = response.data;
                });
            }

            $scope.save = function (item) {
                $scope.isProcessing = true;
                if (item.scheduleStatusID) {
                    ScheduleStatuses.updateItem(item.scheduleStatusID, item).then(function (response) {
                        $state.go('schedulestatuses');
                        $scope.isProcessing = false;
                    });
                } else {
                    ScheduleStatuses.createItem(item).then(function (response) {
                        $state.go('schedulestatuses');
                        $scope.isProcessing = false;
                    });
                }
            }
        }
    ]);
});