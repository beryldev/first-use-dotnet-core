(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('NewRelocationDocCtrl', NewRelocationDocCtrl);

    NewRelocationDocCtrl.$inject = ['$http', 'newDocServiceFactory'];

    function NewRelocationDocCtrl($http, newDocServiceFactory){
        var vm = this;
        vm.service = null;
        vm.openNewLineModal = null;
        vm.openChangeLineModal = null;
        vm.removeLine = null;
        vm.saveDocument = null;

        init();

        function init(){            
            initDocService();         
            console.log('NewRelocationDocCtrl init');
        }

        function initDocService(){
            var config = {
                documentModel: {
                    remarks: '',
                    lines: []
                },
                baseUrl: 'api/document/relocation',
                docLineModalTemplateUrl: 'templates/relocation/relocationDocLineModal.html',
                lineToCmd: lineToCmd
            };

            vm.service = newDocServiceFactory.createService(config);
            vm.service.initNewDoc();
            vm.openNewLineModal = openNewLineModal;
            vm.openChangeLineModal = vm.service.openChangeLineModal;
            vm.removeLine = vm.service.removeLine;
            vm.saveDocument = saveDocument;

            function lineToCmd(line){
                return {
                    lp: line.lp,
                    productId: line.product.id,
                    quantity: line.quantity,
                    from: line.from,
                    to: line.to
                };
            }
        }

        function openNewLineModal(){
             var lineModel = {
                    lp: 0,
                    product: null,
                    quantity: 1,
                    from: '',
                    to: ''
                };

            vm.service.openNewLineModal(lineModel);
        }

        function saveDocument(){
            vm.service.save('documents.relocation');
        }

    }
    
})();