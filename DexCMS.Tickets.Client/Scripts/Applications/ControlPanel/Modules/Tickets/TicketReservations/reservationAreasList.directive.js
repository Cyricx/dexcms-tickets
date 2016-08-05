define([
    'angular',
    'controlpanel-app',
], function (angular, module) {
    module.directive('dexcmsReservationAreasList', [
        'dexcmsSettings',
        function (dexcmsSettings) {
            return {
                restrict: "E",
                replace: true,
                scope: {
                    "ticketAreas": "=",
                    "config": "=",
                },
                templateUrl: dexcmsSettings.startingRoute + '/modules/tickets/ticketreservations/views/_reservationareas.list.html',
                controller: [
                    '$scope',
                    'TicketReservations',
                    function ($scope, TicketReservations) {
                        
                        $scope.editor = {
                            parent: $scope.config,
                            currentState: 'areas',
                            selectedID: null,
                            selectedName: null,
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
                                ticketDiscountID: $scope.config.referenceId,
                                ticketAreaID: item.ticketAreaID,
                                discountReservations: item.newReservations,
                                unavailable: item.newUnavailable
                            };
                            TicketReservations.createItem(saveObject).then(function (response) {
                                //worked!
                                $scope.config.totalReservations += (item.newReservations - item.discountReservations);

                                item.discountReservations = item.newReservations;
                                item.unavailable = item.newUnavailable;
                                item.available = item.newAvailable;
                                item.isEditting = false;
                                item.isProcessing = false;
                                $scope.pendingEdit = false;

                            }, function (err) {
                                //shit
                                item.isEditting = false;
                                item.isProcessing = false;
                                $scope.pendingEdit = false;
                            });

                        };
                        $scope.moveSeat = function (area, fromSeat, toSeat) {
                            console.log('moving');
                            area[fromSeat]--;
                            area[toSeat]++;
                        };

                        $scope.showSections = function (area) {
                            $scope.editor.currentState = 'sections';
                            $scope.editor.selectedID = area.ticketAreaID;
                            $scope.editor.selectedName = area.name;
                            $scope.selectedSections = area.ticketSections;
                        };

                        $scope.calculateAreaSeats = function () {
                            angular.forEach($scope.ticketAreas, function (area) {
                                if (!area.isGA) {
                                    area.maxCapacity = 0;
                                    area.assigned = 0;
                                    area.discountAssigned = 0;
                                    area.unclaimedReservations = 0;
                                    area.available = 0;
                                    area.unavailable = 0;
                                    area.discountReservations = 0;
                                    angular.forEach(area.ticketSections, function (section) {
                                        area.maxCapacity += section.maxCapacity;
                                        area.assigned += section.assigned;
                                        area.discountAssigned += section.discountAssigned;
                                        area.unclaimedReservations += section.unclaimedReservations;
                                        area.available += section.available;
                                        area.unavailable += section.unavailable;
                                        area.discountReservations += section.discountReservations;
                                    });
                                }
                            });
                        };

                }]
            }
        }
    ])
});