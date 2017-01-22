(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('ProductsListCtrl', ProductsListCtrl);

    ProductsListCtrl.$inject = ['$scope', '$http', '$state', 'uiGridConstants'];

    function ProductsListCtrl($scope, $http, $state, uiGridConstants){
        var vm = this;
        vm.filter = {
            page: 1,
            perPage: 10
        };

        $scope.myAppScopeProvider = {
            showInfo : function(row) {
                $state.go('products.details', {id: row.entity.id});
            }
        }

        vm.gridConfig = {
            data: loadData(),
            multiSelect: false,
            enableSelectAll: false,
            enableRowSelection: true,
            noUnselect: true,
            enableRowHeaderSelection: false,
            enableFiltering: true,
            useExternalFiltering: true,
            useExternalPagination: true,
            paginationPageSizes: [10, 25, 50, 75],
            paginationPageSize: 10,
            columnDefs: [
                { name: 'code'},
                { name: 'name'},
                { name: 'ean', displayName: 'EAN'},
                { name: 'sku', displayName: 'SKU'},
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
                            } else {
                                vm.filter[col.name] = '';
                            }
                    });

                    loadData(); 
                });

                gridApi.pagination.on.paginationChanged($scope, function(newPage, pageSize){
                    vm.filter.page = newPage;
                    vm.filter.perPage = pageSize;
                    loadData();
                });
            },
            appScopeProvider: $scope.myAppScopeProvider,
            rowTemplate: '<div ng-dblclick=\'grid.appScope.showInfo(row)\' ng-repeat=\'(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\' class=\'ui-grid-cell\' ng-class=\'{ "ui-grid-row-header-cell": col.isRowHeader }\' ui-grid-cell></div>'
        }
        
        init();

        function init(){
            vm.data =

            console.log('ProductsListCtrl init');
        }

        function loadData(){
            vm.productsBusy = $http.get('api/product', {params: vm.filter})
                .then(function(response){
                    var data = response.data;
                    vm.gridConfig.data = data.items;
                    vm.gridConfig.totalItems = data.total;
                });
        }
    }
})();