(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('NewDeliveryDocCtrl', NewDeliveryDocCtrl)
        .controller('DocLineModalCtrl', DocLineModalCtrl);

    NewDeliveryDocCtrl.$inject = ['$http', '$uibModal', 'messageService'];

    function NewDeliveryDocCtrl($http, $uibModal, messageService){
        var vm = this;
        vm.doc = {};
        vm.addDocLineModal = addDocLineModal;
        vm.deleteDocLine = deleteDocLine;
        vm.changeDocLineModal = changeDocLineModal;
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
                .then(onSuccess);

            function onSuccess(response){
                vm.guid = response.data;
            }
        }

        function addDocLine(line){
            var cmd = {
                productId: line.product.id,
                quantity: line.quantity
            };
            
            $http.post('api/document/delivery/new/'+vm.guid+'/line', cmd)
                .then(onSuccess);

            function onSuccess(response){
                vm.doc.lines = response.data;
            }
        }

        function updateDocLine(line){
            console.log('update', line);
            var config = {
                data: line,
                headers: {'Content-Type': 'application/json'}
            };
            $http.put('api/document/delivery/new/'+vm.guid+'/line', line)
                .then(onSuccess);

            function onSuccess(response){
                vm.doc.lines = response.data;
            }
        }

        function deleteDocLine(line){
            var config = {
                data: line,
                headers: {'Content-Type': 'application/json'}
            };

            $http.delete('api/document/delivery/new/'+vm.guid+'/line', config)
                .then(onSuccess);
            
            function onSuccess(response){
                vm.doc.lines = response.data;
            }
        }

        function addDocLineModal(){
            openLineModal({ title: 'New line', line: {quantity: 1} }, addDocLine);
        }

        function changeDocLineModal(line){
            openLineModal({ title: 'Change line', line: line }, updateDocLine);
        }

        function openLineModal(content, closeCallback){
            var modalInstance = $uibModal.open({
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'myModalContent.html',
                controller: 'DocLineModalCtrl',
                controllerAs: 'vm',
                resolve: {
                    content: function() { return content; }
                }
                //size: size,
                //appendTo: parentElem,        
            });
            modalInstance.close = closeCallback;
        }
    }


    DocLineModalCtrl.$inject = ['$http', '$uibModalInstance', 'messageService', 'content'];

    function DocLineModalCtrl($http, $uibModalInstance, messageService, content){
        var vm = this;
        vm.title = '';
        vm.okClick = okClick;
        vm.cancelClick = cancelClick;
        vm.product = {};
        vm.products = [];
        vm.quantity = 1;
        vm.refreshProducts = refreshProducts;
        vm.lp = null;

        init();

        function init(){
            console.log('content', content);
            vm.title = content.title;
            vm.product = content.line.product;
            vm.quantity = content.line.quantity;
            vm.lp = content.line.lp ? content.line.lp : 0;
        }

        function cancelClick(){
            $uibModalInstance.dismiss('cancelClick');
        }

        function okClick(){
            var line = {
                product: vm.product,
                quantity: vm.quantity,
                lp: vm.lp
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
                .then(onSuccess);

            function onSuccess(response){
                vm.products = response.data.items;
            }
        }
    }
})();