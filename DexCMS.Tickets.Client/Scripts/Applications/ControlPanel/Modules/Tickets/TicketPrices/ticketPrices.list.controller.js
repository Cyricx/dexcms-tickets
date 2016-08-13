define([
   'controlpanel-app'
], function (app) {
    app.controller('ticketPricesListCtrl', [
        '$scope',
        'TicketPrices',
        '$window',
        'dexCMSControlPanelSettings',
        function ($scope, TicketPrices, $window, dexcmsSettings) {
            $scope.title = "View TicketPrices";

            $scope.table = {
                columns: [
                    { property: 'ticketPriceID', title: 'ID' },
                    {
                        property: '', title: '', disableSorting: true,
                        dataTemplate: dexcmsSettings.startingRoute + 'modules/tickets/ticketprices/_ticketprices.list.buttons.html'
                    }
                ],
                defaultSort: 'ticketPriceID',
                functions: {
                    remove: function (id) {
                        if (confirm('Are you sure?')) {
                            TicketPrices.deleteItem(id).then(function (response) {
                                $window.location.reload();
                            });
                        }
                    }
                },
                filePrefix: 'Ticket Prices'
            };

            TicketPrices.getList().then(function (data) {
                $scope.table.promiseData = data;
            });
        }
    ]);
});