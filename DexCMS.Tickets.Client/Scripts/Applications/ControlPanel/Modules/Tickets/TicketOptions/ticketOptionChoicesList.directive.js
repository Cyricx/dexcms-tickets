define([
    'controlpanel-app',
    'angular',
   '../ticketoptionchoices/ticketoptionchoices.service',
], function (module, angular) {
    module.directive('ttcmsTicketOptionChoicesList', [
        'dexCMSControlPanelSettings',
        function (ttcmsSettings) {

            return {
                restrict: "E",
                replace: true,
                scope: {
                    "ticketOption": "="
                },
                templateUrl: ttcmsSettings.startingRoute + './modules/ticketing/ticketoptions/_ticketoptionchoiceslist.html',
                controller: [
                    '$scope',
                    'TicketOptionChoices',
                    function ($scope, TicketOptionChoices) {
                        TicketOptionChoices.getListByOption($scope.ticketOption.ticketOptionID).then(function (data) {
                            $scope.ticketOptionChoices = data;
                        });

                        $scope.delete = function (choice) {
                            if (confirm('Are you sure?')) {
                                TicketOptionChoices.deleteItem(choice.ticketOptionChoiceID).then(function (response) {
                                    $scope.ticketOptionChoices.splice($scope.ticketOptionChoices.indexOf(choice), 1);
                                });
                            }
                        }
                    }
                ]
            };
        }
    ]);
});