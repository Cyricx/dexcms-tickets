define([
    'secure-app'
], function (app) {

    app.controller('completeDetailsCtrl', [
        '$scope',
        'RetrieveContents',
        '$stateParams',
        'SecureOrders',
        '$state',
        'Tickets',
        function ($scope, RetrieveContents, $stateParams, SecureOrders, $state, Tickets) {

            RetrieveContents.getContent('securecomplete').then(function (response) {
                $scope.content = response.data;
            });

            SecureOrders.getItem($stateParams.id).then(function (response) {
                $scope.order = response.data;
                Tickets.clearTickets(response.data.tickets);
            });
           
        }
    ])
});