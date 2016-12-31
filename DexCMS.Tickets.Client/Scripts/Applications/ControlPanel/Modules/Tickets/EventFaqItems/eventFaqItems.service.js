define([
   'controlpanel-app'
], function (app) {
    app.service('EventFaqItems', [
        '$resource',
        '$http',
        '$rootScope',
        function ($resource, $http, $rootScope) {
            var baseUrl = '../api/eventfaqitems';

            return {
                //Create new record
                createItem: function (item) {
                    item.lastUpdatedBy = $rootScope.cpUser.username;
                    item.lastUpdated = new Date();
                    return $resource(baseUrl).save(item).$promise;
                },
                //Get Single Records
                getItem: function (id) {
                    return $http.get(baseUrl + "/" + id);
                },
                //Get All 
                getList: function () {
                    return $resource(baseUrl).query().$promise;
                },
                getListByFaqCategory: function (id) {
                    return $resource(baseUrl + '/byfaqcategory/' + id).query().$promise;
                },
                //Update the Record
                updateItem: function (id, item) {
                    return $http.put(baseUrl + "/" + id, item);
                },
                //Delete the Record
                deleteItem: function (id) {
                    return $http.delete(baseUrl + "/" + id);
                },
            };
        }
    ]);
});