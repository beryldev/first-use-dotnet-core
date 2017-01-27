(function(){
    'use strict';

    angular
        .module('wrhs')
        .controller('EditDeliveryDocCtrl', EditDeliveryDocCtrl);

    EditDeliveryDocCtrl.$inject = ['$http', '$stateParams', '$scope'];

    function EditDeliveryDocCtrl($http, $stateParams, $scope){
        var vm = this;
        vm.document = {};
        vm.gridConfig = {};
        vm.selectedLine = null;
        vm.addLine = addLine;
        vm.removeSelected = removeSelected;

        init();

        function init(){
            $scope.myAppScopeProvider = {
                showInfo : function(row) {
                    console.log(row);
                },
                selectRow: function(row){
                    vm.selectedLine = row.entity;
                }
            }
            vm.gridConfig = getGridConfig();
            console.log('EditDeliveryDocCtrl init');
        }

        function getGridConfig(){
            return {
                data: loadData(),
                multiSelect: false,
                enableSelectAll: false,
                enableRowSelection: true,
                noUnselect: true,
                enableRowHeaderSelection: false,
                enableFiltering: false,
                columnDefs: [
                    { name: 'product.name', displayName: 'Product name', enableColumnMenu: false},
                    { name: 'product.ean', displayName: 'EAN', enableColumnMenu: false},
                    { name: 'quantity', displayName: 'Quantity', enableColumnMenu: false},
                    { name: 'dstLocation', displayName: 'Dst location', enableColumnMenu: false}
                ],
                appScopeProvider: $scope.myAppScopeProvider,
                rowTemplate: '<div ng-click=\'grid.appScope.selectRow(row)\' ng-dblclick=\'grid.appScope.showInfo(row)\' ng-repeat=\'(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\' class=\'ui-grid-cell\' ng-class=\'{ "ui-grid-row-header-cell": col.isRowHeader }\' ui-grid-cell></div>'

            }
        }

        function loadData(){
            $http.get('/api/document/'+$stateParams.id)
                .then(successCallback);

            function successCallback(resp){
                vm.document = resp.data; 
                vm.gridConfig.data = resp.data.lines;             
            }
        }

        function addLine(){
            vm.document.lines.push({
                product: {
                    id: 99,
                    name: 'some prod'
                },
                quantity: 999,
                dstLocation: 'LOC-001-02'
            });
        }

        function removeSelected(){
            if(vm.selectedLine){
                vm.document.lines
                    .splice(vm.document.lines.indexOf(vm.selectedLine), 1);
                vm.selectedLine = null;
            }
            
        }
    }

})();