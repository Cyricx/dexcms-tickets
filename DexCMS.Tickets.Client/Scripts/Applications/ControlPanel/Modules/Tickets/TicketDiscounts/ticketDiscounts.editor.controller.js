define([
   'controlpanel-app',
   '../events/events.navigation.service',
   '../eventagegroups/eventagegroups.service',
   '../ticketareas/ticketareas.service',
   '../ticketoptions/ticketoptions.service',
   '../ticketareadiscounts/ticketareadiscounts.service',
   '../ticketoptiondiscounts/ticketoptiondiscounts.service',
   'underscore'
], function (app) {
    app.controller('ticketDiscountsEditorCtrl', [
        '$scope',
        'TicketDiscounts',
        '$stateParams',
        '$state',
        'EventsNavigation',
        'EventAgeGroups',
        'TicketOptions',
        'TicketAreas',
        'TicketOptionDiscounts',
        'TicketAreaDiscounts',
        '$filter',
        function ($scope, TicketDiscounts,
            $stateParams, $state, EventsNavigation, EventAgeGroups, TicketOptions, TicketAreas,
            TicketOptionDiscounts,
            TicketAreaDiscounts,
            $filter) {

            var id = $stateParams.tdID || null;

            $scope.eventID = $stateParams.id;

            $scope.subtitle = (id == null ? "Add " : "Edit ") + "Ticket Discount";

            $scope.currentItem = { eventID: $scope.eventID, cbEventAges: [], isActive: true};

            var initSubSections = function () {
                TicketAreas.getListByEvent($scope.eventID).then(function (areas) {
                    TicketAreaDiscounts.getListByDiscount(id).then(function (discounts) {
                        var mergedData = areas;
                        //set all areas to have this discountid
                        angular.forEach(areas, function (area) {
                            area.ticketDiscountID = id;
                            area.adjustmentType = "0";
                        });
                        //loop through discounts and find matching area
                        angular.forEach(discounts, function (discount) {
                            var area = _.findWhere(areas, { ticketAreaID: discount.ticketAreaID });
                            area.isStored = true;
                            area.adjustmentType = discount.adjustmentType.toString();
                            area.adjustmentAmount = discount.adjustmentAmount;
                        })

                        $scope.areaDiscounts = areas;
                    });
                });


                TicketOptions.getListByEvent($scope.eventID).then(function (options) {
                    TicketOptionDiscounts.getListByDiscount(id).then(function (discounts) {
                        var mergedData = options;
                        //set all options to have this discountid
                        angular.forEach(options, function (opt) {
                            opt.ticketDiscountID = id;
                            opt.adjustmentType = "0";
                        });
                        //loop through discounts and find matching area
                        angular.forEach(discounts, function (discount) {
                            var opt = _.findWhere(options, { ticketOptionID: discount.ticketOptionID });
                            opt.isStored = true;
                            opt.adjustmentType = discount.adjustmentType.toString();
                            opt.adjustmentAmount = discount.adjustmentAmount;
                        });

                        $scope.optionDiscounts = options;
                    });
                });
            }

            $scope.buildDiscountAmount = function (item) {
                if (item.adjustmentAmount) {
                    if (item.adjustmentType != "0") {
                        return $filter('currency')(item.adjustmentAmount) + ' off';
                    } else {
                        return $filter('number')(item.adjustmentAmount, 0) + '% off';
                    }
                } else {
                    return '(no discount)';
                }
            };
            $scope.subItemEditting = false;
            $scope.switchToEdit = function (item) {
                item.isEditting = true;
                $scope.subItemEditting = true;
                item.newAmount = item.adjustmentAmount;
                item.newType = item.adjustmentType;
            };

            $scope.cancelEdit = function (item) {
                item.isEditting = false;
                $scope.subItemEditting = false;
                delete item.newAmount;
                delete item.newType;
            };

            $scope.deleteArea = function (item) {
                if (confirm('Are you sure?')) {
                    TicketAreaDiscounts.deleteItem(item.ticketDiscountID, item.ticketAreaID).then(function (response) {
                        item.adjustmentAmount = null;
                        item.adjustmentType = 0;
                        item.isStored = false;
                    });
                }
            };

            $scope.saveArea = function (item) {
                item.isProcessing = true;
                item.adjustmentAmount = item.newAmount;
                item.adjustmentType = item.newType;

                if (item.isStored) {
                    //update
                    TicketAreaDiscounts.updateItem(item.ticketDiscountID, item.ticketAreaID, item).then(function (response) {
                        item.isProcessing = false;
                        item.isEditting = false;
                        delete item.newAmount;
                        delete item.newType;
                        $scope.subItemEditting = false;
                    });
                } else {
                    //create
                    TicketAreaDiscounts.createItem(item).then(function (response) {
                        item.isProcessing = false;
                        item.isEditting = false;
                        item.isStored = true;
                        delete item.newAmount;
                        delete item.newType;
                        $scope.subItemEditting = false;
                    });
                }
            };

            $scope.deleteOption = function (item) {
                if (confirm('Are you sure?')) {
                    TicketOptionDiscounts.deleteItem(item.ticketDiscountID, item.ticketOptionID).then(function (response) {
                        item.adjustmentAmount = null;
                        item.adjustmentType = 0;
                        item.isStored = false;
                    });
                }
            };

            $scope.saveOption = function (item) {
                item.isProcessing = true;
                item.adjustmentAmount = item.newAmount;
                item.adjustmentType = item.newType;

                if (item.isStored) {
                    //update
                    TicketOptionDiscounts.updateItem(item.ticketDiscountID, item.ticketOptionID, item).then(function (response) {
                        item.isProcessing = false;
                        item.isEditting = false;
                        delete item.newAmount;
                        delete item.newType;
                        $scope.subItemEditting = false;
                    });
                } else {
                    //create
                    TicketOptionDiscounts.createItem(item).then(function (response) {
                        item.isProcessing = false;
                        item.isEditting = false;
                        item.isStored = true;
                        delete item.newAmount;
                        delete item.newType;
                        $scope.subItemEditting = false;
                    });
                }
            };

            EventsNavigation.getNavigation($scope.eventID, function (data) {
                $scope.navigation = data;
                EventsNavigation.setActive($state.current.name);
            });

            if (id != null) {
                TicketDiscounts.getItem(id).then(function (response) {
                    $scope.currentItem = response.data;
                    $scope.savedMaximumAvailable = response.data.maximumAvailable;
                });

                initSubSections();
            }

            EventAgeGroups.getListByEvent($scope.eventID).then(function (data) {
                $scope.eventAgeGroups = data;
                //if new, auto check or all!
                if (!$scope.currentItem.ticketDiscountID) {
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
                $scope.isProcessing = true;
                if (item.ticketDiscountID) {
                    TicketDiscounts.updateItem(item.ticketDiscountID, item).then(function (response) {
                        $scope.isProcessing = false;
                        $state.go('ticketdiscounts/:id', { id: $scope.eventID });
                    });
                } else {
                    TicketDiscounts.createItem(item).then(function (response) {
                        $scope.isProcessing = false;
                        $state.go('ticketdiscounts/:id', { id: $scope.eventID });
                    });
                }
            };

            $scope.saveAndStay = function (item) {
                $scope.isProcessing = true;
                if (item.ticketDiscountID) {
                    TicketDiscounts.updateItem(item.ticketDiscountID, item).then(function (response) {
                        $scope.isProcessing = false;
                        $scope.savedMaximumAvailable = $scope.currentItem.maximumAvailable;
                    });
                } else {
                    TicketDiscounts.createItem(item).then(function (response) {
                        $scope.isProcessing = false;
                        $scope.currentItem.ticketDiscountID = response.ticketDiscountID;
                        $scope.savedMaximumAvailable = $scope.currentItem.maximumAvailable;
                        id = response.ticketDiscountID;
                        initSubSections();
                    });
                }
            };
        }
    ]);
});