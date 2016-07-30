define([
    'controlpanel-app'
], function (app) {
    app.controller('ticketsListCtrl', [
        '$scope',
        'Tickets',
        'DTOptionsBuilder',
        'DTColumnBuilder',
        '$compile',
        function ($scope, Tickets, DTOptionsBuilder, DTColumnBuilder, $compile) {
            $scope.title = "View Tickets";

            $scope.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                return TicketPrices.getList();
            }).withBootstrap().withOption('createdRow', createdRow);

            $scope.dtColumns = [
                DTColumnBuilder.newColumn('eventID').withTitle('ID'),
                DTColumnBuilder.newColumn('pageContentHeading').withTitle('ID'),
                DTColumnBuilder.newColumn('eventSeriesName').withTitle('Series'),
                DTColumnBuilder.newColumn('availableCount').withTitle('Available'),
                DTColumnBuilder.newColumn('disabledCount').withTitle('Disabled'),
                DTColumnBuilder.newColumn('reservedCount').withTitle('Reserved'),
                DTColumnBuilder.newColumn('assignedCount').withTitle('Assigned'),
                DTColumnBuilder.newColumn('completeCount').withTitle('Complete'),
                DTColumnBuilder.newColumn(null).withTitle('').notSortable().renderWith(actionsHtml)
            ];

            function createdRow(row, data, dataIndex) {
                // Recompiling so we can bind Angular directive to the DT
                $compile(angular.element(row).contents())($scope);
            }

            function actionsHtml(data, type, full, meta) {
                var buttons = '<a class="btn btn-warning" ui-sref="tickets/:id({id: +' + data.eventID + '})">' +
                   '   <i class="fa fa-search"></i>' +
                   '</a>';
                return buttons;
            }

        }
    ]);
});