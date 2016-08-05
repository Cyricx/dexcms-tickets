define([
    'controlpanel-app'
], function (app) {
    app.service('ReportingTickets', [
        '$resource',
        '$http',
        function ($resource, $http) {
            var baseUrl = '../api/reportingtickets';
            
            
            return {
                //Get All 
                getList: function (id) {
                    return $resource(baseUrl + "/" + id).query().$promise;
                },
            };
        }
    ]);
});