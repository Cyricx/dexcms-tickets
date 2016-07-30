﻿define([
   'controlpanel-app'
], function (app) {
    app.service('VenueScheduleLocations', [
        '$resource',
        '$http',
        function ($resource, $http) {
            var baseUrl = '../api/venueScheduleLocations';

            return {
                //Create new record
                createItem: function (item) {
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
                getListByVenue: function (id) {
                    return $resource(baseUrl + '/byvenue/' + id).query().$promise;
                },
                getListByEvent: function (id) {
                    return $resource(baseUrl + '/byevent/' + id).query().$promise;
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