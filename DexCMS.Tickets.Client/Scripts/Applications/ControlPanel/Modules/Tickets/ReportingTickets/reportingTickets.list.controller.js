define([
    'controlpanel-app',
    '../events/events.navigation.service'
], function (app) {
    app.controller('reportingTicketsListCtrl', [
        '$scope',
        'ReportingTickets',
        'DTOptionsBuilder',
        'DTColumnBuilder',
        '$compile',
        '$window',
        '$stateParams',
        'EventsNavigation',
        '$state',
        '$filter',
        function ($scope, ReportingTickets, DTOptionsBuilder, DTColumnBuilder, $compile, $window, $stateParams, EventsNavigation, $state, $filter) {
            $scope.subtitle = "Reporting - Tickets";
            
            $scope.eventID = $stateParams.id;
            
            EventsNavigation.getNavigation($stateParams.id, function (data) {
                $scope.navigation = data;
                EventsNavigation.setActive($state.current.name);
            });
            
            $scope.dtOptions = DTOptionsBuilder.fromFnPromise(function () {
                return ReportingTickets.getList($scope.eventID);
            }).withBootstrap().withOption('createdRow', createdRow);
            
            $scope.dtColumns = [
                DTColumnBuilder.newColumn('orderID').withTitle('Order'),
                DTColumnBuilder.newColumn('ticketID').withTitle('Ticket'),
                DTColumnBuilder.newColumn('userName').withTitle('User').renderWith(userHtml),
                DTColumnBuilder.newColumn('firstName').withTitle('Holder').renderWith(holderHtml),
                DTColumnBuilder.newColumn('ticketAreaName').withTitle('Designation').renderWith(designationHtml),
                DTColumnBuilder.newColumn('ticketDiscountName').withTitle('Discount').renderWith(discountHtml),
                DTColumnBuilder.newColumn('options').withTitle('Options').renderWith(optionHtml),
                DTColumnBuilder.newColumn('ticketTotalPrice').withTitle('Ticket Price').renderWith(currencyHtml),
                DTColumnBuilder.newColumn('orderTotal').withTitle('Order Total').renderWith(currencyHtml),
            ];
            
            function createdRow(row, data, dataIndex) {
                // Recompiling so we can bind Angular directive to the DT
                $compile(angular.element(row).contents())($scope);
            }
            function optionHtml(data, type, full, meta) {
                var html = "<ul>";
                angular.forEach(full.options, function (option) {
                    html += "<li><strong>" + option.optionName + "</strong>: " + option.optionChoiceName + "</li>";
                });
                return html;
            };
            
            function currencyHtml(data, type, full, meta) {
                return $filter('currency')(data);
            }
            
            function discountHtml(data, type, full, meta) {
                var discount = data;
                if (full.ticketDiscountCode) {
                    discount += ' (' + full.ticketDiscountCode + ')';
                }
                return discount;
            }
            
            function userHtml(data, type, full, meta) {
                var html = '<span>' + data + '</span>';
                if (full.preferredName) {
                    html += '<br /><em>(' + full.preferredName + ')</em>';
                }
                return html;
            }
            
            function designationHtml(data, type, full, meta) {
                
                var designation = data;
                
                if (full.ticketSectionName) {
                    designation += ' ' + full.ticketSectionName;
                }
                if (full.ticketRowDesignation) {
                    designation += ' ' + full.ticketRowDesignation;
                }
                return designation;
      
            }
            
            function holderHtml(data, type, full, meta) {
                var holder = data;
                if (full.middleInitial) {
                    holder += " " + full.middleInitial;
                }
                holder += " " + full.lastName;
                
                return holder;
            }
            
            function actionsHtml(data, type, full, meta) {
                var buttons = '<a class="btn btn-warning" ui-sref="scheduleitems/:id/:siID({id: +' + $scope.eventID + ', siID: ' + data.scheduleItemID + '})">' +
                   '   <i class="fa fa-edit"></i>' +
                   '</a>';
                buttons += ' <button class="btn btn-danger" ng-click="delete(' + data.scheduleItemID + ')">' +
                   '   <i class="fa fa-trash-o"></i>' +
                   '</button>';
                return buttons;
            }
            
            function dateHtml(data, type, full, meta) {
                if (data != null) {
                    if (!full.isAllDay) {
                        return $filter('date')(data, "MM/dd/yyyy h:mm a");
                    } else {
                        return $filter('date')(data, "MM/dd/yyyy");
                    }
                    
                } else {
                    return null;
                }

            }
            $scope.delete = function (id) {
                if (confirm('Are you sure?')) {
                    ScheduleItems.deleteItem(id).then(function (response) {
                        $window.location.reload();
                    });
                }
            };
        }
    ]);
});