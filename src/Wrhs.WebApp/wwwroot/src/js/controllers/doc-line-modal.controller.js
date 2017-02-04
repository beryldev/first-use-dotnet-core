(function(){
    'use strict';

     angular
        .module('wrhs')
        .controller('DocLineModalCtrl', DocLineModalCtrl);

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
                pageSize: 10
            };

            $http.get('api/product', {params: filter})
                .then(onSuccess);

            function onSuccess(response){
                vm.products = response.data.items;
            }
        }
    }

})();