define([
    './addticket.directive',
    './adddiscount.directive',
    './listtickets.directive'
], function (addTicket, addDiscount, listTickets) {
    return function (app) {
        addTicket(app);
        addDiscount(app);
        listTickets(app);
    };
});