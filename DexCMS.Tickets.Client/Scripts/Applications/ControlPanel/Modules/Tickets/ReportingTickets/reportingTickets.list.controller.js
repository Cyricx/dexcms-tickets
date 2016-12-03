define([
    'controlpanel-app',
    '../events/events.navigation.service'
], function (app) {
    app.controller('reportingTicketsListCtrl', [
        '$scope',
        'ReportingTickets',
        '$window',
        '$stateParams',
        'EventsNavigation',
        '$state',
        '$filter',
        'dexCMSControlPanelSettings',
        function ($scope, ReportingTickets, $window, $stateParams, EventsNavigation, $state, $filter, dexcmsSettings) {
            $scope.subtitle = "Reporting - Tickets";
            
            $scope.eventID = $stateParams.id;



            var _getTemplate = function (tempType) {
                return dexcmsSettings.startingRoute + 'modules/tickets/reportingTickets/_reportingTickets.list.' + tempType + '.html'
            };

            var _getCurrencyValue = function (value, item) {
                return $filter('currency')(value);
            };
            
            var _renderDate = function (value, item) {
                if (value != null) {
                    return $filter('date')(value, "MM/dd/yyyy h:mm a");
                } else {
                    return null;
                }
            };

            $scope.table = {
                columns: [
                    { property: 'orderID', title: 'Order' },
                    { property: 'ticketID', title: 'Ticket' },
                    { property: 'userName', title: 'User', dataTemplate: _getTemplate('user') },
                    { property: 'firstName', title: 'Holder', dataTemplate: _getTemplate('holder') },
                    { property: 'ticketAreaName', title: 'Designation', dataTemplate: _getTemplate('designation') },
                    { property: 'ticketDiscountName', title: 'Discount', dataTemplate: _getTemplate('discount') },
                    { property: 'options', title: 'Options', dataTemplate: _getTemplate('options') },
                    { property: 'arrivalTime', title: 'Arrival', dataFunction: _renderDate },
                    { property: 'ticketTotalPrice', title: 'Ticket Price', dataFunction: _getCurrencyValue },
                    { property: 'orderTotal', title: 'Order Total', dataFunction: _getCurrencyValue },
                ],
                defaultSort: 'orderID',
                filePrefix: 'Reporting-Tickets'
            };

            ReportingTickets.getList($scope.eventID).then(function (data) {
                $scope.table.promiseData = data;
            });

            EventsNavigation.getNavigation($stateParams.id, function (data) {
                $scope.navigation = data;
                $scope.table.filePrefix = data.title.replace('Manage ', '').replace(/ /g, '-') + '-Reporting-Tickets';
                EventsNavigation.setActive($state.current.name);
            });
        }
    ]);
});