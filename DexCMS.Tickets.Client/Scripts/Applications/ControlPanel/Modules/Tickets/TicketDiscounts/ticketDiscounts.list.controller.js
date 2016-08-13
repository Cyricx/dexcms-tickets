define([
   'controlpanel-app',
   '../events/events.navigation.service'
], function (app) {
    app.controller('ticketDiscountsListCtrl', [
        '$scope',
        'TicketDiscounts',
        '$window',
        '$stateParams',
        '$state',
        'EventsNavigation',
        '$filter',
        'dexCMSControlPanelSettings',
        function ($scope, TicketDiscounts, $window, $stateParams, $state, EventsNavigation, $filter, dexcmsSettings) {
            $scope.subtitle = "View Ticket Discounts";

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
                    { property: 'ticketDiscountID', title: 'ID' },
                    { property: 'name', title: 'Name' },
                    { property: 'code', title: 'Code' },
                    { property: 'cutoffDate', title: 'Cutoff', dataFunction: _renderDate },
                    { property: 'isActive', title: 'Active?' },
                    {
                        property: '', title: '', disableSorting: true,
                        dataTemplate: dexcmsSettings.startingRoute + 'modules/tickets/ticketdiscounts/_ticketdiscounts.list.buttons.html'
                    }
                ],
                defaultSort: 'ticketDiscountID',
                functions: {
                    remove: function (id) {
                        if (confirm('Are you sure?')) {
                            TicketDiscounts.deleteItem(id).then(function (response) {
                                $window.location.reload();
                            });
                        }
                    }
                }
            };

            TicketDiscounts.getListByEvent($scope.eventID).then(function (data) {
                $scope.table.promiseData = data;
            });

            EventsNavigation.getNavigation($stateParams.id, function (data) {
                $scope.navigation = data;
                $scope.table.filePrefix = data.title.replace('Manage ', '').replace(/ /g, '-') + '-Ticket-Discounts';
                EventsNavigation.setActive($state.current.name);
            });
        }
    ]);
});