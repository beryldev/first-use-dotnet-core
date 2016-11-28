(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('DeliveryOperationCtrl', DeliveryOperationCtrl);

    DeliveryOperationCtrl.$inject = ['$stateParams', 'operationServiceFactory'];

    function DeliveryOperationCtrl($stateParams, operationServiceFactory){
        var vm = this;
        vm.service = null;

        init();

        function init(){
            var serviceConfig = {
                baseUrl: 'api/operation/delivery',
                successConfirmMessage: 'Delivery operation confirmed',
                successConfirmRedirect: 'documents.delivery',
                documentId: $stateParams.id,
                operationStep: {
                    line: {},
                    quantity: null,
                    location: ''
                }
            }
            var srv = operationServiceFactory.create(serviceConfig);
            srv.initOperation().then(function(){
                vm.service = srv;
            });

            console.log('DeliveryOperationCtrl init');
        }
    }

})();