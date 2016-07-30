define([
    'controlpanel-app'
], function (app) {
    app.service('Tickets', [
        '$resource',
        '$http',
        function ($resource, $http) {
            var baseUrl = '../api/tickets';
            
            return {
                getList: function () {
                    return $resource(baseUrl).query().$promise;
                }
            };
        }
    ]);
});