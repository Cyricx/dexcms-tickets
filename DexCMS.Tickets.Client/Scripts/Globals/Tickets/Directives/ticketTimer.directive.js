define([
    '../tickets',
    'angular',
    'moment',
], function (module, angular, moment) {
    module.directive('ttcmsTicketTimer', [
        'dexCMSGlobalsTicketsSettings',
        function (Settings) {
            return {
                templateUrl: Settings.startingRoute + 'directives/_ticketTimer.html',
                restrict: 'E',
                replace: true,
                controller: ['$scope', 'Tickets', '$interval', '$attrs', function ($scope, Tickets, $interval, $attrs) {
                    $scope.nextExpiration = Tickets.getNextExpiration();
                    $scope.resetExpirations = function () {
                        Tickets.resetExpirations();
                    };

                    $scope.getTimeClass = function (timeLeft) {
                        if (timeLeft > 600000) {
                            //more than 10 minutes
                            return '';
                        } else if (timeLeft > 300000) {
                            //more than 5 minutes
                            return 'warning';
                        } else if (timeLeft > 0) {
                            return 'danger';
                        }
                    }
                }]
            };
        }]
    );
});