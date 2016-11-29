(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('ReleaseOperationCtrl', ReleaseOperationCtrl);

    ReleaseOperationCtrl.$inject = ['$stateParams', 'operationServiceFactory'];

    function ReleaseOperationCtrl($stateParams, operationServiceFactory){
        var vm = this;
        vm.service = null;

        init();

        function init(){
            var serviceConfig = {
                baseUrl: 'api/operation/release',
                successConfirmMessage: 'Release operation confirmed',
                successConfirmRedirect: 'documents.release',
                documentId: $stateParams.id,
                operationStep: {
                    line: {},
                    quantity: null,
                    location: ''
                }
            };
            var srv = operationServiceFactory.create(serviceConfig);
            srv.initOperation().then(function(){
                vm.service = srv;
                console.log('ReleaseOperationCtrl init');
            });        
        }
    }
})();