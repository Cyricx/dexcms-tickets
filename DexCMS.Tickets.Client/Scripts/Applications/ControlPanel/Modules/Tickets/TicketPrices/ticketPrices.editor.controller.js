define([
   'controlpanel-app'
], function (app) {
    app.controller('ticketPricesEditorCtrl', [
        '$scope',
        'TicketPrices',
        '$stateParams',
        '$state',
        function ($scope, TicketPrices, $stateParams, $state) {

            var id = $stateParams.id || null;

            $scope.title = (id == null ? "Add " : "Edit ") + "TicketPrice";

            $scope.currentItem = {};

            if (id != null) {
                TicketPrices.getItem(id).then(function (response) {
                    $scope.currentItem = response.data;
                    //Use to resolve date isses
                    //$scope.currentItem.startDate = new Date($scope.currentItem.startDate);
                    //if ($scope.currentItem.endDate) {
                    //    $scope.currentItem.endDate = new Date($scope.currentItem.endDate);
                    //}
                });
            }

            $scope.save = function (item) {
                if (item.ticketPriceID) {
                    TicketPrices.updateItem(item.ticketPriceID, item).then(function (response) {
                        $state.go('ticketprices');
                    });
                } else {
                    TicketPrices.createItem(item).then(function (response) {
                        $state.go('ticketprices');
                    });
                }
            }
        }
    ]);
});