(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('DeliveryOperationCtrl', DeliveryOperationCtrl);

    DeliveryOperationCtrl.$inject = ['$stateParams', '$state', '$http', 'messageService'];

    function DeliveryOperationCtrl($stateParams, $state, $http, messageService){
        var vm = this;
        vm.guid = '';
        vm.state = {};
        vm.allocation = {};
        vm.allocateLine = allocateLine;
        vm.saveAllocation = saveAllocation;
        vm.confirmOperation = confirmOperation;

        init();

        function init(){
            initState();
            vm.quantityFocus = false;
            console.log('DeliveryOperationCtrl init');
        }

        function initState(){
            $http.get('api/operation/delivery/new/'+$stateParams.id)
                .then(onSuccess)

            function onSuccess(response){
                vm.guid = response.data;
                getState();
            }
        }

        function getState(){
            $http.get('api/operation/delivery/'+vm.guid)
                .then(onSuccess);

            function onSuccess(response){
                vm.state = response.data;
            }
        }

        function allocateLine(line){
            vm.allocation.line = line;
            vm.quantityFocus = true;
        }

        function saveAllocation(){
            $http.post('api/operation/delivery/'+vm.guid+'/allocation', vm.allocation)
                .then(onSuccess);

            function onSuccess(response){
                vm.state = response.data;
                vm.allocation = {};
                vm.quantityFocus = false;
                messageService.success('', 'Done');          
            }  
        }

        function confirmOperation(){
            $http.post('api/operation/delivery/'+vm.guid)
                .then(onSuccess);

            function onSuccess(response){
                messageService.success('Delivery operation confirmed', 'Success');
                $state.go('documents.delivery');
            }
        }
    }

})();