(function(){
    "use strict"

    angular
        .module("wrhs")
        .controller("ProductsListCtrl", ProductsListCtrl);

    ProductsListCtrl.$inject = ['$scope', '$http', 'uiGridConstants'];

    function ProductsListCtrl($scope, $http, uiGridConstants){
        var vm = this;
        vm.filter = {
            page: 1,
            perPage: 10
        };

        vm.gridConfig = {
            data: loadData(),
            enableFiltering: true,
            useExternalFiltering: true,
            useExternalPagination: true,
            paginationPageSizes: [10, 25, 50, 75],
            paginationPageSize: 25,
            columnDefs: [
                { name: 'code'},
                { name: 'name'},
                { name: 'ean', displayName: "EAN"},
                { name: 'sku', displayName: "SKU"},
                { name: 'description'}
            ],
            onRegisterApi: function(gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.filterChanged($scope, function(){
                    var grid = this.grid;
                    grid.columns.forEach(function(col){
                        if(col.filters[0].term !== undefined && 
                            col.filters[0].term !== null && 
                            col.filters[0].term.length > 0){
                                vm.filter[col.name] = col.filters[0].term;
                            }
                    });

                    loadData(); 
                });

                gridApi.pagination.on.paginationChanged($scope, function(newPage, pageSize){
                    vm.filter.page = newPage;
                    vm.filter.perPage = pageSize;
                    loadData();
                });
            }
        }
        
        init();

        function init(){
            vm.data =

            console.log("ProductsListCtrl init");
        }

        function loadData(){
            $http.get('api/product', {params: vm.filter})
                .then(function(response){
                    var data = response.data;
                    vm.gridConfig.data = data.items;
                    vm.gridConfig.totalItems = data.total;
                });
        }
    }
})();