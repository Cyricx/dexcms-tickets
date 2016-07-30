define([
   'controlpanel-app'
], function (app) {
    app.controller('eventsListCtrl', [
        '$scope',
        'Events',
        'DTOptionsBuilder',
        'DTColumnBuilder',
        '$compile',
        '$window',
        '$filter',
        function ($scope, Events, DTOptionsBuilder, DTColumnBuilder, $compile, $window, $filter) {
            $scope.title = "View Events";

            $scope.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                return Events.getList();
            }).withBootstrap().withOption('createdRow', createdRow);

            $scope.dtColumns = [
                DTColumnBuilder.newColumn('eventID').withTitle('ID'),
                DTColumnBuilder.newColumn('pageContentHeading').withTitle('Event'),
                DTColumnBuilder.newColumn('eventSeriesName').withTitle('Series'),
                DTColumnBuilder.newColumn('venueName').withTitle('Venue'),
                DTColumnBuilder.newColumn('eventStart').withTitle('Start').renderWith(dateHtml),
                DTColumnBuilder.newColumn('eventEnd').withTitle('End').renderWith(dateHtml),
                DTColumnBuilder.newColumn('isPublic').withTitle('Public?'),
                DTColumnBuilder.newColumn(null).withTitle('').notSortable().renderWith(actionsHtml)
            ];

            function dateHtml(data, type, full, meta) {
                if (data != null) {
                    return $filter('date')(data, "MM/dd/yyyy h:mm a");
                } else {
                    return null;
                }
            }

            function createdRow(row, data, dataIndex) {
                // Recompiling so we can bind Angular directive to the DT
                $compile(angular.element(row).contents())($scope);
            }

            function actionsHtml(data, type, full, meta) {
                var buttons = '<a class="btn btn-warning" ui-sref="eventdashboards/:id({id: +' + data.eventID + '})">' +
                   '   <i class="fa fa-search"></i>' +
                   '</a>';
                //! TODO: Handle cascading deletes and determine a stop point
                   // buttons += ' <button class="btn btn-danger" ng-click="delete(' + data.eventID + ')">' +
                   //'   <i class="fa fa-trash-o"></i>' +
                   //'</button>';
                return buttons;
            }

            $scope.delete = function (id) {
                if (confirm('Are you sure?')) {
                    Events.deleteItem(id).then(function (response) {
                        $window.location.reload();
                    });
                }
            };
        }
    ]);
});