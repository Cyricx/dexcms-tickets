define([
   'controlpanel-app'
], function (app) {
    app.controller('scheduleStatusesListCtrl', [
        '$scope',
        'ScheduleStatuses',
        '$window',
        'dexCMSControlPanelSettings',
        function ($scope, ScheduleStatuses, $window, dexcmsSettings) {
            $scope.title = "View Schedule Statuses";

            $scope.table = {
                columns: [
                    {
                        property: 'scheduleStatusID',
                        title: 'ID'
                    },
                    {
                        property: 'name',
                        title: 'Name'
                    },
                    {
                        property: 'cssClass',
                        title: 'CSS'
                    },
                    {
                        property: 'isActive',
                        title: 'Active?'
                    },
                    {
                        property: '',
                        title: '',
                        disableSorting: true,
                        dataTemplate: dexcmsSettings.startingRoute + 'modules/tickets/schedulestatuses/_schedulestatuses.list.buttons.html'
                    }
                ],
                defaultSort: 'scheduleStatusID',
                functions: {
                    remove: function (id) {
                        if (confirm('Are you sure?')) {
                            ScheduleStatuses.deleteItem(id).then(function (response) {
                                $window.location.reload();
                            });
                        }
                    }
                },
                filePrefix: 'Schedule-Statuses'
            };

            ScheduleStatuses.getList().then(function (data) {
                $scope.table.promiseData = data;
            });
        }
    ]);
});