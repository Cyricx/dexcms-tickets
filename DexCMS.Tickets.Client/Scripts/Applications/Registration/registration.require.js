var libs = '../../../../libs/';
require.config({
    urlArgs: "v=1.6.2",
    waitSeconds: 120,
    paths: {
        'registration-app': './registration',
        'dexcms-globals-base': '../../globals/base/index',
        'dexcms-globals-shared': '../../globals/shared/index',
        'dexcms-globals-tickets': '../../globals/tickets/index',
        'angular': libs + 'angular/angular.min',
        'angular-sanitize': libs + 'angular-sanitize/angular-sanitize.min',
        'angular-ui-router': libs + 'angular-ui-router/angular-ui-router.min',
        'angular-resource': libs + 'angular-resource/angular-resource.min',
        'jquery': libs + 'jquery/jquery.min',
        'json': libs + 'requirejs-plugins/json',
        'text': libs + 'text/text',
        'moment': libs + 'moment/moment.min',
        'ngStorage': libs + 'ngstorage/ngStorage.min',
        'ngtoast': libs + 'ngtoast/ngtoast.min',
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
        'angular-ui-router': {
            deps: ['angular']
        },
        'jquery': {
            exports: '$'
        },
        'ngtoast': {
            deps: ['angular']
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