define([
   'controlpanel-app'
], function (app) {
    app.service('TicketAreas', [
        '$resource',
        '$http',
        function ($resource, $http) {
            var baseUrl = '../api/ticketareas';

            return {
                getListByEvent: function (id) {
                    return $resource(baseUrl + "/byevent/" + id).query().$promise;
                },
            };
        }
    ]);
});