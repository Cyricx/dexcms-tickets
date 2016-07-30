define([
   'controlpanel-app'
], function (app) {
    app.service('Events', [
        '$resource',
        '$http',
        function ($resource, $http) {
            var baseUrl = '../api/events';

            return {
                //Create new record
                createItem: function (item) {
                    //return $resource(baseUrl).save(item).$promise;
                    var request = $http({
                        method: "post",
                        url: baseUrl,
                        data: item
                    });
                    return request;
                },
                //Get Single Records
                getItem: function (id) {
                    return $http.get(baseUrl + "/" + id);
                },
                //Get All 
                getList: function () {
                    return $resource(baseUrl).query().$promise;
                },
                //Update the Record
                updateItem: function (id, item) {
                    //return $http.put(baseUrl + "/" + id, item);
                    var upItem = angular.copy(item);
                    if (upItem.pageContent.contentBlocks) {
                        delete upItem.pageContent.contentBlocks;
                    }
                    if (upItem.pageContent.pageContentImages) {
                        delete upItem.pageContent.pageContentImages;
                    }
                    var request = $http({
                        method: "put",
                        url: baseUrl + "/" + id,
                        data: upItem
                    });
                    return request;
                },
                //Delete the Record
                deleteItem: function (id) {
                    return $http.delete(baseUrl + "/" + id);
                },
            };
        }
    ]);
});