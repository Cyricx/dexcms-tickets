define([
    '../tickets',
    'angular'
], function (module, angular) {
    module.service('CashierOrders', [
        '$resource',
        'DexCmsDateCleaner',
        '$http',
        function ($resource, DateCleaner, $http) {

            var baseUrl = '../../../api/cashierorders';

            var _forServer = function (item) {
                var adjustedItem = angular.copy(item);

                adjustedItem.enteredOn = DateCleaner.preServerProcess(adjustedItem.enteredOn);

                return adjustedItem;
            };

            var _fromServer = function (item) {
                item.enteredOn = DateCleaner.correctTimeZone(item.enteredOn);
            };

        return {
                createOrder: function (tickets) {
                    var order = {
                        cashierOrderType: 0,
                        tickets: tickets
                    };
                    return $resource(baseUrl).save(order).$promise;
                },
                createInvoice: function (tickets, email) {
                    var order = {
                        cashierOrderType: 1,
                        tickets: tickets,
                        invoiceEmail: email
                    };
                    return $resource(baseUrl).save(order).$promise;
                },
            getItem: function (id) {
                return $http.get(baseUrl + "/" + id).then(function (response) {
                    _fromServer(response.data);
                    return response;
                });
            },
            //getList: function () {
            //    return $http.get(baseUrl).then(function (response) {
            //        angular.forEach(response.data, function (order) {
            //            _fromServer(order);
            //        });
            //        return response;
            //    });
            //},
            updateItem: function (id, item) {
                return $http({
                    method: "put",
                    url: baseUrl + "/" + id,
                    data: _forServer(item)
                });
            },
            deleteItem: function (id, email) {
                return $http.delete(baseUrl + "/" + id, { params: { email: email } });
            }
        }
    }]);
});