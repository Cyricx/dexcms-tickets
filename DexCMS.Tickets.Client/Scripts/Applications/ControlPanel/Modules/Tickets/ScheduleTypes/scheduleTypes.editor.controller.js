define([
   'controlpanel-app'
], function (app) {
    app.controller('scheduleTypesEditorCtrl', [
        '$scope',
        'ScheduleTypes',
        '$stateParams',
        '$state',
        function ($scope, ScheduleTypes, $stateParams, $state) {

            var id = $stateParams.id || null;

            $scope.title = (id == null ? "Add " : "Edit ") + "Schedule Type";

            $scope.currentItem = {isActive: true};

            if (id != null) {
                ScheduleTypes.getItem(id).then(function (response) {
                    $scope.currentItem = response.data;
                });
            }

            $scope.save = function (item) {
                $scope.isProcessing = true;
                if (item.scheduleTypeID) {
                    ScheduleTypes.updateItem(item.scheduleTypeID, item).then(function (response) {
                        $state.go('scheduletypes');
                        $scope.isProcessing = false;
                    });
                } else {
                    ScheduleTypes.createItem(item).then(function (response) {
                        $state.go('scheduletypes');
                        $scope.isProcessing = false;
                    });
                }
            }
        }
    ]);
});