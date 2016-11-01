(function(){
    "use strict"

    angular
        .module("wrhs")
        .controller("ProductsListCtrl", ProductsListCtrl);

    ProductsListCtrl.$inject = ['$scope', '$http'];

    function ProductsListCtrl($scope, $http){
        var vm = this;

        vm.gridConfig = {
            data: getData(),
            enableFiltering: true,
            useExternalFiltering: true,
            columnDefs: [
                { name: 'code'},
                { name: 'name'},
                { name: 'ean'},
                { name: 'sku'},
                { name: 'description'}
            ],
            onRegisterApi: function(gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.filterChanged($scope, function(){
                    var grid = this.grid;
                    var filter = {};
                    grid.columns.forEach(function(col){
                        if(col.filters[0].term !== undefined && 
                            col.filters[0].term !== null && 
                            col.filters[0].term.length > 0){
                                filter[col.name] = col.filters[0].term;
                            }
                    });

                    loadData(filter); 
                });
            }
        }
        

        init();

        function init(){
            vm.data =

            console.log("ProductsListCtrl init");
        }

        function loadData(filter){
            $http.get('api/product', {params: filter})
                .then(function(response){
                    console.log(response.data);
                });
        }

        function getData(){
            return  [
                {
                    code: "PROD1",
                    name: "Product 1",
                    ean: "11111111",
                    sku: "111",
                    description: "some desc"
                },
                {
                    code: "PROD2",
                    name: "Product 2",
                    ean: "11111112",
                    sku: "112",
                    description: "some desc"
                }
            ];
        }
    }
})();