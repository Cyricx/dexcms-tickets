define([
   'controlpanel-app',
   '../events/events.navigation.service'
], function (app) {
    app.controller('eventAgeGroupsListCtrl', [
        '$scope',
        'EventAgeGroups',
        'DTOptionsBuilder',
        'DTColumnBuilder',
        '$compile',
        '$window',
        '$stateParams',
        'EventsNavigation',
        '$state',
        function ($scope, EventAgeGroups, DTOptionsBuilder, DTColumnBuilder, $compile, $window, $stateParams, EventsNavigation, $state) {
            $scope.subtitle = "View Age Groups";
            
            $scope.eventID = $stateParams.id;

            EventsNavigation.getNavigation($stateParams.id, function (data) {
                $scope.navigation = data;
                EventsNavigation.setActive($state.current.name);
            });

            $scope.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                return EventAgeGroups.getListByEvent($scope.eventID);
            }).withBootstrap().withOption('createdRow', createdRow);

            $scope.dtColumns = [
                DTColumnBuilder.newColumn('eventAgeGroupID').withTitle('ID'),
                DTColumnBuilder.newColumn('name').withTitle('Name'),
                DTColumnBuilder.newColumn('minimumAge').withTitle('Minimum Age'),
                DTColumnBuilder.newColumn('maximumAge').withTitle('Maximum Age'),
                DTColumnBuilder.newColumn(null).withTitle('').notSortable().renderWith(actionsHtml)
            ];

            function createdRow(row, data, dataIndex) {
                // Recompiling so we can bind Angular directive to the DT
                $compile(angular.element(row).contents())($scope);
            }

            function actionsHtml(data, type, full, meta) {
                var buttons = '<a class="btn btn-warning" ui-sref="eventagegroups/:id/:eageID({id: +' + data.eventID + ', eageID: ' + data.eventAgeGroupID + '})">' +
                   '   <i class="fa fa-edit"></i>' +
                   '</a>';
                if (data.ticketPricesCount > 0 ||  data.ticketOptionsCount > 0) {
                    buttons += ' <button class="btn btn-danger" ng-disabled="\'true\'">Currently in use</button';

                } else {
                    buttons += ' <button class="btn btn-danger" ng-click="delete(' + data.eventAgeGroupID + ')">' +
'   <i class="fa fa-trash-o"></i>' +
'</button>';
                }

                return buttons;
            }

            $scope.delete = function (id) {
                if (confirm('Are you sure?')) {
                    EventAgeGroups.deleteItem(id).then(function (response) {
                        $window.location.reload();
                    });
                }
            };
        }
    ]);
});