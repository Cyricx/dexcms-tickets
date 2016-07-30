define([
    'secure-app'
], function (app) {
    app.controller('ticketHoldersEditorCtrl', [
        '$scope',
        '$stateParams',
        'TicketHolders',
        function ($scope, $stateParams, TicketHolders) {

            TicketHolders.getItem($stateParams.id).then(function (response) {
                $scope.order = response.data;
            });
            $scope.userData = userData;

            $scope.saveHolders = function (order) {
                $scope.isProcessing = true;
                var someoneHasNotArrived = false;
                angular.forEach(order.ticketHolders, function (holder) {
                    if (!holder.firstName && !holder.lastName) {
                        holder.hasArrived = false;
                        holder.arrivalTime = null;
                    }
                    if (!holder.hasArrived) {
                        someoneHasNotArrived = true;
                    }
                });
                TicketHolders.updateItem(order.orderID, order).then(function (response) {
                    $scope.order = response.data;
                    $scope.ticketHoldersForm.$setPristine();
                    $scope.isProcessing = false;
                    if (someoneHasNotArrived) {
                        alert('One or more attendees were marked as not arrived!');
                    }
                });
            };
            $scope.setDefaultArrival = function (holder) {
                console.log(holder);
                if (holder.hasArrived) {
                    if (!holder.arrivalTime) {
                        holder.arrivalTime = new Date();
                    }
                }
            };
        }
    ]);
});