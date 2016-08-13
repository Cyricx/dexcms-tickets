define([
    'controlpanel-app',
    '../tickets/tickets.service'
], function (app) {
    app.controller('ticketAssignmentsListCtrl', [
        '$scope',
        'Tickets',
        '$filter',
        'dexCMSControlPanelSettings',
        function ($scope, Tickets, $filter, dexcmsSettings) {
            $scope.title = "View Ticket Assignments";
            
            $scope.table = {
                columns: [
                    { property: 'eventID', title: 'ID' },
                    { property: 'pageContentHeading', title: 'Event' },
                    { property: 'eventSeriesName', title: 'Series' },
                    { property: 'availableCount', title: 'Available' },
                    { property: 'disabledCount', title: 'Disabled' },
                    { property: 'reservedCount', title: 'Reserved' },
                    { property: 'assignedCount', title: 'Assigned' },
                    { property: 'completeCount', title: 'Complete' },
                    {
                        property: '', title: '', disableSorting: true,
                        dataTemplate: dexcmsSettings.startingRoute + 'modules/tickets/ticketassignments/_ticketassignments.list.buttons.html'
                    }
                ],
                defaultSort: 'eventID',
                filePrefix: 'Ticket-Assignments'
            };

            Tickets.getList().then(function (data) {
                $scope.table.promiseData = data;
            });
        }
    ]);
});