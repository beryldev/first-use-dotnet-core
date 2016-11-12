(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('NewDeliveryDocCtrl', NewDeliveryDocCtrl)
        .controller('DocLineModalCtrl', DocLineModalCtrl);

    NewDeliveryDocCtrl.$inject = ['$http', '$uibModal', '$state', 'messageService', 'newDocServiceFactory'];

    function NewDeliveryDocCtrl($http, $uibModal, $state, messageService, newDocServiceFactory){
        var vm = this;
        vm.doc = {};
        vm.service = null;
        vm.openNewLineModal = null;
        vm.openChangeLineModal = null;
        vm.removeLine = null;
        vm.saveDocument = null;

        

        init();

        function init(){
            initDocService();
            console.log('NewDeliveryDocCtrl init');
        }

        function initDocService(){
            var config = {
                documentModel: {
                    remarks: '',
                    lines: []
                },
                baseUrl: 'api/document/delivery',
                docLineModalTemplateUrl: 'templates/delivery/deliveryDocLineModal.html',
                lineToCmd: lineToCmd
            };

            vm.service = newDocServiceFactory.createService(config);
            vm.service.initNewDoc().then(function(){
                vm.doc = vm.service.document;
                vm.openNewLineModal = openNewLineModal;
                vm.openChangeLineModal = vm.service.openChangeLineModal;
                vm.removeLine = vm.service.removeLine;
                vm.saveDocument = saveDocument;
            });

            function lineToCmd(line){
                return {
                    lp: line.lp,
                    productId: line.product.id,
                    quantity: line.quantity
                };
            }
        }

        function openNewLineModal(){
             var lineModel = {
                    lp: 0,
                    product: null,
                    quantity: 1
                };

            vm.service.openNewLineModal(lineModel);
        }

        function saveDocument(){
            vm.service.save('documents.delivery');
        }
    }


    DocLineModalCtrl.$inject = ['$http', '$uibModalInstance', 'messageService', 'content', 'lineModel'];

    function DocLineModalCtrl($http, $uibModalInstance, messageService, content, lineModel){
        var vm = this;
        vm.title = '';
        vm.okClick = okClick;
        vm.cancelClick = cancelClick;
        vm.products = [];
        vm.refreshProducts = refreshProducts;
        vm.model = null;

        init();

        function init(){
            console.log('content', content);
            vm.title = content.title;
            vm.model = lineModel;
        }

        function cancelClick(){
            $uibModalInstance.dismiss('cancelClick');
        }

        function okClick(){
            $uibModalInstance.dismiss('okClick');
            $uibModalInstance.close(vm.model);
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