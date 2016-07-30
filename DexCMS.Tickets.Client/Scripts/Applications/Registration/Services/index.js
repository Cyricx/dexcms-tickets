define([
    './value.service'
], function (valueService) {
    return function (app) {
        valueService(app);
    };
});