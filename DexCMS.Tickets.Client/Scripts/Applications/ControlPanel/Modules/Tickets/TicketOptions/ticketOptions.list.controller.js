define([
   'controlpanel-app',
   '../events/events.navigation.service'
], function (app) {
    app.controller('ticketOptionsListCtrl', [
        '$scope',
        'TicketOptions',
        '$window',
        '$stateParams',
        '$state',
        'EventsNavigation',
        '$filter',
        'dexCMSControlPanelSettings',
        function ($scope, TicketOptions, $window, $stateParams, $state, EventsNavigation, $filter, dexcmsSettings) {
            $scope.subtitle = "View Ticket Options";

            $scope.eventID = $stateParams.id;

            var _renderDate = function (data, item) {
                if (data != null) {
                    return $filter('date')(data, "MM/dd/yyyy h:mm a");
                } else {
                    return null;
                }
            };

            $scope.table = {
                columns: [
                    { property: 'ticketOptionID', title: 'ID' },
                    { property: 'name', title: 'Name' },
                    { property: 'basePrice', title: 'Price' },
                    { property: 'cutoffDate', title: 'Cutoff', dataFunction: _renderDate },
                    {
                        property: '', title: '', disableSorting: true,
                        dataTemplate: dexcmsSettings.startingRoute + 'modules/tickets/ticketoptions/_ticketoptions.list.buttons.html'
                    }
                ],
                defaultSort: 'ticketOptionID',
                functions: {
                    remove: function (id) {
                        if (confirm('Are you sure?')) {
                            TicketOptions.deleteItem(id).then(function (response) {
                                $window.location.reload();
                            });
                        }
                    }
                }
            };

            TicketOptions.getListByEvent($scope.eventID).then(function (data) {
                $scope.table.promiseData = data;
            });

            EventsNavigation.getNavigation($stateParams.id, function (data) {
                $scope.navigation = data;
                $scope.table.filePrefix = data.title.replace('Manage ', '').replace(/ /g, '-') + '-Ticket-Options';
                EventsNavigation.setActive($state.current.name);
            });
        }
    ]);
});