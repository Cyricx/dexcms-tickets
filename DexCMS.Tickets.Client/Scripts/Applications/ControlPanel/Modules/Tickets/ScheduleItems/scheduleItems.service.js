define([
   'controlpanel-app'
], function (app) {
    app.service('ScheduleItems', [
        '$resource',
        '$http',
        'DexCmsDateCleaner',
        function ($resource, $http, DateCleaner) {
            var baseUrl = '../api/scheduleItems';

            var _forServer = function (item) {
                var adjustedItem = angular.copy(item);

                adjustedItem.startDate = DateCleaner.preServerProcess(adjustedItem.startDate);

                if (adjustedItem.endDate) {
                    adjustedItem.endDate = DateCleaner.preServerProcess(adjustedItem.endDate);
                }

                return adjustedItem;
            };

            var _fromServer = function (item) {
                item.startDate = DateCleaner.correctTimeZone(item.startDate);
                if (item.endDate) {
                    item.endDate = DateCleaner.correctTimeZone(item.endDate);
                }
            };

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