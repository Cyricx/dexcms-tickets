define([
   'controlpanel-app',
   '../events/events.navigation.service'
], function (app) {
    app.controller('eventAgeGroupsEditorCtrl', [
        '$scope',
        'EventAgeGroups',
        '$stateParams',
        '$state',
        'EventsNavigation',
        'ngToast',
        function ($scope, EventAgeGroups, $stateParams, $state, EventsNavigation, ngToast) {
            EventsNavigation.getNavigation($stateParams.id, function (data) {
                $scope.navigation = data;
                EventsNavigation.setActive($state.current.name);
            });

            var id = $stateParams.eageID || null;

            $scope.eventID = $stateParams.id;

            $scope.subtitle = (id == null ? "Add " : "Edit ") + "Age Group";

            $scope.currentItem = { eventID:$scope.eventID};

            if (id != null) {
                EventAgeGroups.getItem(id).then(function (response) {
                    $scope.currentItem = response.data;
                });
            }

            $scope.save = function (item) {
                $scope.isProcessing = true;
                if (item.eventAgeGroupID) {
                    EventAgeGroups.updateItem(item.eventAgeGroupID, item).then(function (response) {
                        $scope.isProcessing = false;
                        $state.go('eventagegroups/:id', { id: $scope.eventID });
                    }, function (error) {
                        console.error(error.data.exceptionMessage);
                        ngToast.create({
                            className: 'danger',
                            content: '<h2>' + error.data.exceptionMessage + '</h2>'
                        });
                        $scope.isProcessing = false;
                    });
                } else {
                    EventAgeGroups.createItem(item).then(function (response) {
                        $scope.isProcessing = false;
                        $state.go('eventagegroups/:id', {id: $scope.eventID});
                    }, function (error) {
                        console.error(error.data.exceptionMessage);
                        ngToast.create({
                            className: 'danger',
                            content: '<h2>' + error.data.exceptionMessage + '</h2>'
                        });
                        $scope.isProcessing = false;
                    });
                }
            }
        }
    ]);
});