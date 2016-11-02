(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('ProductDetailsCtrl', ProductDetailsCtrl);

    ProductDetailsCtrl.$inject = ['$scope', '$state', '$stateParams', '$http', 'messageService'];

    function ProductDetailsCtrl($scope, $state, $stateParams, $http, messageService){
        var vm = this;
        vm.product = {};
        vm.update = updateProduct;
        vm.stocksSelect = stocksSelect;
         vm.gridConfig = {
            data: loadData(), 
            enableFiltering: false,
            columnDefs: [
                { name: 'location'},
                { name: 'quantity'},
            ],
        }

        init();

        function init(){
            getProduct($stateParams.id);
            console.log('ProductDetailsCtrl init');
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
                messageService.success('', 'Changes saved');
            }

            function onError(error){
                messageService.requestError(error);
            }
        }

        function stocksSelect(){
            loadData();
        }

         function loadData(){
            $http.get('api/product/'+$stateParams.id+'/stocks')
                .then(function(response){
                    var data = response.data;
                    vm.gridConfig.data = data;
                });
        }
    }
})();