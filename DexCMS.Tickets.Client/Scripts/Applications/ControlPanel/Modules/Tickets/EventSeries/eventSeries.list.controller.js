define([
   'controlpanel-app'
], function (app) {
    app.controller('eventSeriesListCtrl', [
        '$scope',
        'EventSeries',
        '$window',
        'dexCMSControlPanelSettings',
        function ($scope, EventSeries, $window, dexcmsSettings) {
            $scope.title = "View Event Series";

            $scope.table = {
                columns: [
                    { property: 'eventSeriesID', title: 'ID' },
                    { property: 'seriesName', title: 'Name' },
                    { property: 'isActive', title: 'Is Active?' },
                    {
                        property: '', title: '', disableSorting: true,
                        dataTemplate: dexcmsSettings.startingRoute + 'modules/tickets/eventseries/_eventseries.list.buttons.html'
                    }
                ],
                defaultSort: 'eventSeriesID',
                functions: {
                    remove: function (id) {
                        if (confirm('Are you sure?')) {
                            EventSeries.deleteItem(id).then(function (response) {
                                $window.location.reload();
                            });
                        }
                    }
                },
                filePrefix: 'Event-Series'
            };

            EventSeries.getList().then(function (data) {
                $scope.table.promiseData = data;
            });
        }
    ]);
});