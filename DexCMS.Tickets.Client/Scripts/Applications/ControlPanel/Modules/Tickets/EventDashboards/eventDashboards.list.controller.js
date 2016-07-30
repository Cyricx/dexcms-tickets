define([
   'controlpanel-app',
   '../events/events.navigation.service'
], function (app) {
    app.controller('eventDashboardsListCtrl', [
        '$scope',
        'EventDashboards',
        'DTOptionsBuilder',
        'DTColumnBuilder',
        '$compile',
        '$window',
        '$stateParams',
        '$state',
        'EventsNavigation',
        function ($scope, EventDashboards, DTOptionsBuilder, DTColumnBuilder, $compile, $window, $stateParams, $state, EventsNavigation) {
            $scope.title = "View EventDashboards";
            EventsNavigation.getNavigation($stateParams.id, function (data) {
                $scope.navigation = data;
                EventsNavigation.setActive($state.current.name);
            });
            //$scope.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
            //    return EventDashboards.getList();
            //}).withBootstrap().withOption('createdRow', createdRow);

            //$scope.dtColumns = [
            //    DTColumnBuilder.newColumn('eventDashboardID').withTitle('ID'),

            //    DTColumnBuilder.newColumn(null).withTitle('').notSortable().renderWith(actionsHtml)
            //];

            //function createdRow(row, data, dataIndex) {
            //    // Recompiling so we can bind Angular directive to the DT
            //    $compile(angular.element(row).contents())($scope);
            //}

            //function actionsHtml(data, type, full, meta) {
            //    var buttons = '<a class="btn btn-warning" ui-sref="eventdashboards/:id({id: +' + data.eventDashboardsID + '})">' +
            //       '   <i class="fa fa-edit"></i>' +
            //       '</a>';
            //        buttons += ' <button class="btn btn-danger" ng-click="delete(' + data.eventDashboardsID + ')">' +
            //       '   <i class="fa fa-trash-o"></i>' +
            //       '</button>';
            //    return buttons;
            //}

            //$scope.delete = function (id) {
            //    if (confirm('Are you sure?')) {
            //        EventDashboards.deleteItem(id).then(function (response) {
            //            $window.location.reload();
            //        });
            //    }
            //};
        }
    ]);
});