define([
   'controlpanel-app'
], function (app) {
    app.controller('scheduleStatusesListCtrl', [
        '$scope',
        'ScheduleStatuses',
        'DTOptionsBuilder',
        'DTColumnBuilder',
        '$compile',
        '$window',
        function ($scope, ScheduleStatuses, DTOptionsBuilder, DTColumnBuilder, $compile, $window) {
            $scope.title = "View Schedule Statuses";

            $scope.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                return ScheduleStatuses.getList();
            }).withBootstrap().withOption('createdRow', createdRow);

            $scope.dtColumns = [
                DTColumnBuilder.newColumn('scheduleStatusID').withTitle('ID'),
                DTColumnBuilder.newColumn('name').withTitle('Name'),
                DTColumnBuilder.newColumn('cssClass').withTitle('CSS'),
                DTColumnBuilder.newColumn('isActive').withTitle('Active?'),
                DTColumnBuilder.newColumn(null).withTitle('').notSortable().renderWith(actionsHtml)
            ];

            function createdRow(row, data, dataIndex) {
                // Recompiling so we can bind Angular directive to the DT
                $compile(angular.element(row).contents())($scope);
            }

            function actionsHtml(data, type, full, meta) {
                var buttons = '<a class="btn btn-warning" ui-sref="schedulestatuses/:id({id: +' + data.scheduleStatusID + '})">' +
                   '   <i class="fa fa-edit"></i>' +
                   '</a>';

                    if (data.scheduleItemCount == 0) {
                        buttons += ' <button class="btn btn-danger" ng-click="delete(' + data.scheduleStatusID + ')">' +
                       '   <i class="fa fa-trash-o"></i>' +
                       '</button>';
                    } else {
                        buttons += ' <button class="btn btn-danger" ng-disabled="\'true\'">Currently in use</button';
                    }
                return buttons;
            }

            $scope.delete = function (id) {
                if (confirm('Are you sure?')) {
                    ScheduleStatuses.deleteItem(id).then(function (response) {
                        $window.location.reload();
                    });
                }
            };
        }
    ]);
});