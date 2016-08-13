define([
   'controlpanel-app',
   '../venues/venues.service'
], function (app) {
    app.controller('venueScheduleLocationsListCtrl', [
        '$scope',
        'VenueScheduleLocations',
        '$window',
        '$stateParams',
        '$state',
        '$filter',
        'Venues',
        'dexCMSControlPanelSettings',
        function ($scope, VenueScheduleLocations,
            $window, $stateParams, $state, $filter, Venues, dexcmsSettings) {

            var _venueID = $stateParams.id;
            $scope.venueID = _venueID;
            $scope.buildTitle = function () {
                if ($scope.venue) {
                    return "View Schedule Locations for " + $scope.venue.name;
                } else {
                    return "Loading...";
                }
            };

            if (_venueID) {
                Venues.getItem(_venueID).then(function (response) {
                    $scope.venue = response.data;
                });
            }

            $scope.table = {
                columns: [
                    { property: 'venueScheduleLocationID', title: 'ID' },
                    { property: 'name', title: 'Name' },
                    { property: 'cssClass', title: 'CSS' },
                    { property: 'isActive', title: 'Active?' },
                    {
                        property: '', title: '', disableSorting: true,
                        dataTemplate: dexcmsSettings.startingRoute + 'modules/tickets/venueschedulelocations/_venueschedulelocations.list.buttons.html'
                    }
                ],
                defaultSort: 'venueScheduleLocationID',
                functions: {
                    remove: function (id) {
                        if (confirm('Are you sure?')) {
                            VenueScheduleLocations.deleteItem(id).then(function (response) {
                                $window.location.reload();
                            });
                        }
                    }
                },
                filePrefix: 'Venue-Schedule-Locations'
            };

            VenueScheduleLocations.getListByVenue(_venueID).then(function (data) {
                $scope.table.promiseData = data;
            });
        }
    ]);
});