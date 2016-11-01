(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('ProductDetailsCtrl', ProductDetailsCtrl);

    ProductDetailsCtrl.$inject = ['$state', '$stateParams', '$http', 'messageService'];

    function ProductDetailsCtrl($state, $stateParams, $http, messageService){
        var vm = this;
        vm.product = {};
        vm.update = updateProduct;

        init();

        function init(){
            getProduct($stateParams.id);
            console.log('ProductDetailsCtrl init', $stateParams.id);
        }

        function getProduct(id){
            $http.get('api/product/'+id)
                .then(onSuccess, onError);

            function onSuccess(response){
                vm.product = response.data;
            }

            function onError(error){
                messageService.requestError(error);
                $state.go('products');
            }
        }

        function updateProduct(){
            $http.put('api/product/'+$stateParams.id, vm.product)
                .then(onSuccess, onError);

            function onSuccess(response){
                messageService.success("", "Changes saved");
            }

            function onError(error){
                messageService.requestError(error);
            }
        }
    }
})();