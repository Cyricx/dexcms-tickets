define([
   'controlpanel-app'
], function (app) {
    app.controller('scheduleTypesListCtrl', [
        '$scope',
        'ScheduleTypes',
        '$window',
        'dexCMSControlPanelSettings',
        function ($scope, ScheduleTypes, $window, dexcmsSettings) {
            $scope.title = "View Schedule Types";

            $scope.table = {
                columns: [
                    { property: 'scheduleTypeID', title: 'ID' },
                    { property: 'name', title: 'Name' },
                    { property: 'cssClass', title: 'CSS' },
                    { property: 'isActive', title: 'Active?' },
                    {
                        property: '', title: '', disableSorting: true,
                        dataTemplate: dexcmsSettings.startingRoute + 'modules/tickets/scheduletypes/_scheduletypes.list.buttons.html'
                    }
                ],
                defaultSort: 'scheduleTypeID',
                functions: {
                    remove: function (id) {
                        if (confirm('Are you sure?')) {
                            ScheduleTypes.deleteItem(id).then(function (response) {
                                $window.location.reload();
                            });
                        }
                    }
                },
                filePrefix: 'Schedule-Types'
            };

            ScheduleTypes.getList().then(function (data) {
                $scope.table.promiseData = data;
            });
        }
    ]);
});