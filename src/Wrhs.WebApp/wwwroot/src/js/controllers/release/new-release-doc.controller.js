(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('NewReleaseDocCtrl', NewReleaseDocCtrl);

    NewReleaseDocCtrl.$inject = ['$http', 'newDocServiceFactory'];

    function NewReleaseDocCtrl($http, newDocServiceFactory){
        var vm = this;
        vm.service = null;
        vm.openNewLineModal = null;
        vm.openChangeLineModal = null;
        vm.removeLine = null;
        vm.saveDocument = null;

        init();

        function init(){
            initDocService();
            console.log('NewReleaseDocCtrl init');
        }

        function initDocService(){
            var config = {
                documentModel: {
                    remarks: '',
                    lines: []
                },
                baseUrl: 'api/document/release',
                docLineModalTemplateUrl: 'templates/release/releaseDocLineModal.html',
                lineToCmd: lineToCmd
            }

            vm.service = newDocServiceFactory.createService(config);
            vm.service.initNewDoc();
            vm.openNewLineModal = openNewLineModal;
            vm.openChangeLineModal = vm.service.openChangeLineModal;
            vm.removeLine = vm.service.removeLine;
            vm.saveDocument = saveDocument;
        }

        function lineToCmd(line){
            return {
                lp: line.lp,
                productId: line.product.id,
                quantity: line.quantity,
                location: line.location
            };
        }

        function openNewLineModal(){
            var lineModel = {
                lp: 0,
                product: null,
                quantity: 1,
                location: ''
            }

            vm.service.openNewLineModal(lineModel);
        }

        function saveDocument(){
            vm.service.save('documents.release');
        }
    }
})();