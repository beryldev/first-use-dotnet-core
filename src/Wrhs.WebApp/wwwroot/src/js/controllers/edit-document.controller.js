(function(){
    'use strict';

    angular
        .module('wrhs')
        .controller('EditDocumentCtrl', EditDocumentCtrl);

    EditDocumentCtrl.$inject = ['$scope', '$state', '$stateParams', 'documentServiceFactory', 'config'];

    function EditDocumentCtrl($scope, $state, $stateParams, documentServiceFactory, config){
        var vm = this;
        vm.document = {};
        vm.gridConfig = {};
        vm.selectedLine = null;
        vm.service = null;
        vm.goBack = goBack;
        vm.beginOperation = beginOperation;

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

            vm.service = documentServiceFactory.createService(config.docServiceConfig, $scope);
            
            vm.gridConfig = getGridConfig();
            console.log('EditDocumentCtrl init');
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
                columnDefs: config.columnDefs,
                appScopeProvider: $scope.myAppScopeProvider,
                rowTemplate: '<div ng-click=\'grid.appScope.selectRow(row)\' ng-dblclick=\'grid.appScope.showInfo(row)\' ng-repeat=\'(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\' class=\'ui-grid-cell\' ng-class=\'{ "ui-grid-row-header-cell": col.isRowHeader }\' ui-grid-cell></div>'

            }
        }

        function loadData(){
            $scope.documentBusy = vm.service.getDocument($stateParams.id)
                .then(successCallback);

            function successCallback(){
                vm.document = vm.service.document; 
                vm.gridConfig.data = vm.document.lines;         
            }
        }

        function goBack(){
            $state.go(config.docServiceConfig.goBackState);
        }

        function beginOperation(){
            $state.go(config.beginOperationState, {id: vm.document.id});
        }
    }

})();