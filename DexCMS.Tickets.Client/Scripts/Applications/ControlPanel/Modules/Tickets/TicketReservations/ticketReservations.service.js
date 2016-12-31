define([
   'controlpanel-app'
], function (app) {
    app.service('TicketReservations', [
        '$resource',
        '$http',
        function ($resource, $http) {
            var baseUrl = '../api/ticketreservations';

            return {
                //Create new record
                createItem: function (item) {
                    return $resource(baseUrl).save(item).$promise;
                },
                //Get Single Records
                getListByDiscount: function (id) {
                    return $http.get(baseUrl + "/" + id);
                },
            };
        }
    ]);
});