define([
   'controlpanel-app'
], function (app) {
    app.controller('eventSeriesListCtrl', [
        '$scope',
        'EventSeries',
        'DTOptionsBuilder',
        'DTColumnBuilder',
        '$compile',
        '$window',
        function ($scope, EventSeries, DTOptionsBuilder, DTColumnBuilder, $compile, $window) {
            $scope.title = "View EventSeries";

            $scope.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                return EventSeries.getList();
            }).withBootstrap().withOption('createdRow', createdRow);

            $scope.dtColumns = [
                DTColumnBuilder.newColumn('eventSeriesID').withTitle('ID'),
                DTColumnBuilder.newColumn('seriesName').withTitle('Name'),
                DTColumnBuilder.newColumn('isActive').withTitle('Is Active?'),
                DTColumnBuilder.newColumn(null).withTitle('').notSortable().renderWith(actionsHtml)
            ];

            function createdRow(row, data, dataIndex) {
                // Recompiling so we can bind Angular directive to the DT
                $compile(angular.element(row).contents())($scope);
            }

            function actionsHtml(data, type, full, meta) {
                var buttons = '<a class="btn btn-warning" ui-sref="eventseries/:id({id: +' + data.eventSeriesID + '})">' +
                   '   <i class="fa fa-edit"></i>' +
                   '</a>';
                    buttons += ' <button class="btn btn-danger" ng-click="delete(' + data.eventSeriesID + ')">' +
                   '   <i class="fa fa-trash-o"></i>' +
                   '</button>';
                return buttons;
            }

            $scope.delete = function (id) {
                if (confirm('Are you sure?')) {
                    EventSeries.deleteItem(id).then(function (response) {
                        $window.location.reload();
                    });
                }
            };
        }
    ]);
});