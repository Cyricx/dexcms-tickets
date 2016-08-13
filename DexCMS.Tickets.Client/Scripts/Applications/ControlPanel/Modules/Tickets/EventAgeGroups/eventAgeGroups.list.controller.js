define([
   'controlpanel-app',
   '../events/events.navigation.service'
], function (app) {
    app.controller('eventAgeGroupsListCtrl', [
        '$scope',
        'EventAgeGroups',
        '$window',
        '$stateParams',
        'EventsNavigation',
        '$state',
        'dexCMSControlPanelSettings',
        function ($scope, EventAgeGroups, $window, $stateParams, EventsNavigation, $state, dexcmsSettings) {
            $scope.subtitle = "View Age Groups";
            
            $scope.eventID = $stateParams.id;

            $scope.table = {
                columns: [
                    { property: 'eventAgeGroupID', title: 'ID' },
                    { property: 'name', title: 'Name' },
                    { property: 'minimumAge', title: 'Minimum Age' },
                    { property: 'maximumAge', title: 'Maximum Age' },
                    {
                        property: '', title: '', disableSorting: true,
                        dataTemplate: dexcmsSettings.startingRoute + 'modules/tickets/eventagegroups/_eventagegroups.list.buttons.html'
                    }
                ],
                defaultSort: 'eventAgeGroupID',
                functions: {
                    remove: function (id) {
                        if (confirm('Are you sure?')) {
                            EventAgeGroups.deleteItem(id).then(function (response) {
                                $window.location.reload();
                            });
                        }
                    }
                }
            };

            EventsNavigation.getNavigation($stateParams.id, function (data) {
                $scope.navigation = data;
                $scope.table.filePrefix = data.title.replace('Manage ','').replace(/ /g, '-') + '-Event-Age-Groups';
                EventsNavigation.setActive($state.current.name);
            });


            EventAgeGroups.getListByEvent($scope.eventID).then(function (data) {
                $scope.table.promiseData = data;
            });

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
        }
    ]);
});