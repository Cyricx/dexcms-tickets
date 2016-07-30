define([
    'secure-app',
    'angular'
], function (app, angular) {
    app.service('CheckIns', [
        '$resource',
        'DexCmsDateCleaner',
        '$http',
        function ($resource, DateCleaner, $http) {
            var baseUrl = '../../../api/checkins';

            var _forServer = function (item) {
                var adjustedItem = angular.copy(item);
                if (adjustedItem.arrivalTime) {
                    adjustedItem.arrivalTime = DateCleaner.preServerProcess(adjustedItem.arrivalTime);
                }
                return adjustedItem;
            };
            
            var _fromServer = function (item) {
                if (item.arrivalTime) {
                    item.arrivalTime = DateCleaner.correctTimeZone(item.arrivalTime);
                }
            };

            return {
                getList: function (segment) {
                    return $http.get(baseUrl + '/' + segment).then(function (response) {
                        angular.forEach(response.data, function (ticket) {
                            _fromServer(ticket);
                        });
                        return response;
                    });
                },
                updateItem: function (segment, id, item) {
                    return $http({
                        method: "put",
                        url: baseUrl + "/" + segment + "/" + id,
                        data: _forServer(item)
                    });
                },
            }
        }
    ])
});