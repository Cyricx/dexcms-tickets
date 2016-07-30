define([
   'controlpanel-app',
   '../events/events.navigation.service',
   '../eventagegroups/eventagegroups.service',
   '../ticketoptions/ticketoptions.service'
], function (app) {
    app.controller('ticketOptionChoicesEditorCtrl', [
        '$scope',
        'TicketOptionChoices',
        '$stateParams',
        '$state',
        'EventsNavigation',
        'EventAgeGroups',
        'TicketOptions',
        function ($scope, TicketOptionChoices, $stateParams, $state, EventsNavigation, EventAgeGroups, TicketOptions) {

            $scope.eventID = $stateParams.id;
            $scope.optionID = $stateParams.toID;


            var id = $stateParams.tocID || null;

            $scope.buildSubTitle = function () {
                if ($scope.ticketOption) {
                    return (id == null ? "Add " : "Edit ") + "Option Choice for " + $scope.ticketOption.name;

                } else {
                    return "Loading...";
                }
                
            };


            EventsNavigation.getNavigation($scope.eventID, function (data) {
                $scope.navigation = data;
                EventsNavigation.setActive($state.current.name);
            });

            $scope.currentItem = {ticketOptionID: $scope.optionID, cbEventAges: [], markupPrice: 0};

            TicketOptions.getItem($scope.optionID).then(function (response) {
                $scope.ticketOption = response.data;
            });

            if (id != null) {
                TicketOptionChoices.getItem(id).then(function (response) {
                    $scope.currentItem = response.data;
                });
            }

            EventAgeGroups.getListByEvent($scope.eventID).then(function (data) {
                $scope.eventAgeGroups = data;
                //if new, auto check or all!
                if (!$scope.currentItem.ticketOptionChoiceID) {
                    angular.forEach($scope.eventAgeGroups, function (age) {
                        $scope.currentItem.cbEventAges.push(age.eventAgeGroupID);
                    });
                }
            });

            $scope.ageIsSelected = function (age) {
                return $scope.currentItem.cbEventAges.indexOf(age.eventAgeGroupID) > -1;
            }
            $scope.toggleAges = function (age) {
                var index = $scope.currentItem.cbEventAges.indexOf(age.eventAgeGroupID);
                if (index > -1) {
                    $scope.currentItem.cbEventAges.splice(index, 1);
                } else {
                    $scope.currentItem.cbEventAges.push(age.eventAgeGroupID);
                }
            }

            $scope.isInvalidChecks = function () {
                return $scope.currentItem.cbEventAges.length === 0;
            }

            $scope.save = function (item) {
                if (item.ticketOptionChoiceID) {
                    TicketOptionChoices.updateItem(item.ticketOptionChoiceID, item).then(function (response) {
                        $state.go('ticketoptions/:id/:toID', { id: $scope.eventID, toID: $scope.optionID });
                    });
                } else {
                    TicketOptionChoices.createItem(item).then(function (response) {
                        $state.go('ticketoptions/:id/:toID', { id: $scope.eventID, toID: $scope.optionID });
                    });
                }
            }
        }
    ]);
});