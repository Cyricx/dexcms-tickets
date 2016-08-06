define([
    'angular',
    'controlpanel-app',
], function (angular, module) {
    module.directive('dexcmsReservationRowsList', [
        'dexCMSControlPanelSettings',
        function (dexcmsSettings) {
            return {
                restrict: "E",
                replace: true,
                scope: {
                    "ticketRows": "=",
                    "editor": "=",
                    "execOnChange": "&",
                    "config":"="
                },
                templateUrl: dexcmsSettings.startingRoute + '/modules/tickets/ticketreservations/_reservationrows.list.html',
                controller: [
                    '$scope',
                    'TicketReservations',
                    function ($scope, TicketReservations) {
                        $scope.showAreas = function () {
                            $scope.editor.currentState = 'areas';
                            $scope.editor.subSelectedID = null;
                            $scope.editor.subSelectedName = null;
                            $scope.editor.selectedID = null;
                            $scope.editor.selectedName = null;
                        };

                        $scope.showSections = function () {
                            $scope.editor.currentState = 'sections';
                            $scope.editor.subSelectedID = null;
                            $scope.editor.subSelectedName = null;
                        }

                        $scope.pendingEdit = false;

                        $scope.switchEdit = function (item) {
                            item.isEditting = true;
                            $scope.pendingEdit = true;
                            item.newReservations = item.discountReservations;
                            item.newUnavailable = item.unavailable;
                            item.newAvailable = item.available;
                            item.maxReservations = item.discountReservations + ($scope.config.maximumAvailable - $scope.config.totalReservations);
                        };

                        $scope.cancelEdit = function (item) {
                            item.isEditting = false;
                            $scope.pendingEdit = false;
                        };
                        $scope.saveEdit = function (item) {
                            item.isProcessing = true;

                            var saveObject = {
                                ticketDiscountID: $scope.editor.parent.referenceId,
                                ticketRowID: item.ticketRowID,
                                discountReservations: item.newReservations,
                                unavailable: item.newUnavailable
                            };
                            TicketReservations.createItem(saveObject).then(function (response) {
                                $scope.config.totalReservations += (item.newReservations - item.discountReservations);

                                item.discountReservations = item.newReservations;
                                item.unavailable = item.newUnavailable;
                                item.available = item.newAvailable;
                                item.isEditting = false;
                                item.isProcessing = false;
                                $scope.pendingEdit = false;
                                $scope.execOnChange()();//unwrap it!
                            }, function (err) {
                                item.isEditting = false;
                                item.isProcessing = false;
                                $scope.pendingEdit = false;
                                console.warn(err);
                            });

                        };
                        $scope.moveSeat = function (area, fromSeat, toSeat) {
                            area[fromSeat]--;
                            area[toSeat]++;
                        };


                }]
            }
        }
    ])
});