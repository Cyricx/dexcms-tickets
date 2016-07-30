define([
   'controlpanel-app',
   'moment',
   '../events/events.navigation.service'
], function (app, moment) {
    app.controller('ticketCutoffsListCtrl', [
        '$scope',
        'TicketCutoffs',
        '$stateParams',
        '$state',
        'EventsNavigation',
        function ($scope, TicketCutoffs, $stateParams, $state, EventsNavigation) {
            var eventID = $stateParams.id;
            $scope.subtitle = "View Prices";

            $scope.eventID = $stateParams.id;

            EventsNavigation.getNavigation($stateParams.id, function (data) {
                $scope.navigation = data;
                EventsNavigation.setActive($state.current.name);
            });

            $scope.ticketCutoffs = [];

            //loading ticket cutoffs
            var loadCutoffs = function () {
                TicketCutoffs.getListByEvent($scope.eventID).then(function (response) {
                    $scope.ticketCutoffs = response;
                    $scope.isLoading = false;
                });
            };
            $scope.isLoading = true;
            loadCutoffs();

            //CUTOFF - ADD NEW
            $scope.openAddCutoff = function () {
                $scope.newTicketCutoff = { eventID: $scope.eventID };
                $scope.isAddingCutoff = true;
            };
            $scope.cancelAddCutoff = function () {
                delete $scope.addCutoff;
                $scope.isAddingCutoff = false;
            };
            $scope.saveCutoff = function (cutoff) {
                $scope.pendingAddCutoff = true;
                TicketCutoffs.createItem(cutoff).then(function (response) {
                    console.log('created!');
                    loadCutoffs();
                    $scope.isAddingCutoff = false;
                    $scope.pendingAddCutoff = false;
                })
            };


            //CUTOFF - EDIT
            $scope.editCutoff = function (e, cutoff) {
                e.stopPropagation();
                e.preventDefault();
                _cancelCutoffEdits();
                cutoff.isOpen = true;

                cutoff.isEditting = true;
                $scope.editTicketCutoff = angular.copy(cutoff);
                $scope.editTicketCutoff.onSellDate = _fixDate($scope.editTicketCutoff.onSellDate);
                $scope.editTicketCutoff.cutoffDate = _fixDate($scope.editTicketCutoff.cutoffDate);

            };

            $scope.cancelEditCutoff = function (cutoff) {
                cutoff.isEditting = false;
                cutoff.isOpen = false;
            }

            $scope.saveEditCutoff = function (editCutoff, cutoff) {
                $scope.pendingEditCutoff = true;
                cutoff.name = editCutoff.name;
                cutoff.onSellDate = editCutoff.onSellDate;
                cutoff.cutoffDate = editCutoff.cutoffDate;
                TicketCutoffs.updateItem(cutoff).then(function (response) {
                    cutoff.isEditting = false;
                    cutoff.isOpen = false;
                });
            }
            
            var _fixDate = function (date) {
                var fixedDate = _getDatePieces(date);
                return new Date(fixedDate[0], (fixedDate[1] - 1), fixedDate[2]);
            }

            var _getDatePieces = function (datestring) {
                var dateParts = [];
                var datestring = datestring.substr(0, datestring.indexOf('T'));
                dateParts = datestring.split('-');
                return dateParts;
            }

            var _cancelCutoffEdits = function () {
                angular.forEach($scope.ticketCutoffs, function (cutoff) {
                    if (cutoff.isEditting) {
                        cutoff.isEditting = false;
                    }
                })
            }

            //CUTOFF - DELETE
            $scope.$on('ticketCutoffDeleted', function (event, cutoff) {
                $scope.ticketCutoffs.splice($scope.ticketCutoffs.indexOf(cutoff), 1);
            });
            //CUTOFF - ADD
            $scope.$on('ticketCutoffAdded', function (event, cutoff) {
                $scope.ticketCutoffs.push(cutoff);
                loadCutoffs();
            });


        }//end controller
    ]);
});