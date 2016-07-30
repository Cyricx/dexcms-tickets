define([
   'controlpanel-app'
], function (app) {
    app.controller('ticketPricesListCtrl', [
        '$scope',
        'TicketPrices',
        'DTOptionsBuilder',
        'DTColumnBuilder',
        '$compile',
        '$window',
        function ($scope, TicketPrices, DTOptionsBuilder, DTColumnBuilder, $compile, $window) {
            $scope.title = "View TicketPrices";

            $scope.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                return TicketPrices.getList();
            }).withBootstrap().withOption('createdRow', createdRow);

            $scope.dtColumns = [
                DTColumnBuilder.newColumn('ticketPriceID').withTitle('ID'),

                DTColumnBuilder.newColumn(null).withTitle('').notSortable().renderWith(actionsHtml)
            ];

            function createdRow(row, data, dataIndex) {
                // Recompiling so we can bind Angular directive to the DT
                $compile(angular.element(row).contents())($scope);
            }

            function actionsHtml(data, type, full, meta) {
                var buttons = '<a class="btn btn-warning" ui-sref="ticketprices/:id({id: +' + data.ticketPricesID + '})">' +
                   '   <i class="fa fa-edit"></i>' +
                   '</a>';
                    buttons += ' <button class="btn btn-danger" ng-click="delete(' + data.ticketPricesID + ')">' +
                   '   <i class="fa fa-trash-o"></i>' +
                   '</button>';
                return buttons;
            }

            $scope.delete = function (id) {
                if (confirm('Are you sure?')) {
                    TicketPrices.deleteItem(id).then(function (response) {
                        $window.location.reload();
                    });
                }
            };
        }
    ]);
});