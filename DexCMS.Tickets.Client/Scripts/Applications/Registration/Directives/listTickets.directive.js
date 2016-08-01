define([
    'underscore',
], function (_) {
    return function (module) {
        module.directive('dexcmsListTickets', function () {
            return {
                templateUrl: '../../../scripts/dexcmsapps/applications/registration/directives/_listTickets.html',
                restrict: 'E',
                controller: ['$scope', 'EventInfo', 'Tickets', 'Registrations', '$filter',
                    function ($scope, EventInfo, Tickets, Registrations, $filter) {
                        Registrations.setSegment(EventInfo.segment);
                        $scope.tickets = Tickets.getTickets();

                        $scope.deleteTicket = function (ticket) {
                            if (confirm('Are you sure? This will delete your reservation for this ticket.')) {
                                Tickets.deleteTicket(ticket);
                            }
                        };

                        $scope.ticketOptions = {};

                        var _buildOptions = function () {
                            var ticketOptions = {};
                            if (Object.keys($scope.ticketOptions).length != 0) {
                                ticketOptions = $scope.ticketOptions;
                            }

                            angular.forEach(Tickets.getTickets(), function (ticket) {
                                if (!ticket.ticketDiscountID) {
                                    Registrations.ticketOptions(ticket.ageID).then(function (response) {
                                        ticketOptions[ticket.ticketSeatID] = response.data;
                                    });
                                } else {
                                    Registrations.ticketOptionsByDiscount(ticket.ageID, ticket.ticketDiscountID).then(function (response) {
                                        ticketOptions[ticket.ticketSeatID] = response.data;
                                    });
                                }
                            });
                            $scope.ticketOptions = ticketOptions;
                        };

                        $scope.$watch('tickets.length', function (newVal, oldVal) {
                            _buildOptions();
                        })

                        _buildOptions();

                        $scope.calculateTicket = function (ticket, valid) {
                            var price = ticket.basePrice;
                            if (ticket.options) {
                                angular.forEach(ticket.options, function (value, key) {
                                    var option = _.findWhere($scope.ticketOptions[ticket.ticketSeatID], { ticketOptionID: parseInt(key) });
                                    if (option) {
                                        var choice = _.findWhere(option.ticketOptionChoices, { ticketOptionChoiceID: parseInt(value) });
                                        if (choice) {
                                            price += choice.adjustedPrice;
                                        }
                                    }
                                });
                            }
                            ticket.isValid = valid;
                            ticket.totalPrice = price;
                            return price;
                        };

                        $scope.getChoiceName = function (choice) {
                            return choice.name + ' +' + $filter('currency')(choice.adjustedPrice);
                        };
                    }]
            }

        });
    };
});