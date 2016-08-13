define([
   'controlpanel-app',
    '../venues/venues.service',
    '../events/events.navigation.service'
], function (app) {
    app.controller('eventTicketsListCtrl', [
        '$scope',
        'EventTickets',
        '$stateParams',
        'Venues',
        '$state',
        'EventsNavigation',
        function ($scope, EventTickets, $stateParams, Venues, $state, EventsNavigation) {
            var eventID = $stateParams.id;
            
            $scope.isLoading = true;
            EventTickets.getItem(eventID).then(function (response) {
                $scope.currentItem = response.data;
                $scope.isLoading = false;
            });


            $scope.title = "Manage Tickets";
            
            EventsNavigation.getNavigation($stateParams.id, function (data) {
                $scope.navigation = data;
                EventsNavigation.setActive($state.current.name);
            });


            $scope.seatingGroup = "Areas";
            $scope.seatingNote = "(Tenting, Cabin, Lawn, Seats, etc)";
            
            
            $scope.calculateColumns = function (collection, isEditorColumn) {
                if (collection) {
                    var columnWidth = 1;

                    if (collection.length + 1 < 12) {
                        columnWidth = Math.floor(12 / (collection.length + 1));
                    }
                    if (columnWidth < 3) {
                        columnWidth = 3;
                    }
                    return "col-xs-" + (columnWidth * 2) + " col-md-" + columnWidth;
                }
            };

            $scope.managingAreaIndex = -1;
            $scope.managingSectionIndex = -1;
            $scope.managingRowIndex = -1;

            $scope.switchAdd = function (location) {
                $scope['adding' + location] = !$scope['adding' + location];
                if ($scope['adding' + location]) {
                    $scope['new' + location] = {};
                    if (location == 'Area' || location == 'Row') {
                        $scope['new' + location].maxCapacity = 1;
                    }
                }
            };

            $scope.switchEdit = function (location, item, index) {
                        item.editting = !item.editting;
                        if (item.editting) {
                            $scope['edit'+location] = angular.copy(item);
                            $scope['managing' + location + 'Index'] = index;
                        } else {
                            $scope['managing' + location + 'Index'] = -1;
                        }
            };

            $scope.changeUnavailable = function (location, amount) {
                location.newUnavailable = location.newUnavailable || 0;
                if (location.newUnavailable + amount <= location.newMaxCapacity && location.newUnavailable + amount >= 0) {
                    location.newUnavailable = location.newUnavailable + amount;
                }
            };

            $scope.saveArea = function (area) {
                var newArea = angular.copy(area);
                newArea.newUnavailable = newArea.newUnavailable || 0;
                newArea.ticketSections = [];
                if (!$scope.currentItem.ticketAreas) {
                    $scope.currentItem.ticketAreas = [];
                }
                $scope.currentItem.ticketAreas.push(newArea);
                $scope.switchAdd('Area');
            };

            $scope.updateArea = function (area, editted, index) {
                area.name = editted.name;
                area.isGA = editted.isGA;
                area.displayOrder = editted.displayOrder;
                area.newMaxCapacity = editted.newMaxCapacity;
                area.newUnavailable = editted.newUnavailable;
                $scope.switchEdit('Area', area, index);
            };

            $scope.deleteArea = function (area, index) {
                if (confirm('Are you sure?')) {
                    $scope.currentItem.ticketAreas.splice(index, 1);
                    $scope.managingAreaIndex = -1;
                }
            };

            $scope.enableClick = function (type, index) {
                return $scope['adding' + type] || ($scope['managing' + type + 'Index'] != -1 && $scope['managing' + type + 'Index'] != index);
            };

            $scope.saveSection = function (section) {
                var newSection = angular.copy(section);
                newSection.ticketRows = [];
                $scope.selectedArea.ticketSections.push(newSection);
                $scope.switchAdd('Section');
            };

            $scope.updateSection = function (section, editted, index) {
                section.name = editted.name;
                $scope.switchEdit('Section', section, index);
            };

            $scope.deleteSection = function (section, index) {
                if (confirm('Are you sure?')) {
                    $scope.selectedArea.ticketSections.splice(index, 1);
                    $scope.managingSectionIndex = -1;
                }
            };

            $scope.setSections = function (area) {
                $scope.selectedArea = area;
                $scope.changeToGroup("Sections");
            };

            $scope.setRows = function (section) {
                $scope.selectedSection = section;
                $scope.changeToGroup("Rows");
            };

            $scope.changeToGroup = function (seatingGroup) {
                switch (seatingGroup) {
                    case "Rows":
                        $scope.seatingGroup = "Rows";
                        $scope.seatingNote = "(Left Side, Right Side, Row A, Row B, etc)";
                        $scope.managingRowIndex = -1;
                        break;
                    case "Sections":
                        $scope.seatingGroup = "Sections";
                        $scope.seatingNote = "(S1, S2, Section A, Section B, etc)";
                        $scope.managingSectionIndex = -1;
                        break;
                    default:
                        $scope.seatingGroup = "Areas";
                        $scope.seatingNote = "(Tenting, Cabin, Lawn, Seats, etc)";
                        $scope.managingAreaIndex = -1;
                        break;
                }
            };
            
            $scope.toPreviousGroup = function (currentGroup) {
                switch (currentGroup) {
                    case "Rows":
                        $scope.changeToGroup("Sections");
                        break;
                    case "Sections":
                        $scope.changeToGroup("Areas");
                        break;
                }
            };

            $scope.saveRow = function (row) {
                var newRow = angular.copy(row);
                newRow.newUnavailable = newRow.newUnavailable || 0;
                $scope.selectedSection.ticketRows.push(newRow);
                $scope.switchAdd('Row');
            };

            $scope.updateRow = function (row, editted, index) {
                row.designation = editted.designation;
                row.newMaxCapacity = editted.newMaxCapacity;
                row.newUnavailable = editted.newUnavailable;
                $scope.switchEdit('Row', row, index);
            };

            $scope.deleteRow = function (row, index) {
                if (confirm('Are you sure?')) {
                    $scope.selectedSection.ticketRows.splice(index, 1);
                    $scope.managingRowIndex = -1;
                }
            };

            $scope.saveAll = function () {
                $scope.isSavingAll = true;
                if (confirm('Are you sure you wish to permanently set these tickets?')) {
                    EventTickets.updateItem(eventID, $scope.currentItem).then(function () {
                        EventTickets.getItem(eventID).then(function (response) {
                            $scope.currentItem = response.data;
                            $scope.isSavingAll = false;
                            $scope.changeToGroup();
                        });
                    });
                }
            };

            $scope.getAreaCount = function (area, property) {
                if (area.isGA) {
                    return area[property];
                } else {
                    
                    var count = 0;
                    if (area.ticketSections) {
                        angular.forEach(area.ticketSections, function (section) {
                            if (section.ticketRows) {
                                angular.forEach(section.ticketRows, function (row) {
                                    count += row[property];
                                });
                            }
                        });
                    }
                    return count;
                }
            };

            $scope.removeAllSeats = function () {
                if (confirm('Are you sure you want to delete ALL areas?!')) {
                    $scope.currentItem.ticketAreas = [];
                    $scope.selectedArea = null;
                    $scope.selectedSection = null;
                    $scope.managingAreaIndex = -1;
                    $scope.managingSectionIndex = -1;
                    $scope.managingRowIndex = -1;
                    $scope.changeToGroup('areas');
                    $scope.hasLoadedVenue = false;
                }
            };

            $scope.loadFromVenue = function () {
                $scope.hasLoadedVenue = true;
                Venues.getItem($scope.currentItem.venueID).then(function (response) {
                    var venue = response.data;
                    var et = $scope.currentItem;
                    angular.forEach(venue.venueAreas, function (area) {
                        //process areas
                        if (area.isGA) {
                            et.ticketAreas.push({
                                name: area.name,
                                isGA: area.isGA,
                                maxCapacity: area.gaSeatCount,
                                newMaxCapacity: area.gaSeatCount,
                                unavailable: 0,
                                newUnavailable: 0
                            });
                        } else {
                            var etArea = {
                                name: area.name,
                                isGA: area.isGA,
                                ticketSections: []
                            };

                            //process sections
                            angular.forEach(area.venueSections, function (section) {
                                var etSection = {
                                    name: section.name,
                                    ticketRows: []
                                };

                                //process rows
                                angular.forEach(section.venueRows, function (row) {
                                    etSection.ticketRows.push({
                                        designation: row.designation,
                                        maxCapacity: row.seatCount,
                                        newMaxCapacity: row.seatCount,
                                        unavailable: 0,
                                        newUnavailable: 0
                                    });
                                });

                                etArea.ticketSections.push(etSection);
                            });

                            et.ticketAreas.push(etArea);
                        }
                    })
                });
            };
        }
    ]);
});