(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('NewDeliveryDocCtrl', NewDeliveryDocCtrl)
        .controller('NewLineModalCtrl', NewLineModalCtrl);

    NewDeliveryDocCtrl.$inject = ['$http', '$uibModal', 'messageService'];

    function NewDeliveryDocCtrl($http, $uibModal, messageService){
        var vm = this;
        vm.doc = {};
        vm.openNewLineModal = openNewLineModal;
        vm.guid = null;

        init();

        function init(){
            initEmptyDocument();
            getNewDocGuid();
            console.log('NewDeliveryDocCtrl init');
        }

        function initEmptyDocument(){
            vm.doc = {
                remarks: '',
                lines: []
            };
        }

        function getNewDocGuid(){
            $http.get('api/document/delivery/new')
                .then(onSuccess, onError);

            function onSuccess(response){
                vm.guid = response.data;
            }

            function onError(error){
                messageService.requestError(error);
            }
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
            modalInstance.close = addDocLine;
        }

        function addDocLine(line){
            console.log(line);
            var cmd = {
                productId: line.product.id,
                quantity: line.quantity
            };
            
            $http.post('api/document/delivery/new/'+vm.guid+'/line', cmd)
                .then(onSuccess, onError);

            function onSuccess(response){
                vm.doc.lines = response.data;
            }

            function onError(error){
                messageService.requestError(error);
            }
        }
    }


    NewLineModalCtrl.$inject = ['$http', '$uibModalInstance', 'messageService'];

    function NewLineModalCtrl($http, $uibModalInstance, messageService){
        var vm = this;
        vm.okClick = okClick;
        vm.cancelClick = cancelClick;
        vm.product = {};
        vm.products = [];
        vm.quantity = 1;
        vm.refreshProducts = refreshProducts;

        function cancelClick(){
            $uibModalInstance.dismiss('cancelClick');
        }

        function okClick(){
            var line = {
                product: vm.product,
                quantity: vm.quantity
            };
            $uibModalInstance.dismiss('okClick');
            $uibModalInstance.close(line);
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