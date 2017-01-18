(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('StocksListCtrl', StocksListCtrl);

    StocksListCtrl.$inject = ['$http'];

    function StocksListCtrl($http){
        var vm = this;
        vm.gridConfig = null;

        init();

        function init(){
            setupGrid();
            console.log('StocksListCtrl init');
        }

        function setupGrid(){
            vm.gridConfig = {
                data: loadData(), 
                enableFiltering: true,
                columnDefs: [
                    { name: 'location'},
                    { name: 'product.name', displayName: 'Product name'},
                    { name: 'product.code', displayName: 'Product code'},
                    { name: 'quantity'},
                ],
            }
        }

        function loadData(){
            $http.get('api/stock')
                .then(function(response){
                    var data = response.data.items;
                    vm.gridConfig.data = data;
                });
        }
    }
})();