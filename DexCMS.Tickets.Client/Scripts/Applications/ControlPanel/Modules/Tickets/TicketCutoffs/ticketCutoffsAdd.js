define([
    'angular',
    'controlpanel-app'
], function (angular, module) {
    module.directive('dexcmsTicketCutoffsAdd', [
        'dexCMSControlPanelSettings',
        function (dexcmsSettings) {
            return {
                restrict: "E",
                replace: true,
                scope: {
                    "ticketCutoff": "="
                },
                templateUrl: dexcmsSettings.startingRoute + '/modules/ticketing/ticketcutoffs/_ticketcutoffs.add.html',
                controller: [
                    '$scope',
                    'TicketCutoffs',
                    '$stateParams',
                    'ngToast',
                    function ($scope, TicketCutoffs, $stateParams, ngToast) {

                    $scope.formInputs = dexcmsSettings.startingRoute + '/modules/ticketing/ticketcutoffs/_ticketcutoffs.form.html';

                    var eventID = $stateParams.id;

                    $scope.isAdding = false;

                    var init = function () {
                        $scope.currentItem = {};
                    };

                    $scope.cancel = function () {
                        $scope.isAdding = false;
                    }

                    $scope.save = function (item) {
                        $scope.pending = true;
                        item.eventID = eventID;
                        TicketCutoffs.createItem(item).then(
                            function (response) {
                                $scope.$emit('ticketCutoffAdded', response);
                                $scope.pending = false;
                                $scope.isAdding = false;
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