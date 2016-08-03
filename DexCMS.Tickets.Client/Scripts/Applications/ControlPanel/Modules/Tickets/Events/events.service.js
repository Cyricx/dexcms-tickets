define([
    'controlpanel-app'
], function (app) {
    app.service('Events', [
        '$resource',
        '$http',
        'DexCmsDateCleaner',
        function ($resource, $http, DateCleaner) {
            var baseUrl = '../api/events';
            
            var _forServer = function (item) {
                var adjustedItem = angular.copy(item);
                if (adjustedItem.disablePublicRegistration) {
                    adjustedItem.disablePublicRegistration = DateCleaner.preServerProcess(adjustedItem.disablePublicRegistration);
                }
                adjustedItem.eventStart = DateCleaner.preServerProcess(adjustedItem.eventStart);
                adjustedItem.eventEnd = DateCleaner.preServerProcess(adjustedItem.eventEnd);
                return adjustedItem;

            };
            
            var _fromServer = function (item) {
                if (item.disablePublicRegistration) {
                    item.disablePublicRegistration = DateCleaner.correctTimeZone(item.disablePublicRegistration);
                }
                item.eventStart = DateCleaner.correctTimeZone(item.eventStart);
                item.eventEnd = DateCleaner.correctTimeZone(item.eventEnd);
                return item;
            };
            
            return {
                //Create new record
                createItem: function (item) {
                    return $resource(baseUrl).save(_forServer(item)).$promise;
                },
                //Get Single Records
                getItem: function (id) {
                    return $http.get(baseUrl + "/" + id).then(function (response) {
                        response.data = _fromServer(response.data);
                        console.log(response);
                        return response;
                    });
                },
                //Get All 
                getList: function () {
                    return $resource(baseUrl).query().$promise.then(function (response) {
                        angular.forEach(response, function (data) {
                            data = _fromServer(data);
                        });
                        return response;
                    });
                },
                //Update the Record
                updateItem: function (id, item) {
                    var upItem = angular.copy(item);
                    if (upItem.pageContent.contentBlocks) {
                        delete upItem.pageContent.contentBlocks;
                    }
                    if (upItem.pageContent.pageContentImages) {
                        delete upItem.pageContent.pageContentImages;
                    }
                    
                    return $http.put(baseUrl + '/' + id, _forServer(item));
                },
                //Delete the Record
                deleteItem: function (id) {
                    return $http.delete(baseUrl + "/" + id);
                },
            };
        }
    ]);
});