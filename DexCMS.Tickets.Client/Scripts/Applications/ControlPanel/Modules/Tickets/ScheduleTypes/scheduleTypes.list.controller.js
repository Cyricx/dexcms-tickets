define([
   'controlpanel-app'
], function (app) {
    app.controller('scheduleTypesListCtrl', [
        '$scope',
        'ScheduleTypes',
        'DTOptionsBuilder',
        'DTColumnBuilder',
        '$compile',
        '$window',
        function ($scope, ScheduleTypes, DTOptionsBuilder, DTColumnBuilder, $compile, $window) {
            $scope.title = "View Schedule Types";

            $scope.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                return ScheduleTypes.getList();
            }).withBootstrap().withOption('createdRow', createdRow);

            $scope.dtColumns = [
                DTColumnBuilder.newColumn('scheduleTypeID').withTitle('ID'),
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
                var buttons = '<a class="btn btn-warning" ui-sref="scheduletypes/:id({id: +' + data.scheduleTypeID + '})">' +
                   '   <i class="fa fa-edit"></i>' +
                   '</a>';
                if (data.scheduleItemCount == 0) {
                    buttons += ' <button class="btn btn-danger" ng-click="delete(' + data.scheduleTypeID + ')">' +
                   '   <i class="fa fa-trash-o"></i>' +
                   '</button>';
                } else {
                    buttons += ' <button class="btn btn-danger" ng-disabled="\'true\'">Currently in use</button';
                }

                return buttons;
            }

            $scope.delete = function (id) {
                if (confirm('Are you sure?')) {
                    ScheduleTypes.deleteItem(id).then(function (response) {
                        $window.location.reload();
                    });
                }
            };
        }
    ]);
});