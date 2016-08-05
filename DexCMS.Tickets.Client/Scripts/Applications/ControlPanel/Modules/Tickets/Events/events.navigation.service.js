define([
    'controlpanel-app'
], function (app) {
    app.service('EventsNavigation', [
        '$http',
        function ($http) {
            var eventNav = null;

            var _buildNavItems = function (id) {
                var dataID = '({id:' + id + '})';
                return [
                    { name: 'eventdashboards/:id', text: 'Dashboard', icon: 'fa-dashboard', data: dataID },
                    { name: 'events/:id', text: 'Settings', icon: 'fa-wrench', data: dataID },
                    { name: 'scheduleitems/:id', text: 'Schedule', icon: 'fa-calendar-o', data: dataID },
                    { name: 'eventfaqcategories/:id', text: 'FAQs', icon: 'fa-question-circle', data: dataID },
                    { text: 'Tickets', icon: 'fa-ticket', items: [
                        { name: 'eventtickets/:id', text: 'Ticket Setup', icon: 'fa-ticket', data: dataID },
                        { name: 'eventagegroups/:id', text: 'Ages', icon: 'fa-users', data: dataID },
                        { name: 'ticketcutoffs/:id', text: 'Prices', icon: 'fa-usd', data: dataID },
                        { name: 'ticketoptions/:id', text: 'Ticket Options', icon: 'fa-shopping-bag', data: dataID },
                            { name: 'ticketdiscounts/:id', text: 'Discounts', icon: 'fa-tags', data: dataID },
                            { name: 'reportingtickets/:id', text: 'Reports', icon: 'fa-list', data: dataID },
                    ]},

                ];
            };

            return {
                getNavigation: function (id, cb, title) {
                    if (eventNav == null || eventNav.eventID != id) {
                        if (title == null) {
                            $http.get('../api/events/lookupname/' + id).then(function (response) {
                                eventNav = {
                                    eventID: id,
                                    title: 'Manage ' + response.data,
                                    navItems: _buildNavItems(id)
                                };
                                cb(eventNav);
                            });
                        } else {
                            eventNav = { eventID: id, title: 'Manage ' + title, navItems: _buildNavItems(id) };
                            cb(eventNav);
                        }
                    } else {
                        cb(eventNav);
                    }
                },
                setActive: function (stateName) {
                    var baseState = stateName.substr(0, stateName.indexOf('/'));
                    angular.forEach(eventNav.navItems, function (nav) {
                        if (!nav.items) {
                            var navBaseState = nav.name.substr(0, nav.name.indexOf('/'));
                            if (nav.cssClass && navBaseState != baseState) {
                                nav.cssClass = '';
                            } else if (navBaseState == baseState) {
                                nav.cssClass = 'active';
                            }
                        } else {
                            //deal with nested ones
                            var found = false;
                            angular.forEach(nav.items, function (item) {
                                if (!found) {
                                    var itemBaseState = item.name.substr(0, item.name.indexOf('/'));
                                    if (baseState == itemBaseState) {
                                        found = true;
                                        item.cssClass = 'active';
                                    } else {
                                        item.cssClass = '';
                                    }
                                } else {
                                    item.cssClass = '';
                                }
                            });
                            nav.cssClass = found ? 'active' : '';
                        }
                    })
                }
            }

        }
    ]);
});