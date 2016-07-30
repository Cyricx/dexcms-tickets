define([
    'controlpanel-app',
    '../tickets/tickets.service'
], function (app) {
    app.controller('ticketAssignmentsEditorCtrl', [
        '$scope',
        'Tickets',
        '$stateParams',
        '$state',
        function ($scope, Tickets, $stateParams, $state) {
            var id = $stateParams.id || null;

            if (id == null) {
                $state.go('ticketassignments');
            } else {

            }
        }
    ]);
});