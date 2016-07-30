define([
    'secure-app'
], function (app) {

    app.controller('paymentDetailsCtrl', [
        '$scope',
        'RetrieveContents',
        '$stateParams',
        'CashierOrders',
        '$state',
        'Tickets',
        'ngToast',
        function ($scope, RetrieveContents, $stateParams, CashierOrders, $state, Tickets, ngToast) {
            $scope.orderStatuses = [
                { id: 0, name: "Pending"}, 
                { id: 1, name: "Partial"}, 
                { id: 2, name: "Complete"}, 
                { id: 3, name: "Refunded" },
            ];
            $scope.paymentTypes = [
                { id: 0, name: "Pending" },
                { id: 1, name: "Cash" },
                { id: 2, name: "Check" },
                { id: 6, name: "Coupons" },
                { id: 3, name: "Paypal" },
                { id: 4, name: "NoCharge" },
                { id: 5, name: "PendingInvoice" }
            ];


            RetrieveContents.getContent('securecomplete').then(function (response) {
                $scope.content = response.data;
            });

            CashierOrders.getItem($stateParams.id).then(function (response) {
                $scope.order = response.data;
                Tickets.removeAllTickets();
            });
           
            $scope.updateOrder = function (order) {
                CashierOrders.updateItem(order.orderID, order).then(function (response) {
                    if ((order.orderStatus === 2 || order.orderStatus === 3) &&
                        (order.paymentType !== 0 && order.paymentType !==5)) {
                        $state.go('ticketholders/:id', { id: order.orderID });
                    } else {
                        ngToast.create({
                            className: 'danger',
                            content:
                                'Order is not marked complete or payment is pending!<br />' +
                                'Please correct and save again.'
                        });
                    }
                });
            };
        }
    ])
});