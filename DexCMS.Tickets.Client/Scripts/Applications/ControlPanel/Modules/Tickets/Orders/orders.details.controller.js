define([
    'controlpanel-app'
], function (app) {
	app.controller('ordersDetailsCtrl', [
        '$scope',
        'Orders',
        '$stateParams',
        function ($scope, Orders, $stateParams) {

        	var id = $stateParams.id || null;

        	$scope.title = "View Order";

        	$scope.currentItem = {};

        	if (id != null) {
        		Orders.getItem(id).then(function (response) {
        			$scope.currentItem = response.data;
        			$scope.currentItem.enteredOn = new Date($scope.currentItem.enteredOn);
        		});
        	}
        }
	]);
});