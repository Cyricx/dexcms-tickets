define([
   'secure-app',
   'underscore'
], function (app, _) {
    app.controller('checkoutListCtrl', [
        '$scope',
        'Tickets',
        'SecureTicketOptions',
        'SecureOrders',
        '$state',
        '$window',
        '$timeout',
        'PublicEvents',
        'CashierOrders',
        function ($scope
            , Tickets, SecureTicketOptions, SecureOrders, $state, $window, $timeout, PublicEvents, CashierOrders
            ) {

            $scope.displayType = 'pay';
            
            $scope.hideSummary = userData.isCashier === 'True';

            $scope.tickets = Tickets.getTickets();

            PublicEvents.getList().then(function (response) {
                $scope.publicEvents = response;
            });


            var _hasValidated = false;
            $scope.$watch('tickets.length', function (newVal, oldVal) {
                if (!_hasValidated && newVal > 0) {
                    validateTickets();
                }
            });
            var validateTickets = function () {
                _hasValidated = true;
                Tickets.validateTickets();
            };

            $scope.ticketOptions = {};

            angular.forEach(Tickets.getTickets(), function (ticket) {
                if (ticket.options) {
                    angular.forEach(ticket.options, function (value, key) {
                        $scope.ticketOptions[ticket.ticketSeatID] = [];
                        SecureTicketOptions.getOptionDetails(key, value, ticket.ticketDiscountID,
                            ticket.discountConfirmationNumber).then(function (response) {
                                $scope.ticketOptions[ticket.ticketSeatID].push(response.data);
                            });
                    });
                }
            });

            $scope.deleteTicket = function (ticket) {
                if (confirm('Are you sure? This will delete your reservation for this ticket.')) {
                    Tickets.deleteTicket(ticket);
                }
            };


            $scope.goToPay = function (tickets) {
                SecureOrders.createItem($scope.tickets).then(function (response) {
                    $window.location.href = response.approvalUrl;
                });
            };

            $scope.goToNoPay = function (tickets) {
                SecureOrders.createItem($scope.tickets).then(function (response) {
                    $state.go('complete/:id', { id: response.orderID });
                });
            };

            $scope.goToCashOrCheck = function (tickets) {
                $scope.isProcessing = true;
                CashierOrders.createOrder(tickets).then(function (response) {
                    $state.go('payment/:id', { id: response.orderID });
                }, function (error) {
                    console.error(error);
                    $scope.isProcessing = false;
                });
            };
            $scope.goToInvoice = function (tickets, email) {
                $scope.isProcessing = true;
                CashierOrders.createInvoice(tickets, email).then(function (response) {
                    $state.go('invoice/:id', { id: response.orderID });
                }, function (error) {
                    console.error(error);
                    $scope.isProcessing = false;
                });
            };
        }
    ]);
});