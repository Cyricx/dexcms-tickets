define([
    'angular',
    'controlpanel-app',
], function (angular, module) {
    module.directive('dexcmsTicketPricesAreasList', [
        'dexCMSControlPanelSettings',
        function (dexcmsSettings) {
            return {
                restrict: "E",
                replace: true,
                scope: {
                    "ticketAreas":"="
                },
                templateUrl: dexcmsSettings.startingRoute + '/modules/ticketing/ticketcutoffs/_ticketpricesareas.list.html',
                controller: [
                    '$scope',
                    function ($scope) {

                        $scope.calculateGrid = function () {
                                var columnWidth = 1;

                                if ($scope.ticketAreas.length < 12) {
                                    columnWidth = Math.floor(12 / $scope.ticketAreas.length);
                                }
                                if (columnWidth < 6) {
                                    columnWidth = 6;
                                }
                                return "col-xs-" + (columnWidth * 2) + " col-md-" + columnWidth;
                        }
                }]
            }
        }
    ])
});