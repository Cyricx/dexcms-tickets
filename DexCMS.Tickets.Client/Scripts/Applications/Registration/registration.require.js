var libs = '../../../../libs/';
require.config({
    urlArgs: "v=2016.08.14.15.34.22",
    waitSeconds: 120,
    paths: {
        'registration-app': './registration',
        'dexcms-globals-base': '../../globals/base/index',
        'dexcms-globals-shared': '../../globals/shared/index',
        'dexcms-globals-tickets': '../../globals/tickets/index',
        'angular': libs + 'angular/angular.min',
        'angular-sanitize': libs + 'angular-sanitize/angular-sanitize.min',
        'angular-smart-table': libs + 'angular-smart-table/smart-table.min',
        'angular-ui-router': libs + 'angular-ui-router/angular-ui-router.min',
        'angular-resource': libs + 'angular-resource/angular-resource.min',
        'jquery': libs + 'jquery/jquery.min',
        'json': libs + 'requirejs-plugins/json',
        'text': libs + 'text/text',
        'moment': libs + 'moment/moment.min',
        'ngStorage': libs + 'ngstorage/ngStorage.min',
        'ngtoast': libs + 'ngtoast/ngtoast.min',
        "ng-csv": libs + 'ng-csv/ng-csv.min',
        'oclazyload': libs + 'oclazyload/ocLazyLoad.require.min',
        'tg-angular-validator': libs + 'tg-angular-validator/angular-validator.min',
        'underscore': libs + 'underscore/underscore-min'
    },
    shim: {
        'angular': {
            exports: 'angular',
            deps: ['jquery']
        },
        'ngStorage': {
            deps: ['angular']
        },
        'angular-resource': {
            deps: ['angular']
        },
        'angular-route': {
            deps: ['angular']
        },
        'angular-sanitize': {
            deps: ['angular']
        },
        'angular-smart-table': {
            deps: ['angular']
        },
        'angular-ui-router': {
            deps: ['angular']
        },
        'jquery': {
            exports: '$'
        },
        'ngtoast': {
            deps: ['angular']
        },
        'ng-csv': {
            deps: ['angular', 'angular-sanitize']
        },
        'oclazyload': {
            deps: ['angular']
        },
        'tg-angular-validator': {
            deps: ['angular']
        },
        'underscore': {
            exports: "_"
        }
    },
    deps: ['./registration.bootstrap']
});