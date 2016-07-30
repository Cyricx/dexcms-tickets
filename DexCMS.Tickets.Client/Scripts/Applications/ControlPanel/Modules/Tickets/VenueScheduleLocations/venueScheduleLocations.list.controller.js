define([
   'controlpanel-app',
   '../venues/venues.service'
], function (app) {
    app.controller('venueScheduleLocationsListCtrl', [
        '$scope',
        'VenueScheduleLocations',
        'DTOptionsBuilder',
        'DTColumnBuilder',
        '$compile',
        '$window',
        '$stateParams',
        '$state',
        '$filter',
        'Venues',
        function ($scope, VenueScheduleLocations, DTOptionsBuilder, DTColumnBuilder, $compile,
            $window, $stateParams, $state, $filter, Venues) {

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


            $scope.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                return VenueScheduleLocations.getListByVenue(_venueID);
            }).withBootstrap().withOption('createdRow', createdRow);

            $scope.dtColumns = [
                DTColumnBuilder.newColumn('venueScheduleLocationID').withTitle('ID'),
                DTColumnBuilder.newColumn('name').withTitle('Name'),
                DTColumnBuilder.newColumn('cssClass').withTitle('CSS'),
                DTColumnBuilder.newColumn('isActive').withTitle('Active?'),
                DTColumnBuilder.newColumn(null).withTitle('').notSortable().renderWith(actionsHtml)
            ];

            function createdRow(row, data, dataIndex) {
                // Recompiling so we can bind Angular directive to the DT
                $compile(angular.element(row).contents())($scope);
            }

            function actionsHtml(data, type, full, meta) {
                var buttons = '<a class="btn btn-warning" ui-sref="venueschedulelocations/:id/:vslID({id: +' + _venueID + ', vslID: ' + data.venueScheduleLocationID + '})">' +
                   '   <i class="fa fa-edit"></i>' +
                   '</a>';
                if (data.scheduleItemCount == 0) {
                    buttons += ' <button class="btn btn-danger" ng-click="delete(' + data.venueScheduleLocationID + ')">' +
                   '   <i class="fa fa-trash-o"></i>' +
                   '</button>';
                } else {
                    buttons += ' <button class="btn btn-danger" ng-disabled="\'true\'">Currently in use</button';
                }
                return buttons;
            }



            $scope.delete = function (id) {
                if (confirm('Are you sure?')) {
                    VenueScheduleLocations.deleteItem(id).then(function (response) {
                        $window.location.reload();
                    });
                }
            };
        }
    ]);
});