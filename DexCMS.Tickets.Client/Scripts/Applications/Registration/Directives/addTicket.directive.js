define([
    'underscore',
], function (_) {
    return function (module) {
        module.directive('dexcmsAddTicket', function () {

            return {
                restrict: 'E',
                templateUrl: '../../../scripts/dexcmsapps/applications/registration/directives/_addTicket.html',
                controller: ['$scope', 'EventInfo', 'Registrations', '$filter', 'ngToast', 'Tickets',
                    function ($scope, EventInfo, Registrations, $filter, ngToast, Tickets) {

                        Registrations.setSegment(EventInfo.segment);

                        $scope.currentItem = {};
                        $scope.isOpen = false;
                        $scope.hasLoaded = false;
                        $scope.registrationMessage = 'This event is not currently open for registration.';

                        //!? Possibly not in use!
                        // $scope.tickets = Tickets.getTickets();

                        var _areas, _sections, _rows;

                        var _buildAreas = function (areas) {
                            var retAreas = [];
                            angular.forEach(areas, function (area) {
                                if (area.availableTickets > 0) {
                                    var display = area.name + ' (' + area.availableTickets + ' available)';
                                    retAreas.push({
                                        areaID: area.registrationAreaID,
                                        display: display
                                    });
                                }
                            });
                            return retAreas;
                        };

                        var _buildSections = function (areaID) {
                            var retSections = [];
                            var getAreas = _.where(_areas, { registrationAreaID: areaID });
                            if (getAreas) {
                                var selectedArea = getAreas[0];
                                if (!selectedArea.isGA) {
                                    _sections = selectedArea.registrationSections;

                                    angular.forEach(_sections, function (section) {
                                        if (section.availableTickets > 0) {
                                            var display = section.name + ' (' + section.availableTickets + ' available)';
                                            retSections.push({
                                                sectionID: section.registrationSectionID,
                                                display: display
                                            });
                                        }
                                    });
                                } else {
                                    $scope.ticketLimit = selectedArea.availableTickets;
                                    _buildAges();
                                    return undefined;
                                }
                            }
                            return retSections;
                        };

                        var _buildRows = function (sectionID) {
                            var retRows = [];
                            var getSections = _.where(_sections, { registrationSectionID: sectionID });
                            if (getSections) {
                                var selectedSection = getSections[0];
                                _rows = selectedSection.registrationRows;

                                angular.forEach(_rows, function (row) {
                                    if (row.availableTickets > 0) {
                                        var display = row.designation + ' (' + row.availableTickets + ' available)';
                                        retRows.push({
                                            rowID: row.registrationRowID,
                                            display: display
                                        });
                                    }
                                });
                            }
                            return retRows;
                        };

                        var _buildAges = function () {
                            Registrations.getAgeGroups($scope.currentItem.areaID).then(function (response) {
                                $scope.ageGroups = response.data;
                            });
                        };

                        var initAreas = function () {
                            Registrations.getAreas().then(function (response) {
                                if (response.data.length > 0) {
                                    _areas = response.data;
                                    $scope.areas = _buildAreas(response.data);
                                    $scope.hasLoaded = true;
                                    $scope.isOpen = true;
                                } else {
                                    $scope.hasLoaded = true;
                                    $scope.isOpen = false;
                                }
                            }, function (error) {
                                $scope.hasLoaded = true;
                                $scope.isOpen = false;
                                $scope.registrationMessage = error.data.message;
                            });
                        };

                        var _buildStorageTickets = function (localTickets, serverTickets) {
                            var i = 0;
                            angular.forEach(localTickets.tickets, function (ticket) {
                                var location = '';
                                if ($scope.currentItem.rowID) {
                                    location = (_.where(_areas, { registrationAreaID: $scope.currentItem.areaID }))[0].name;
                                    location += " " + (_.where(_sections, { registrationSectionID: $scope.currentItem.sectionID }))[0].name;
                                    location += " " + (_.where(_rows, { registrationRowID: $scope.currentItem.rowID }))[0].designation;
                                } else {
                                    location = (_.where(_areas, { registrationAreaID: $scope.currentItem.areaID }))[0].name;
                                }
                                var storageTicket = {
                                    location: location,
                                    confirmationNumber: serverTickets[i].confirmationNumber,
                                    expirationTime: serverTickets[i].expirationTime,
                                    ticketSeatID: serverTickets[i].ticketSeatID,
                                    ticketPriceID: serverTickets[i].ticketPriceID,
                                    basePrice: ticket.basePrice,
                                    ageGroup: ticket.ageGroup,
                                    ageID: ticket.ageID
                                };
                                Tickets.addTicket(storageTicket);
                                i++;
                            });
                        };

                        initAreas();

                        Registrations.getPrices().then(function (response) {
                            $scope.prices = response.data;
                        });

                        var _resetSections = function () {
                            _sections = undefined;
                            $scope.sections = undefined;
                            $scope.currentItem.sectionID = undefined;
                            _resetRows();
                        };
                        var _resetRows = function () {
                            _rows = undefined;
                            $scope.rows = undefined;
                            $scope.currentItem.rowID = undefined;
                            _resetTickets();
                        };
                        var _resetTickets = function () {
                            $scope.ticketLimit = undefined;
                            $scope.currentItem.seatCount = undefined;
                            $scope.currentItem.tickets = undefined;
                        };

                        var _wipeSelections = function () {
                            item = null;
                            $scope.currentItem = {};
                            _areas = null;
                            $scope.areas = null;
                            _sections = null;
                            $scope.sections = null;
                            _row = null;
                            $scope.rows = null;
                            $scope.ticketLimit = null;
                        }

                        //VIEW DATA
                        $scope.ticketLimit;

                        //VIEW METHODS
                        $scope.selectedArea = function (areaID) {
                            _resetSections();
                            if (areaID) {
                                $scope.sections = _buildSections(areaID);
                            }
                        };
                        $scope.selectedSection = function (sectionID) {
                            _resetRows();
                            if (sectionID) {
                                $scope.rows = _buildRows(sectionID);
                            }
                        };
                        $scope.selectedRow = function (rowID) {
                            _resetTickets();
                            if (rowID) {
                                var getRows = _.where(_rows, { registrationRowID: rowID });
                                if (getRows) {
                                    $scope.ticketLimit = getRows[0].availableTickets;
                                    _buildAges();
                                }
                            }
                        };

                        $scope.createTickets = function (count) {
                            $scope.currentItem.tickets = [];
                            for (var i = 0; i < count; i++) {
                                $scope.currentItem.tickets.push({})
                            }
                        };
                        $scope.validateTickets = function () {
                            if (!$scope.currentItem.tickets || $scope.currentItem.tickets.length == 0) {
                                return true;
                            } else {
                                var missingPrice = true;
                                angular.forEach($scope.currentItem.tickets, function (ticket) {
                                    missingPrice = !ticket.ticketPriceID;
                                });
                                return missingPrice;
                            }
                        };

                        $scope.selectedAge = function (ticket) {
                            if (ticket.ticketPriceID) {
                                var age = _.where($scope.ageGroups, { ticketPriceID: ticket.ticketPriceID });
                                if (age.length > 0) {
                                    ticket.basePrice = age[0].basePrice;
                                    ticket.ageGroup = age[0].ageGroup;
                                    ticket.ageID = age[0].registrationAgeGroupID;
                                }

                            } else {
                                ticket.basePrice = null;
                                ticket.ageGroup = null;
                                ticket.ageID = null;
                            }
                        };

                        $scope.saveItem = function (item) {
                            Registrations.postAddTickets(item).then(function (response) {
                                if (response.data.tickets.length > 0) {
                                    _buildStorageTickets(item, response.data.tickets);
                                    _wipeSelections();
                                    initAreas();
                                    $scope.$emit('addedTickets', {});
                                } else {
                                    ngToast.create({
                                        content: 'Unfortunately those tickets have been reserved by another individual. Please select a new set of tickets.',
                                        className: 'toast-error'
                                    });
                                    _wipeSelections();
                                    initAreas();
                                }
                            });
                        };

                        $scope.getSelectedClass = function (areaID) {
                            if ($scope.currentItem.areaID) {
                                if ($scope.currentItem.areaID == areaID) {
                                    return 'selected';
                                } else {
                                    return 'unselected';
                                }
                            } else {
                                return '';
                            }
                        }

                    }]
            }
        });
    };
});