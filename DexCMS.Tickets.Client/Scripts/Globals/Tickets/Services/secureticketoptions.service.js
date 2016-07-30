define([
    '../tickets'
], function (module) {
    module.service('SecureTicketOptions', ['$http', function ($http) {

        var baseUrl = '../../../api/secureticketoptions';

        return {
            getOptionDetails: function (optionID, choiceID, discountID, discountConfirmation) {
                var params = {
                    choiceID: choiceID
                };
                if (discountID) {
                    params.discountID = discountID;
                }
                if (discountConfirmation) {
                    params.discountConfirmation = discountConfirmation;
                }
                return $http.get(baseUrl + '/' + optionID, { params: params });
            }
        }
    }]);
});