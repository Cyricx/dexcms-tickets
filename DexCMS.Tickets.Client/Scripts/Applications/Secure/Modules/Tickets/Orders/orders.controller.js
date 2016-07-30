define([
    'secure-app'
], function (app) {
    app.controller('ordersListCtrl', [
        '$scope',
        'SecureOrders',
        function ($scope, SecureOrders) {

            SecureOrders.getList().then(function (response) {
                var orders = [];
                angular.forEach(response.data, function (order) {
                    if (order.orderStatus === "Complete") {
                        orders.push(order);
                    }
                });
                $scope.orders = orders;
            });

        }
    ]);
});