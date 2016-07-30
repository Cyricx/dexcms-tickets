define([
    '../tickets'
], function (module) {
    module.service('Registrations', ['$http', function ($http) {
        var EventInfo = {
            segment: ''
        };

        var setSegment = function (seg) {
            EventInfo.segment = seg;
        };

        return {
            setSegment: setSegment,
            checkEventDisabled: function (segment) {
                return $http.get('../../../api/registration/' + segment + '/checkeventdisabled');
            },
            getAreas: function () {
                return $http.get('../../../api/registration/' + EventInfo.segment + '/areas');
            },
            getAreasByDiscount: function (id) {
                return $http.get('../../../api/registration/' + EventInfo.segment + '/areas/' + id);
            },
            getAgeGroups: function (id) {
                return $http.get('../../../api/registration/' + EventInfo.segment + '/agegroups/' + id);
            },
            getAgeGroupsByDiscount: function (id, secondKey) {
                return $http.get('../../../api/registration/' + EventInfo.segment + '/agegroups/' + id + '/' + secondKey);
            },
            getPrices: function () {
                return $http.get('../../../api/registration/' + EventInfo.segment + '/registrationprices');
            },
            getPricesByDiscount: function (id) {
                return $http.get('../../../api/registration/' + EventInfo.segment + '/registrationprices/' + id);
            },
            postAddTickets: function (addRequest) {
                return $http.post('../../../api/registration/' + EventInfo.segment + '/addtickets', addRequest);
            },
            unreserveTicket: function (ticketID) {
                return $http.delete('../../../api/registration/' + EventInfo.segment + '/unreserveTicket/' + ticketID);
            },
            resetExpirations: function (tickets) {
                return $http.put('../../../api/registration/' + EventInfo.segment + '/resetexpirations', tickets);
            },
            retrieveDiscounts: function () {
                return $http.get('../../../api/registration/' + EventInfo.segment + '/retrievediscounts');
            },
            ticketOptions: function (id) {
                return $http.get('../../../api/registration/' + EventInfo.segment + '/ticketoptions/' + id);
            },
            ticketOptionsByDiscount: function (id, secondKey) {
                return $http.get('../../../api/registration/' + EventInfo.segment + '/ticketoptions/' + id + '/' + secondKey);
            },
            verifyDiscount: function (discount) {
                return $http.post('../../../api/registration/' + EventInfo.segment + '/verifydiscount', discount);
            }
        }
    }]);
});