define([
    'angular',
    'controlpanel-app',
], function (angular, module) {
    module.directive('dexcmsReservationSectionsList', [
        'dexcmsSettings',
        function (dexcmsSettings) {
            return {
                restrict: "E",
                replace: true,
                scope: {
                    "ticketSections": "=",
                    "editor": "=",
                    "execOnChange": "&",
                    "config":"="
                },
                templateUrl: dexcmsSettings.startingRoute + '/modules/ticketing/ticketreservations/views/_reservationsections.list.html',
                controller: [
                    '$scope',
                    function ($scope) {
                        $scope.showAreas = function () {
                            $scope.editor.selectedID = null;
                            $scope.editor.selectedName = null;
                            $scope.editor.currentState = 'areas';
                        };

                        $scope.showRows = function (section) {
                            $scope.editor.currentState = 'rows';
                            $scope.editor.subSelectedID = section.ticketSectionID;
                            $scope.editor.subSelectedName = section.name;
                            $scope.selectedRows = section.ticketRows;
                            
                        };

                        $scope.calculateCounts = function () {
                            angular.forEach($scope.ticketSections, function (section) {
                                section.maxCapacity = 0;
                                section.assigned = 0;
                                section.pendingPurchase = 0;
                                section.discountAssigned = 0;
                                section.pendingDiscount = 0;
                                section.unclaimedReservations = 0;
                                section.available = 0;
                                section.unavailable = 0;
                                section.discountReservations = 0;
                                angular.forEach(section.ticketRows, function (row) {
                                    section.maxCapacity += row.maxCapacity;
                                    section.assigned += row.assigned;
                                    section.pendingPurchase += row.pendingPurchase;
                                    section.discountAssigned += row.discountAssigned;
                                    section.pendingDiscount += row.pendingDiscount;
                                    section.unclaimedReservations += row.unclaimedReservations;
                                    section.available += row.available;
                                    section.unavailable += row.unavailable;
                                    section.discountReservations += row.discountReservations;
                                });
                            });
                            $scope.execOnChange()();
                        };

                        $scope.calculateCounts();
                        

                    }
                ]
            }
        }
    ])
});