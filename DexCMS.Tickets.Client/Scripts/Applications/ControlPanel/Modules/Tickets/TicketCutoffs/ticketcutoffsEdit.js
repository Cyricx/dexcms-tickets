define([
    'angular',
    'controlpanel-app'
], function (angular, module) {
    module.directive('ttcmsTicketCutoffsEdit', [
        'dexCMSControlPanelSettings',
        function (ttcmsSettings) {
            return {
                restrict: "E",
                replace: true,
                scope: {
                    "ticketCutoff": "="
                },
                templateUrl: ttcmsSettings.startingRoute + '/modules/ticketing/ticketcutoffs/_ticketcutoffs.edit.html',
                controller: [
                    '$scope',
                    'TicketCutoffs',
                    'ngToast',
                    function ($scope, TicketCutoffs, ngToast) {

                    $scope.formInputs = ttcmsSettings.startingRoute + '/modules/ticketing/ticketcutoffs/_ticketcutoffs.form.html';

                    var init = function () {
                        $scope.currentItem = angular.copy($scope.ticketCutoff);
                    };

                    $scope.cancel = function (cutoff) {
                        cutoff.isEditting = false;
                    }

                    $scope.update = function (editCutoff, cutoff) {
                        $scope.pending = true;
                        cutoff.name = editCutoff.name;
                        cutoff.onSellDate = editCutoff.onSellDate;
                        cutoff.cutoffDate = editCutoff.cutoffDate;
                        TicketCutoffs.updateItem(cutoff.ticketCutoffID, cutoff).then(
                            function (response) {
                                cutoff.isEditting = false;
                                $scope.pending = false;
                                init();
                            },
                            function (error) {
                                console.error(error.data.exceptionMessage);
                                ngToast.create({
                                    className: 'danger',
                                    content: '<h2>' + error.data.exceptionMessage + '</h2>'
                                });
                                $scope.pending = false;
                            }
                        );
                    };

                    //Start it!
                    init();
                }]
            }
        }
    ])
});