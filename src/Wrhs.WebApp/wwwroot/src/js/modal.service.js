(function() {
'use strict';

    angular
        .module('wrhs')
        .service('modalService', ModalService);

    ModalService.$inject = ['$uibModal'];
    function ModalService($uibModal) {
        this.showConfirmModal = showConfirmModal;
        
         /*
            var config = {
                title: 'some-title',
                message: 'some-message',
                onConfirm: function(){},
                onCancel: function(){}
            }
         */
        function showConfirmModal(config) {
             $uibModal.open({
                size: 'sm',
                templateUrl: 'templates/confirmModal.html',
                controller: 'ConfirmModalController',
                controllerAs: 'vm',
                resolve: {
                    confirmModalConfig: function(){
                        return config;
                    }
                }
            });
        }
    }
})();