define([
    'controlpanel-app',
    '../../base/seochangefrequency/seochangefrequency.service',
    '../../base/contentblockorder/contentblockorder.service',
    '../../base/contentblocks/contentblocks.service',
    '../../base/pagecontentimages/pagecontentimages.service',
    '../venues/venues.service',
    '../eventseries/eventseries.service'
], function (app) {
    app.controller('eventsEditorCtrl', [
        '$scope',
        'Events',
        '$stateParams',
        '$state',
        'SeoChangeFrequency',
        'ContentBlockOrders',
        'ContentBlocks',
        'Venues',
        'EventSeries',
        '$window',
        'PageContentImages',
        '$location',
        'EventsNavigation',
        'ngToast',
        function ($scope, Events, $stateParams, $state, SeoChangeFrequency, ContentBlockOrders,
            ContentBlocks, Venues, EventSeries, $window, PageContentImages, $location, EventsNavigation,
            ngToast) {
            $scope.currentItem = { pageContent: { contentAreaID : 1, pageTypeID: 2 }, isPublic: true, forceRegistrationDisabled: false };
            $scope.processing = false;
            
            var id = $stateParams.id || null;
            $scope.isEdit = id != null;
            
            //setup
            Venues.getList().then(function (response) {
                $scope.venues = response;
            });
            EventSeries.getList().then(function (response) {
                $scope.eventSeries = response;
            });
            $scope.hasDragged = false;
            $scope.dragTime = function (event) {
                $scope.hasDragged = true;
            }
            $scope.dragTimeImage = function (event) {
                $scope.hasDraggedImage = true;
            }
            $scope.seoChangeFrequencies = SeoChangeFrequency.getList();
            
            
            if (id != null) {
                EventsNavigation.getNavigation(id, function (data) {
                    $scope.navigation = data;
                    EventsNavigation.setActive($state.current.name);
                });
                $scope.subtitle = "Edit Event";
                
                Events.getItem(id).then(function (response) {
                    $scope.currentItem = response.data;
                    $scope.currentItem.eventStart = new Date($scope.currentItem.eventStart);
                    $scope.currentItem.eventEnd = new Date($scope.currentItem.eventEnd);

                });
            } else {
                $scope.title = "Add Event";
                $scope.currentItem.pageContent.contentTypeID = 5;
            }
            
            $scope.saveAndStay = function (item) {
                $scope.processing = true;
                Events.createItem(item).then(function (response) {
                    $scope.currentItem.pageContent.pageContentID = response.data.eventID;
                    id = $scope.currentItem.pageContent.pageContentID;
                    $scope.currentItem.eventID = response.data.eventID;
                    $scope.processing = false;
                });
            }
            $scope.save = function (item) {
                $scope.processing = true;
                $scope.modelError = null;
                if (item.eventSeriesID) {
                    angular.forEach($scope.eventSeries, function (series) {
                        if (series.eventSeriesID == item.eventSeriesID) {
                            item.pageContent.urlSegment = series.seriesUrlSegment;
                        }
                    });
                }
                
                if (item.eventID) {
                    Events.updateItem(item.eventID, item).success(function (response) {
                        if ($state.current.name != "events/new") {
                            ngToast.create({
                                className: 'success',
                                content: '<h2>Event save successful!</h2>'
                            });
                            $scope.processing = false;
                        } else {
                            $state.go('events');
                        }
                    }).error(function (err) {
                        console.warn('ERROR');
                        console.log(err);
                        $scope.modelError = {
                            message: err.message,
                            errors: err.modelState ? err.modelState.errors : err.stackTrace
                        }
                        $scope.processing = false;
                    });
                } else {
                    Events.createItem(item).success(function (response) {
                        $state.go('events');
                    }).error(function (err) {
                        console.warn('ERROR');
                        console.log(err);
                        var message = err.message;
                        message += err.exceptionMessage ? ' ' + err.exceptionMessage : '';
                        $scope.modelError = {
                            message: message,
                            errors: err.modelState ? err.modelState.errors : err.stackTrace
                        }
                        $scope.processing = false;
                    });
                }
            }
            
            $scope.saveImageOrder = function (item) {
                $scope.processing = true;
                var data = {
                    pageContentID: item.eventID,
                    pageContentImages: []
                };
                for (var i = 0; i < item.pageContent.pageContentImages.length; i++) {
                    data.pageContentImages.push({
                        imageID: item.pageContent.pageContentImages[i].imageID,
                        displayOrder: i
                    });
                }
                
                PageContentImages.updateItems(data).then(function (response) {
                    $scope.hasDraggedImage = false;
                    $scope.processing = false;
                });
            };
            $scope.deleteImage = function ($event, item) {
                $scope.processing = true;
                $event.preventDefault();
                if (confirm('Are you sure?')) {
                    PageContentImages.deleteItem(item.imageID, $scope.currentItem.eventID).then(function (response) {
                        $location.search({ settings: 'true' });
                        $window.location.reload();
                    });
                }
            };
            
            $scope.saveContentBlockOrder = function (item) {
                $scope.processing = true;
                var cbOrder = [];
                angular.forEach(item.contentBlocks, function (cb) {
                    cbOrder.push({
                        contentBlockID: cb.contentBlockID,
                        displayOrder: cb.displayOrder
                    });
                });
                ContentBlockOrders.updateItems(cbOrder).then(function (response) {
                    $scope.hasDragged = false
                    $scope.processing = false;
                });
            };
            $scope.deleteContentBlock = function ($event, item) {
                $scope.processing = true;
                $event.preventDefault();
                if (confirm('Are you sure?')) {
                    ContentBlocks.deleteItem(item.contentBlockID).then(function (response) {
                        $location.search({ settings: 'true' });
                        $window.location.reload();
                    });
                }
            };


        }
    ]);
});