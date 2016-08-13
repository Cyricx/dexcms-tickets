define([
   'controlpanel-app'
], function (app) {
    app.controller('eventsListCtrl', [
        '$scope',
        'Events',
        '$window',
        '$filter',
        'dexCMSControlPanelSettings',
        function ($scope, Events, $window, $filter, dexcmsSettings) {
            $scope.title = "View Events";

            var _renderDate = function (value, item) {
                if (value != null) {
                    return $filter('date')(value, "MM/dd/yyyy h:mm a");
                } else {
                    return null;
                }
            };

            $scope.table = {
                columns: [
                    { property: 'eventID', title: 'ID' },
                    { property: 'pageContentHeading', title: 'Event' },
                    { property: 'eventSeriesName', title: 'Series' },
                    { property: 'venueName', title: 'Venue' },
                    { property: 'eventStart', title: 'Start', dataFunction: _renderDate },
                    { property: 'eventEnd', title: 'End', dataFunction: _renderDate },
                    { property: 'isPublic', title: 'Public?' },
                    {
                        property: '', title: '', disableSorting: true,
                        dataTemplate: dexcmsSettings.startingRoute + 'modules/tickets/events/_events.list.buttons.html'
                    }
                ],
                defaultSort: 'eventID',
                filePrefix: 'Events'
            };

            Events.getList().then(function (data) {
                $scope.table.promiseData = data;
            });
        }
    ]);
});