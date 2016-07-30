define([
    'angular',
    'controlpanel-app',
    '../ticketprices/ticketprices.service',
], function (angular, module) {
    module.directive('ttcmsTicketPricesAreasDetails', [
        'dexCMSControlPanelSettings',
        function (ttcmsSettings) {
            return {
                restrict: "E",
                replace: true,
                scope: {
                    "ticketArea": "="
                },
                templateUrl: ttcmsSettings.startingRoute + '/modules/ticketing/ticketcutoffs/_ticketpricesareas.details.html',
                controller: [
                    '$scope',
                    'TicketPrices',
                    function ($scope, TicketPrices) {
                        $scope.savePrice = function (price) {
                            if (price.ticketPriceID) {
                                TicketPrices.updateItem(price.ticketPriceID, price).then(function (response) {
                                    price.isEditting = false;
                                })
                            } else {
                                price.ticketPriceID = 0;
                                TicketPrices.createItem(price).then(function (response) {
                                    angular.extend(price, response);
                                    price.isEditting = false;
                                });
                            }
                        };

                        $scope.deletePrice = function (price) {
                            if (confirm('Are you sure?')) {
                                TicketPrices.deleteItem(price.ticketPriceID).then(function (response) {
                                    price.isEditting = false;
                                    price.basePrice = null;
                                    price.ticketPriceID = null;
                                });
                            }
                        };

                }]
            }
        }
    ])
});