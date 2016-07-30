define([
   'controlpanel-app',
   '../events/events.navigation.service'
], function (app) {
    app.controller('scheduleItemsListCtrl', [
        '$scope',
        'ScheduleItems',
        'DTOptionsBuilder',
        'DTColumnBuilder',
        '$compile',
        '$window',
        '$stateParams',
        'EventsNavigation',
        '$state',
        '$filter',
        function ($scope, ScheduleItems, DTOptionsBuilder, DTColumnBuilder, $compile, $window, $stateParams, EventsNavigation, $state, $filter) {
            $scope.subtitle = "View Schedule";

            $scope.eventID = $stateParams.id;

            EventsNavigation.getNavigation($stateParams.id, function (data) {
                $scope.navigation = data;
                EventsNavigation.setActive($state.current.name);
            });

            $scope.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                return ScheduleItems.getListByEvent($scope.eventID);
            }).withBootstrap().withOption('createdRow', createdRow);

            $scope.dtColumns = [
                DTColumnBuilder.newColumn('scheduleItemID').withTitle('ID'),
                DTColumnBuilder.newColumn('title').withTitle('Title'),
                DTColumnBuilder.newColumn('startDate').withTitle('Start').renderWith(dateHtml),
                DTColumnBuilder.newColumn('endDate').withTitle('End').renderWith(dateHtml),
                DTColumnBuilder.newColumn('scheduleStatusName').withTitle('Status'),
                DTColumnBuilder.newColumn('scheduleTypeName').withTitle('Type'),
                DTColumnBuilder.newColumn(null).withTitle('').notSortable().renderWith(actionsHtml)
            ];

            function createdRow(row, data, dataIndex) {
                // Recompiling so we can bind Angular directive to the DT
                $compile(angular.element(row).contents())($scope);
            }

            function actionsHtml(data, type, full, meta) {
                var buttons = '<a class="btn btn-warning" ui-sref="scheduleitems/:id/:siID({id: +' + $scope.eventID + ', siID: ' + data.scheduleItemID + '})">' +
                   '   <i class="fa fa-edit"></i>' +
                   '</a>';
                    buttons += ' <button class="btn btn-danger" ng-click="delete(' + data.scheduleItemID + ')">' +
                   '   <i class="fa fa-trash-o"></i>' +
                   '</button>';
                return buttons;
            }

            function dateHtml(data, type, full, meta) {
                if (data != null) {
                    if (!full.isAllDay) {
                        return $filter('date')(data, "MM/dd/yyyy h:mm a");
                    } else {
                        return $filter('date')(data, "MM/dd/yyyy");
                    }
                    
                } else {
                    return null;
                }

            }
            $scope.delete = function (id) {
                if (confirm('Are you sure?')) {
                    ScheduleItems.deleteItem(id).then(function (response) {
                        $window.location.reload();
                    });
                }
            };
        }
    ]);
});