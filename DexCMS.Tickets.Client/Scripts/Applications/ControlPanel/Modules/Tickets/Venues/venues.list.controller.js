define([
    'controlpanel-app'
], function (app) {
    app.controller('venuesListCtrl', [
        '$scope',
        'Venues',
        '$window',
        'dexCMSControlPanelSettings',
        function ($scope, Venues, $window, dexcmsSettings) {
            $scope.title = "View Venues";

            $scope.table = {
                columns: [
                    { property: 'venueID', title: 'ID' },
                    { property: 'name', title: 'Name' },
                    {
                        property: '', title: 'Schedule Locations', disableSorting: true,
                        dataTemplate: dexcmsSettings.startingRoute + 'modules/tickets/venues/_venues.list.schedulelocations.html'
                    },
                    {
                        property: '', title: '', disableSorting: true,
                        dataTemplate: dexcmsSettings.startingRoute + 'modules/tickets/venues/_venues.list.buttons.html'
                    }
                ],
                defaultSort: 'venueID',
                functions: {
                    remove: function (id) {
                        if (confirm('Are you sure?')) {
                            Venues.deleteItem(id).then(function (response) {
                                $window.location.reload();
                            });
                        }
                    }
                },
                filePrefix: 'Venues'
            };

            Venues.getList().then(function (data) {
                $scope.table.promiseData = data;
            });
        }
    ]);
});