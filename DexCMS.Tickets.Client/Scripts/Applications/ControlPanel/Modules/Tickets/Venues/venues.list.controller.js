define([
    'controlpanel-app'
], function (app) {
    app.controller('venuesListCtrl', [
        '$scope',
        'Venues',
        'DTOptionsBuilder',
        'DTColumnBuilder',
        '$compile',
        '$window',
        function ($scope, Venues, DTOptionsBuilder, DTColumnBuilder, $compile, $window) {
            $scope.title = "View Venues";

            $scope.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                return Venues.getList();
            }).withBootstrap().withOption('createdRow', createdRow);

            $scope.dtColumns = [
                DTColumnBuilder.newColumn('venueID').withTitle('ID'),
                DTColumnBuilder.newColumn('name').withTitle('Name'),
                DTColumnBuilder.newColumn(null).withTitle('Schedule Locations').notSortable().renderWith(locationsHtml),
                DTColumnBuilder.newColumn(null).withTitle('').notSortable().renderWith(actionsHtml)
            ];

            function createdRow(row, data, dataIndex) {
                // Recompiling so we can bind Angular directive to the DT
                $compile(angular.element(row).contents())($scope);
            }

            function actionsHtml(data, type, full, meta) {
                var buttons = '<a class="btn btn-warning" ui-sref="venues/:id({id:' + data.venueID + '})">' +
                   '   <i class="fa fa-edit"></i>' +
                   '</a>';
                buttons += ' <button class="btn btn-danger" ng-click="delete(' + data.venueID + ')">' +
                   '   <i class="fa fa-trash-o"></i>' +
                   '</button>';
                return buttons;
            }

            function locationsHtml(data, type, full, meta) {
                return '<a class="btn btn-primary" ui-sref="venueschedulelocations/:id({id:' + data.venueID + '})">' +
                    'View</a>';
            }

            $scope.delete = function (id) {
                if (confirm('Are you sure?')) {
                    Venues.deleteItem(id).then(function (response) {
                        $window.location.reload();
                    });
                }
            };
        }
    ]);
});