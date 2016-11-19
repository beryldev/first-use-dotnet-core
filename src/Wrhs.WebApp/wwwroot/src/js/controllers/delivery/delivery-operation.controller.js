(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('DeliveryOperationCtrl', DeliveryOperationCtrl);

    DeliveryOperationCtrl.$inject = ['$stateParams', '$http', '$scope'];

    function DeliveryOperationCtrl($stateParams, $http, $scope){
        var vm = this;
        vm.guid = '';
        vm.state = {};
        vm.allocation = {};
        vm.allocateLine = allocateLine;
        vm.saveAllocation = saveAllocation;

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
            vm.quantityFocus = false;
        }
    }

})();