define([
	'controlpanel-app'
], function (app) {
	app.controller('ordersListCtrl', [
        '$scope',
        'Orders',
        'DTOptionsBuilder',
        'DTColumnBuilder',
        '$compile',
        '$filter',
        '$window',
        function ($scope, Orders, DTOptionsBuilder, DTColumnBuilder, $compile, $filter, $window) {
        	$scope.title = "View Orders";

        	$scope.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
        		return Orders.getList();
        	}).withBootstrap().withOption('createdRow', createdRow);

        	$scope.dtColumns = [
                DTColumnBuilder.newColumn('orderID').withTitle('ID'),
                DTColumnBuilder.newColumn('userName').withTitle('Name'),
                DTColumnBuilder.newColumn('orderStatusName').withTitle('Status'),
                DTColumnBuilder.newColumn('enteredOn').withTitle('Entered').renderWith(dateHtml),
                DTColumnBuilder.newColumn('orderTotal').withTitle('Total').renderWith(currencyHtml),
                DTColumnBuilder.newColumn('ticketCount').withTitle('Tickets'),
                DTColumnBuilder.newColumn(null).withTitle('').notSortable().renderWith(actionsHtml)
        	];

        	function createdRow(row, data, dataIndex) {
        		// Recompiling so we can bind Angular directive to the DT
        		$compile(angular.element(row).contents())($scope);
        	}

        	function actionsHtml(data, type, full, meta) {
        		var buttons = '<a class="btn btn-success" ui-sref="orders/details/:id({id: +' + data.orderID + '})">' +
                   '   <i class="fa fa-search"></i>' +
                   '</a>';
        		return buttons;
        	}

        	function dateHtml(data, type, full, meta) {
        		return new Date(data).toLocaleString();
        	}

        	function currencyHtml(data, type, full, meta) {
        	    return $filter('currency')(data);
        	}

        	$scope.cleanOrders = function () {
        		$scope.processing = true;
        		Orders.deleteExpired().then(function (response) {
        			$scope.processing = false;
        	        $window.location.reload();
        	    });
        	};
        }
	]);
});