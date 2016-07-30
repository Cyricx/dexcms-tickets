define([
    'controlpanel-app',
    '../../base/states/states.service'
], function (app) {
    app.controller('venuesEditorCtrl', [
        '$scope',
        'Venues',
        '$stateParams',
        '$state',
        'States',
        function ($scope, Venues, $stateParams, $state, States) {

            var id = $stateParams.id || null;

            $scope.title = (id == null ? "Add " : "Edit ") + "Venue";

            $scope.currentItem = {};

            States.getList().then(function (response) {
                $scope.states = response;
            });

            if (id != null) {
                Venues.getItem(id).then(function (response) {
                    $scope.currentItem = response.data;
                });
            }

            $scope.seatingGroup = "Areas";
            $scope.seatingNote = "(Tenting, Cabin, Lawn, Seats, etc)";

            $scope.changeSeatingGroup = function (seatingGroup) {
                switch (seatingGroup) {
                    case "Sections":
                        seatingGroup = "Areas";
                        $scope.seatingNote = "(Tenting, Cabin, Lawn, Seats, etc)";
                        break;
                    case "Rows":
                        seatingGroup = "Sections";
                        $scope.seatingNote = "(S1, S2, Section A, Section B, etc)";
                        break;
                    default:
                        seatingGroup = "Areas";
                        break;
                }
                $scope.seatingGroup = seatingGroup;
            };

            $scope.selectedArea = {};

            $scope.setSections = function (area) {
                if (validateArea(area)) {
                    $scope.seatingGroup = "Sections";
                    $scope.seatingNote = "(S1, S2, Section 1, Section 2, etc)";
                    $scope.selectedArea = area;
                }
            }
            $scope.setRows = function (section) {
                if (validateSection(section)) {
                    $scope.seatingGroup = "Rows";
                    $scope.seatingNote = "(Left Side, Right Side, Row A, Row B, etc)";
                    $scope.selectedSection = section;
                }
            }

            $scope.save = function (item) {
                if (item.venueID) {
                    Venues.updateItem(item.venueID, item).then(function (response) {
                        $state.go('venues');
                    });
                } else {
                    Venues.createItem(item).then(function (response) {
                        $state.go('venues');
                    });
                }
            }

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
            }
            $scope.switchAddArea = function () {
                if ($scope.managingArea == -1) {
                    $scope.addingArea = !$scope.addingArea;
                }
            }
            $scope.switchAddSection = function () {
                if ($scope.managingSection == -1) {
                    $scope.addingSection = !$scope.addingSection;
                }
            }
            $scope.switchAddRow = function () {
                if ($scope.managingRow == -1) {
                    $scope.addingRow = !$scope.addingRow;
                }
            }

            $scope.saveArea = function (area) {
                if (validateArea(area)) {
                    var newArea = angular.copy(area);
                    newArea.venueSections = [];
                    if (!$scope.currentItem.venueAreas) {
                        $scope.currentItem.venueAreas = [];
                    }
                    $scope.currentItem.venueAreas.push(newArea);
                    area.name = '';
                    $scope.switchAddArea();
                }
            }

            $scope.deleteArea = function (area) {
                if (confirm('Are you sure?')) {
                    var index = $scope.currentItem.venueAreas.indexOf(area);
                    $scope.currentItem.venueAreas.splice(index, 1);
                }
            }

            $scope.saveSection = function (section) {
                if (validateSection(section)) {
                    var newSection = angular.copy(section);
                    newSection.venueRows = [];
                    $scope.selectedArea.venueSections.push(newSection);
                    section.name = '';
                    $scope.switchAddSection();
                }
            }

            $scope.deleteSection = function (section) {
                if (confirm('Are you sure?')) {
                    var index = $scope.selectedArea.venueSections.indexOf(section);
                    $scope.selectedArea.venueSections.splice(index, 1);
                }
            }

            $scope.saveRow = function (row) {
                if (validateRow(row)) {
                    var newRow = angular.copy(row);
                    $scope.selectedSection.venueRows.push(newRow);
                    row.designation = '';
                    $scope.switchAddRow();
                }
            }
            $scope.deleteRow = function (row) {
                if (confirm('Are you sure?')) {
                    var index = $scope.selectedSection.venueRows.indexOf(row);
                    $scope.selectedSection.venueRows.splice(index, 1);
                }
            }
            $scope.setIcon = function (item) {
                return item.editting ? "fa-floppy-o" : "fa-pencil-square-o";
            }
            $scope.managingArea = -1;
            $scope.managingSection = -1;
            $scope.managingRow = -1;
            $scope.changeEditor = function (item, index) {
                //have to manually check validation for now
                switch ($scope.seatingGroup) {
                    case "Areas":
                        if (!$scope.addingArea) {
                            if (!item.editting) {
                                item.editting = true;
                                $scope.managingArea = index;
                            } else if (validateArea(item)) {
                                item.editting = false;
                                $scope.managingArea = -1;
                            }
                        }
                        break;
                    case "Sections":
                        if (!$scope.addingSection) {
                            if (!item.editting) {
                                item.editting = true;
                                $scope.managingSection = index;
                            } else if (validateSection(item)) {
                                item.editting = false;
                                $scope.managingSection = -1;
                            }
                        }
                        break;

                    case "Rows":
                        if (!$scope.addingRow) {
                            if (!item.editting) {
                                item.editting = true;
                                $scope.managingRow = index;
                            } else if (validateRow(item)) {
                                item.editting = false;
                                $scope.managingRow = -1;
                            }
                        }

                    default:
                        break;

                }
            };

            $scope.enableClick = function (type, index) {
                return $scope['adding' + type] || ($scope['managing' + type] != -1 && $scope['managing' + type] != index);
            }

             var validateArea = function (area) {
                if (area.name) {
                    return true;
                } else {
                    return false;
                }
            }
            var validateSection = function (section) {
                if (section.name) {
                    return true;
                } else {
                    return false;
                }
            }
            var validateRow = function (row) {
                if (row.designation) {
                    return true;
                } else {
                    return false;
                }
            }
        }//end controller function
    ]);
});