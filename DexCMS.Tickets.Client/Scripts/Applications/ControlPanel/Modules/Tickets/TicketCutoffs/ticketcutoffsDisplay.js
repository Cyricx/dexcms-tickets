define([
    'angular',
    'controlpanel-app'
], function (angular, module) {
    module.directive('dexcmsTicketCutoffsDisplay', [
        'dexCMSControlPanelSettings',
        function (dexcmsSettings) {
            return {
                restrict: "E",
                replace: true,
                scope: {
                    "ticketCutoff": "="
                },
                templateUrl: dexcmsSettings.startingRoute + '/modules/ticketing/ticketcutoffs/_ticketcutoffs.display.html',
                controller: ['$scope', 'TicketCutoffs', function ($scope, TicketCutoffs) {
                    //! Added temporarily
                    $scope.ticketCutoff.showContents = true;


                    $scope.delete = function (cutoff) {
                        if (confirm("Are you sure?")) {
                            TicketCutoffs.deleteItem(cutoff.ticketCutoffID).then(function () {
                                $scope.$emit('ticketCutoffDeleted', cutoff);
                            })
                        }
                    }
                }]
            }
        }
    ])
});