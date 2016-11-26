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
            var operationStep = {
                line: {},
                quantity: null,
                from: '',
                to: ''
            };
            var srv = operationServiceFactory
                .create('api/operation/relocation', $stateParams.id, operationStep);
            
            srv.initOperation().then(function(){
                vm.service = srv;
            });
            console.log('RelocationOperationCtrl init');
        }
    }
})();