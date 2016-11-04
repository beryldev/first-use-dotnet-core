(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('NewDeliveryDocCtrl', NewDeliveryDocCtrl)
        .controller('NewLineModalCtrl', NewLineModalCtrl);

    NewDeliveryDocCtrl.$inject = ['$http', '$uibModal'];

    function NewDeliveryDocCtrl($http, $uibModal){
        var vm = this;
        vm.doc = {};
        vm.openNewLineModal = openNewLineModal;

        init();

        function init(){
            initEmptyDocument();
            console.log('NewDeliveryDocCtrl init');
        }

        function initEmptyDocument(){
            vm.doc = {
                remarks: '',
                lines: []
            };
        }

        function openNewLineModal(){
            var modalInstance = $uibModal.open({
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'myModalContent.html',
                controller: 'NewLineModalCtrl',
                controllerAs: 'vm',
                //size: size,
                //appendTo: parentElem,        
            });
        }
    }


    NewLineModalCtrl.$inject = ['$http', '$uibModalInstance', 'messageService'];

    function NewLineModalCtrl($http, $uibModalInstance, messageService){
        var vm = this;
        vm.cancel = cancel;
        vm.product = {};
        vm.products = [];
        vm.refreshProducts = refreshProducts;

        function cancel(){
            $uibModalInstance.dismiss('cancel');
        }

        function refreshProducts(value){
            var filter = {
                name: value,
                page: 1,
                perPage: 10
            };

            $http.get('api/product', {params: filter})
                .then(onSuccess, onError);

            function onSuccess(response){
                vm.products = response.data.items;
            }

            function onError(error){
                messageService.requestError(error);
            }
        }

    }
})();