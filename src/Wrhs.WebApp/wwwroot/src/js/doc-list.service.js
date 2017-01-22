(function(){
    'use strict'

    angular
        .module('wrhs')
        .factory('documentListFactory', documentListFactory);

    documentListFactory.$inject = ['$http'];

    function documentListFactory($http){
        var factory = {
            createService: createService
        };

        return factory;

        function createService($scope, documentUrl, onRowDoubleClick){
            var service = {
                gridConfig: {},
                filter: {},
                loadData: loadData
            }

            initService();

            return service;

            function initService(){
                $scope.myAppScopeProvider = {
                    showInfo : function(row) {
                        onRowDoubleClick(row);
                    }
                }
                service.gridConfig = gridConfig();
                service.filter = filter();    
            }

            function loadData(){
                $scope.documentsBusy = $http.get(documentUrl, { params: service.filter})
                    .then(onSuccess);

                function onSuccess(response){
                    service.gridConfig.data = response.data.items;
                    service.gridConfig.totalItems = response.data.total;
                }
            }

           
            function gridConfig(){
                return  {
                    data: service.loadData(),
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
                        { name: 'issueDate', displayName: 'Issue date', type: 'date',  cellFilter: 'date:\'yyyy-MM-dd\'' },
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
                                        service.filter[col.name] = col.filters[0].term;
                                    } else {
                                        service.filter[col.name] = '';
                                    }
                            });

                            service.loadData(); 
                        });

                        gridApi.pagination.on.paginationChanged($scope, function(newPage, pageSize){
                            service.filter.page = newPage;
                            service.filter.perPage = pageSize;
                            service.loadData();
                        });
                    },
                    appScopeProvider: $scope.myAppScopeProvider,
                    rowTemplate: '<div ng-dblclick=\'grid.appScope.showInfo(row)\' ng-repeat=\'(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\' class=\'ui-grid-cell\' ng-class=\'{ "ui-grid-row-header-cell": col.isRowHeader }\' ui-grid-cell></div>'
                }
            }

            function filter(){
                return {
                    page: 1,
                    perPage: 10
                };
            }
        }
    }
})();