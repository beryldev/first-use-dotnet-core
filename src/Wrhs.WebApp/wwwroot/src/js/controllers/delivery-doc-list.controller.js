(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('DeliveryDocListCtrl', DeliveryDocListCtrl);

    DeliveryDocListCtrl.$inject = ['$scope', '$http', 'messageService'];

    function DeliveryDocListCtrl($scope, $http, messageService){
        var vm = this;
        vm.filter = {
            page: 1,
            perPage: 10
        };

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
                { name: 'fullNumber', displayName: 'Number'},
                { name: 'issueDate', displayName: 'Issue date'},
                { name: 'remarks'}
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
            //appScopeProvider: $scope.myAppScopeProvider,
            //rowTemplate: '<div ng-dblclick=\'grid.appScope.showInfo(row)\' ng-repeat=\'(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\' class=\'ui-grid-cell\' ng-class=\'{ "ui-grid-row-header-cell": col.isRowHeader }\' ui-grid-cell></div>'
        }

        init();

        function init(){
            console.log('DeliveryDocListCtrl init');
        }

        function loadData(){
            $http.get('api/document/delivery', { params: vm.filter})
                .then(onSuccess, onFail);

            function onSuccess(response){
                console.log(response.data);
            }

            function onFail(error){
                messageService.requestError(error);
            }
        }
    }
})();