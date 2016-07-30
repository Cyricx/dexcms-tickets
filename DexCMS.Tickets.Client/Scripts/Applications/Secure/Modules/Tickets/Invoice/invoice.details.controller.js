define([
    'secure-app'
], function (app) {

    app.controller('invoiceDetailsCtrl', [
        '$scope',
        'RetrieveContents',
        '$stateParams',
        'CashierOrders',
        '$state',
        'Tickets',
        function ($scope, RetrieveContents, $stateParams, CashierOrders, $state, Tickets) {
            $scope.correctedEmail = '';
            RetrieveContents.getContent('securecomplete').then(function (response) {
                $scope.content = response.data;
            });
            $scope.isLoading = false;
            var _loadOrder = function () {
                $scope.isLoading = true;
                CashierOrders.getItem($stateParams.id).then(function (response) {
                    $scope.isLoading = false;
                    $scope.order = response.data;
                    Tickets.removeAllTickets();
                }, function (error) {
                    console.error(error);
                    $scope.isLoading = false;
                });
            };

            _loadOrder();

            $scope.checkPayment = _loadOrder;
           
            $scope.sendNewInvoice = function (email, order) {
                $scope.isProcessing = true;
                CashierOrders.deleteItem(order.orderID, email).then(function (response) {
                    $scope.isProcessing = false;
                    _loadOrder();
                    $scope.correctedEmail = '';
                }, function (error) {
                    console.error(error);
                    $scope.isProcessing = false;
                    _loadOrder();
                });
            };
            //upate to save notes

            $scope.saveNotes = function (order) {
                $scope.isProcessing = true;
                CashierOrders.updateItem(order.orderID, order).then(function (response) {
                    $scope.isProcessing = false;
                    _loadOrder();
                    $state.go('ticketholders/:id', { id: order.orderID });
                }, function (error) {
                    console.error(error);
                    $scope.isProcessing = false;
                    _loadOrder();
                });
            };
        }
    ])
});