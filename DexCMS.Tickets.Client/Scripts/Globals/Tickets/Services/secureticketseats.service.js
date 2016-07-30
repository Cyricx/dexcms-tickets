define([
    '../tickets'
], function (module) {
    module.service('SecureTicketSeats', ['$http', function ($http) {

        return {
            resetExpirations: function (tickets) {
                return $http.put('../../../api/secureticketseats', tickets);
            },
            unreserveTicket: function (ticketID) {
                return $http.delete('../../../api/secureticketseats/' + ticketID);
            },
            validateTickets: function (tickets) {
                return $http.post('../../../api/secureticketseats', tickets);
            }
        }
    }]);
});