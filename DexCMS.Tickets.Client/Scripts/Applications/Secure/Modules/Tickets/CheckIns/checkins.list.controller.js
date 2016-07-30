define([
    'secure-app'
], function (app) {
    app.controller('checkInsListCtrl', [
        '$scope',
        'CheckIns',
        '$stateParams',
        '$state',
        '$uibModal',
        'dexCMSSecureSettings',
        function ($scope, CheckIns, $stateParams, $state, $uibModal, Settings) {
            
            var segment = $stateParams.segment;
            $scope.segment = segment;
            CheckIns.getList(segment).then(function (response) {
                $scope.tickets = response.data;
            });

            $scope.currentPage = 1;
            $scope.maxPagesDisplayed = 5;
            $scope.itemsPerPage = 10;

            $scope.sortBy = 'purchasedBy';
            $scope.showArrivals = false;

            $scope.changeSort = function (sortBy) {
                $scope.sortBy = $scope.sortBy == sortBy ? '-' + sortBy : sortBy;
            };

            $scope.checkArrivals = function (ticket) {
                
                if ($scope.showArrivals && ticket.arrivalTime) {
                    return true;
                } else if (!$scope.showArrivals && !ticket.arrivalTime) {
                    return true;
                } else {
                    return false;
                }

            };

            $scope.buildPageMessage = function (totalItems, currentPage, itemsPerPage) {
                var min = ((currentPage - 1) * itemsPerPage) + 1;
                var max = currentPage * itemsPerPage;
                max = max > totalItems ? totalItems : max;
                return 'Viewing ' + min + ' to ' + max + ' items out of ' + totalItems;
            };

            $scope.getSortClass = function (column) {
                if ($scope.sortBy === column) {
                    return 'fa-sort-desc';
                } else if ($scope.sortBy === '-' + column) {
                    return 'fa-sort-asc';
                } else {
                    return 'fa-sort dull-icon';
                }
            };

            $scope.checkEdit = function (ticket) {
                var editItem = angular.copy(ticket);

                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: Settings.startingRoute + 'modules/tickets/checkins/checkins.modal.html',
                    controller: 'ModalInstanceCtrl',
                    resolve: {
                        item: function () {
                            return editItem;
                        }
                    }
                });
                modalInstance.result.then(function (item) {
                    ticket.firstName = item.firstName;
                    ticket.middleInitial = item.middleInitial;
                    ticket.lastName = item.lastName;
                    ticket.arrivalTime = item.arrivalTime;
                    ticket.hasArrived = item.hasArrived;
                })
            };
        }
    ]);


    app.controller('ModalInstanceCtrl', [
        '$scope',
        '$uibModalInstance',
        'item',
        'CheckIns',
        '$stateParams',
        function ($scope, $uibModalInstance, item, CheckIns, $stateParams) {
            var segment = $stateParams.segment;

            $scope.item = item;
            $scope.currentPage = 1;
            $scope.saveTicket = function (ticket) {
                $scope.isProcessing = true;
                var saveTicket = $scope.item;
                //ticket.firstName = $scope.currentEditting.firstName;
                //ticket.middleInitial = $scope.currentEditting.middleInitial;
                CheckIns.updateItem(segment, saveTicket.ticketID, saveTicket).then(function (response) {
                    $scope.isProcessing = false;
                    $uibModalInstance.close(saveTicket);
                    if (!saveTicket.arrivalTime) {
                        alert('The attendee was NOT marked as arrived.');
                    }
                }, function (error) {
                    console.error(error);
                    $scope.item = null;
                    $scope.isProcessing = false;
                });
            };

            $scope.setDefaultArrival = function () {
                if ($scope.item.hasArrived) {
                    if (!$scope.item.arrivalTime) {
                        $scope.item.arrivalTime = new Date();
                    }
                }
            };
            $scope.clearEdit = function () {
                $scope.currentEditting = null;
                $uibModalInstance.dismiss('cancel');
            }

        }
    ])
});