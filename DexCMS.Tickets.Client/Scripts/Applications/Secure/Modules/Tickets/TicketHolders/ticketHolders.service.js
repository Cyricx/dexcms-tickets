define([
    'secure-app'
], function (app) {
    app.service('TicketHolders', [
        '$resource',
        '$http',
        function ($resource, $http) {
            var baseUrl = '../api/ticketholders';

            return {
                getItem: function (id) {
                    return $http.get(baseUrl + '/' + id);
                },
                updateItem: function (id, item) {
                    return $http.put(baseUrl + '/' + id, item);
                }
            };
        }
    ]);
});