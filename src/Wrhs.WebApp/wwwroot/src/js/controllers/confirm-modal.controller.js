(function() {
'use strict';

    angular
        .module('wrhs')
        .controller('ConfirmModalController', ConfirmModalController);

    ConfirmModalController.$inject = ['$uibModalInstance', 'confirmModalConfig'];
    function ConfirmModalController($uibModalInstance, confirmModalConfig) {
        var vm = this;
        vm.title = '';
        vm.message = '';
        vm.ok = okClick;
        vm.cancel = cancelClick;


        activate();

        function activate() {
            vm.title = confirmModalConfig.title,
            vm.message = confirmModalConfig.message
         }

         function okClick(){
             var result = confirmModalConfig.onConfirm();
             $uibModalInstance.close(result);
         }

         function cancelClick(){
             confirmModalConfig.onCancel();
             $uibModalInstance.dismiss('cancel');
         }
    }
})();