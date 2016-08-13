define([
	'controlpanel-app'
], function (app) {
	app.controller('ordersListCtrl', [
        '$scope',
        'Orders',
        '$filter',
        '$window',
        'dexCMSControlPanelSettings',
        function ($scope, Orders, $filter, $window, dexcmsSettings) {
        	$scope.title = "View Orders";

        	var _renderDate = function (value, item) {
        	    if (value != null) {
        	        return $filter('date')(value, "MM/dd/yyyy h:mm a");
        	    } else {
        	        return null;
        	    }
        	};

        	$scope.table = {
        	    columns: [
                    { property: 'orderID', title: 'ID' },
                    { property: 'userName', title: 'Name' },
                    { property: 'orderStatusName', title: 'Status' },
                    { property: 'enteredOn', title: 'Entered', dataFunction: _renderDate },
                    {
                        property: 'orderTotal', title: 'Total', dataFunction: function (data, item) {
                            return $filter('currency')(data);
                        }
                    },
                    { property: 'ticketCount', title: 'Tickets' },
                    {
                        property: '', title: '', disableSorting: true,
                        dataTemplate: dexcmsSettings.startingRoute + 'modules/tickets/orders/_orders.list.buttons.html'
                    }
        	    ],
        	    defaultSort: 'orderID',
                filePrefix: 'Orders'
        	};

        	Orders.getList().then(function (data) {
        	    $scope.table.promiseData = data;
        	});

            //ToDo: possible bugs with server side implementation
        	//$scope.cleanOrders = function () {
        	//	$scope.processing = true;
        	//	Orders.deleteExpired().then(function (response) {
        	//		$scope.processing = false;
        	//        $window.location.reload();
        	//    });
        	//};
        }
	]);
});