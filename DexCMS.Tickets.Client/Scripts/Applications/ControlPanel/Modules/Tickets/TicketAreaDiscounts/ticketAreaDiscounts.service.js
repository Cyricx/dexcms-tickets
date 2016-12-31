define([
   'controlpanel-app'
], function (app) {
    app.service('TicketAreaDiscounts', [
        '$resource',
        '$http',
        function ($resource, $http) {
            var baseUrl = '../api/ticketareadiscounts';

            return {
                //Create new record
                createItem: function (item) {
                    return $resource(baseUrl).save(item).$promise;
                },
                //Get Single Records
                getItem: function (id, secondID) {
                    return $http.get(baseUrl + "/" + id + "/" + secondID);
                },
                //Get All 
                getList: function () {
                    return $resource(baseUrl).query().$promise;
                },
                getListByDiscount: function (id) {
                    return $resource(baseUrl+ '/bydiscount/' + id).query().$promise;
                },
                //Update the Record
                updateItem: function (id, secondID, item) {
                    return $http.put(baseUrl + "/" + id + "/" + secondID, item);
                },
                //Delete the Record
                deleteItem: function (id, secondID) {
                    return $http.delete(baseUrl + "/" + id + "/" + secondID);
                },
            };
        }
    ]);
});