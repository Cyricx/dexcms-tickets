define([
    '../tickets',
    'moment',
    'underscore',
], function (module, moment, _) {
    module.service('Tickets', [
        '$localStorage', 'SecureTicketSeats', '$interval',
        function ($localStorage, SecureTicketSeats, $interval) {

            if (!$localStorage.tickets) {
                $localStorage.tickets = [];
            }
            var _ticketOptions = {};
            var _tickets = $localStorage.tickets;

            var _getTickets = function () {

                return _tickets;
            };

            var _addTicket = function (ticket) {
                _tickets.push(ticket);
                if (_tickets.length == 1) {
                    expirationInterval = _startExpirationInterval();
                }
            };

            var _expireTicket = function (ticket) {
                _tickets.splice(_tickets.indexOf(ticket), 1);
                if (_tickets.length == 0) {
                    expirationInterval = undefined;
                }
            };
            
            var _removeAllTickets = function () {
                $localStorage.tickets = [];
            };
            var _clearTickets = function (ticketsToRemove) {
                _.each(ticketsToRemove, function (ticketToRemove) {
                    var ticket = _.findWhere(_tickets, { ticketSeatID: ticketToRemove.ticketSeatID });
                    _tickets.splice(_tickets.indexOf(ticket), 1);
                });
            };
            
            var _deleteTicket = function (ticket) {
                SecureTicketSeats.unreserveTicket(ticket.ticketSeatID).then(function () {
                    _tickets.splice(_tickets.indexOf(ticket), 1);
                });
            };
            var expirationInterval, _nextTicket = {};

            var _calculateTimeLeft = function (ticket) {
                var expiration = moment(new Date(ticket.expirationTime));
                var diff = expiration.diff(moment());
                if (diff > 0) {
                    return diff;
                } else {
                    _expireTicket(ticket);
                    return null;
                }
            };

            var _formatDiff = function (diff) {
                return moment(diff).format('m:ss');
            }

            var _getNextExpiration = function () {
                if (!angular.isDefined(expirationInterval)) {
                    _startExpirationInterval();
                }
                return _nextTicket;
            };

            var _startExpirationInterval = function () {
                expirationInterval = $interval(function () {
                    if (_tickets.length > 0) {
                        _nextTicket.timeLeft = _calculateTimeLeft(_tickets[0])
                        _nextTicket.printLeft = _formatDiff(_nextTicket.timeLeft);
                    } else {
                        _nextTicket.timeLeft = null;
                        _nextTicket.printLeft = null;
                    }
                }, 1000);
            };

            var _resetExpirations = function () {
                var updateTickets = {
                    ticketSeats: []
                };
                angular.forEach(_tickets, function (ticket) {
                    updateTickets.ticketSeats.push({
                        ticketSeatID: ticket.ticketSeatID,
                        pendingPurchaseConfirmation: ticket.confirmationNumber
                    });
                });


                SecureTicketSeats.resetExpirations(updateTickets).then(function (response) {
                    var deleteTickets = [];
                    for (var i = 0; i < _tickets.length; i++) {
                        var verifySeat = _.contains(response.data.updatedTicketSeats, _tickets[i].ticketSeatID);
                        if (verifySeat) {
                            _tickets[i].expirationTime = response.data.expirationDate;
                        } else {
                            deleteTickets.push(_tickets[i]);
                        }
                    }
                    if (deleteTickets.length > 0) {
                        angular.forEach(deleteTickets, function (delTicket) {
                            _tickets.splice(_tickets.indexOf(delTicket), 1);
                        });
                    }
                });
            };

            var _validateTickets = function () {
                SecureTicketSeats.validateTickets(_tickets).then(function (response) {
                    var deleteTickets = [];
                    for (var i = 0; i < _tickets.length; i++) {
                        var verifySeat = _.findWhere(response.data, { ticketSeatID: _tickets[i].ticketSeatID });
                        if (verifySeat) {
                            _tickets[i].totalPrice = verifySeat.totalPrice;
                        } else {
                            deleteTickets.push(_tickets[i]);
                        }
                    }
                    if (deleteTickets.length > 0) {
                        angular.forEach(deleteTickets, function (delTicket) {
                            _tickets.splice(_tickets.indexOf(delTicket), 1);
                        });
                    }
                });
            };

            return {
                getTickets: _getTickets,
                addTicket: _addTicket,
                deleteTicket: _deleteTicket,
                getNextExpiration: _getNextExpiration,
                resetExpirations: _resetExpirations,
                validateTickets: _validateTickets,
                clearTickets: _clearTickets,
                removeAllTickets: _removeAllTickets
            };
        }
    ]);
});