define([
   'controlpanel-app',
   '../events/events.navigation.service'
], function (app) {
    app.controller('ticketOptionsListCtrl', [
        '$scope',
        'TicketOptions',
        'DTOptionsBuilder',
        'DTColumnBuilder',
        '$compile',
        '$window',
        '$stateParams',
        '$state',
        'EventsNavigation',
        '$filter',
        function ($scope, TicketOptions, DTOptionsBuilder, DTColumnBuilder, $compile, $window, $stateParams, $state, EventsNavigation, $filter) {
            $scope.subtitle = "View Ticket Options";

            $scope.eventID = $stateParams.id;

            EventsNavigation.getNavigation($stateParams.id, function (data) {
                $scope.navigation = data;
                EventsNavigation.setActive($state.current.name);
            });

            $scope.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                return TicketOptions.getListByEvent($scope.eventID);
            }).withBootstrap().withOption('createdRow', createdRow);

            $scope.dtColumns = [
                DTColumnBuilder.newColumn('ticketOptionID').withTitle('ID'),
                DTColumnBuilder.newColumn('name').withTitle('Name'),
                DTColumnBuilder.newColumn('basePrice').withTitle('Price'),
                DTColumnBuilder.newColumn('cutoffDate').withTitle('Cutoff').renderWith(dateHtml),
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
                var buttons = '<a class="btn btn-warning" ui-sref="ticketoptions/:id/:toID({id: +' + data.eventID + ', toID: ' + data.ticketOptionID + '})">' +
                   '   <i class="fa fa-edit"></i>' +
                   '</a>';
                    buttons += ' <button class="btn btn-danger" ng-click="delete(' + data.ticketOptionID + ')">' +
                   '   <i class="fa fa-trash-o"></i>' +
                   '</button>';
                return buttons;
            }

            $scope.delete = function (id) {
                if (confirm('Are you sure?')) {
                    TicketOptions.deleteItem(id).then(function (response) {
                        $window.location.reload();
                    });
                }
            };
        }
    ]);
});