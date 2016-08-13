define([
   'controlpanel-app',
   '../events/events.navigation.service'
], function (app) {
    app.controller('scheduleItemsListCtrl', [
        '$scope',
        'ScheduleItems',
        '$window',
        '$stateParams',
        'EventsNavigation',
        '$state',
        '$filter',
        'dexCMSControlPanelSettings',
        function ($scope, ScheduleItems, $window, $stateParams, EventsNavigation, $state, $filter, dexcmsSettings) {
            $scope.subtitle = "View Schedule";

            $scope.eventID = $stateParams.id;

            var _renderDate = function (value, item) {
                if (value != null) {
                    if (!item.isAllDay) {
                        return $filter('date')(value, "MM/dd/yyyy h:mm a");
                    } else {
                        return $filter('date')(value, "MM/dd/yyyy");
                    }

                } else {
                    return null;
                }
            };
            
            $scope.table = {
                columns: [
                    { property: 'scheduleItemID', title: 'ID' },
                    { property: 'title', title: 'Title' },
                    { property: 'startDate', title: 'Start', dataFunction: _renderDate },
                    { property: 'endDate', title: 'End', dataFunction: _renderDate },
                    { property: 'scheduleStatusName', title: 'Status' },
                    { property: 'scheduleTypeName', title: 'Type' },
                    {
                        property: '', title: '', disableSorting: true,
                        dataTemplate: dexcmsSettings.startingRoute + 'modules/tickets/scheduleitems/_scheduleitems.list.buttons.html'
                    }
                ],
                defaultSort: 'scheduleItemID',
                functions: {
                    remove: function (id) {
                        if (confirm('Are you sure?')) {
                            ScheduleItems.deleteItem(id).then(function (response) {
                                $window.location.reload();
                            });
                        }
                    }
                }
            };

            ScheduleItems.getListByEvent($scope.eventID).then(function (data) {
                $scope.table.promiseData = data;
            });

            EventsNavigation.getNavigation($stateParams.id, function (data) {
                $scope.navigation = data;
                $scope.table.filePrefix = data.title.replace('Manage ', '').replace(/ /g, '-') + '-Schedule-Items';
                EventsNavigation.setActive($state.current.name);
            });
        }
    ]);
});