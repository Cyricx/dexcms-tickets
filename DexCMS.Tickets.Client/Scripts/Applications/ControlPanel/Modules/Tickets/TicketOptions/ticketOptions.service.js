define([
   'controlpanel-app'
], function (app) {
    app.service('TicketOptions', [
        '$resource',
        '$http',
        'DexCmsDateCleaner',
        function ($resource, $http, DateCleaner) {
            var baseUrl = '../api/ticketoptions';

            var _forServer = function (item) {
                var adjustedItem = angular.copy(item);
                adjustedItem.cutoffDate = DateCleaner.preServerProcess(adjustedItem.cutoffDate);
                return adjustedItem;
            };

            var _fromServer = function (item) {
                item.cutoffDate = DateCleaner.correctTimeZone(item.cutoffDate);
            }

            return {
                //Create new record
                createItem: function (item) {
                    return $resource(baseUrl).save(_forServer(item)).$promise;
                },
                //Get Single Records
                getItem: function (id) {
                    return $http.get(baseUrl + "/" + id).then(function (response) {
                        _fromServer(response.data);
                        return response;
                    });
                },
                //Get All 
                getList: function () {
                    return $resource(baseUrl).query().$promise.then(function (response) {
                        angular.forEach(response, function (data) {
                            _fromServer(data);
                        });
                        return response;
                    });
                },
                getListByEvent: function (id) {
                    return $resource(baseUrl + '/byevent/' + id).query().$promise.then(function (response) {
                        angular.forEach(response, function (data) {
                            _fromServer(data);
                        });
                        return response;
                    });
                },
                //Update the Record
                updateItem: function (id, item) {
                    return $http.put(baseUrl + "/" + id, _forServer(item));
                },
                //Delete the Record
                deleteItem: function (id) {
                    return $http.delete(baseUrl + "/" + id);
                },
            };
        }
    ]);
});