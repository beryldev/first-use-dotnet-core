(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('RelocationOperationCtrl', RelocationOperationCtrl);

    RelocationOperationCtrl.$inject = ['$stateParams', 'operationServiceFactory'];

    function RelocationOperationCtrl($stateParams, operationServiceFactory){
        var vm = this;
        vm.service = null;

        init();

        function init(){
            var serviceConfig = {
                baseUrl: 'api/operation/relocation',
                successConfirmMessage: 'Relocation operation confirmed',
                successConfirmRedirect: 'documents.relocation',
                documentId: $stateParams.id,
                operationStep: {
                    line: {},
                    quantity: null,
                    from: '',
                    to: ''
                }
            }
            var srv = operationServiceFactory.create(serviceConfig);
            
            srv.initOperation().then(function(){
                vm.service = srv;
            });
            
            console.log('RelocationOperationCtrl init');
        }
    }
})();