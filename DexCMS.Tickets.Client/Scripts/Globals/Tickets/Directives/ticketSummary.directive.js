define([
    '../tickets',
    'underscore',
], function (module, _) {
    module.directive('ttcmsTicketSummary', [
        'dexCMSGlobalsTicketsSettings',
        function (Settings) {
            return {
                templateUrl: Settings.startingRoute + 'directives/_ticketsummary.html',
                restrict: 'E',
                scope: {
                    'displayType': '=',
                    'goToPay': '&',
                    'goToNoPay': '&'
                },
                controller: ['$scope', 'Tickets', '$filter',
                    function ($scope, Tickets, $filter) {

                        $scope.displayType = $scope.displayType || 'viewCart';

                        $scope.tickets = Tickets.getTickets();

                        $scope.totalPrice = function () {
                            var price = 0;
                            angular.forEach($scope.tickets, function (ticket) {
                                if (ticket.totalPrice) {
                                    price += ticket.totalPrice;
                                }
                            });
                            return price;
                        };
                        $scope.isADisabled = function () {
                            var disabled = false;
                            angular.forEach($scope.tickets, function (ticket) {
                                if (!disabled) {
                                    disabled = !ticket.isValid;
                                }
                            });
                            return disabled;
                        };

                        var init = function () {
                            var total = $scope.totalPrice();
                            $scope.displayType = total == 0 ? 'noCharge' : 'amountDue';
                        };

                        if ($scope.displayType == 'pay') {
                            init();
                        }

                    }//end ctrl
                ]
            };
        }]
    );
});