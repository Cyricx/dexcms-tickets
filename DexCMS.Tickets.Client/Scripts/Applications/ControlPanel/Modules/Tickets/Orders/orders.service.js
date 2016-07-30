define([
	'controlpanel-app'
], function (app) {
	app.service('Orders', [
		'$resource',
		'$http',
        'DexCmsDateCleaner',
		function ($resource, $http, DateCleaner) {
			var baseUrl = '../api/orders';

			var _fromServer = function (item) {
				item.enteredOn = DateCleaner.correctTimeZone(item.enteredOn);
				if (item.payments && item.payments.length > 0) {
					angular.forEach(item.payments, function (payment) {
						if (payment.paidOn) {
							payment = DateCleaner.correctTimeZone(payment.paidOn);
						}
					});
				}
			};

			return {
				getItem: function (id) {
					return $http.get(baseUrl + '/' + id).then(function (response) {
						_fromServer(response.data);
						return response;
					});
				},
				getList: function () {
					return $resource(baseUrl).query().$promise.then(function (response) {
						angular.forEach(response, function (data) {
							_fromServer(data);
						});
						return response;
					});
				},
				deleteExpired: function () {
					return $http.delete(baseUrl);
				}
			};
		}
	]);
});