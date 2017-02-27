(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('NewDocumentController', NewDocumentController);

    NewDocumentController.$inject = ['$scope', '$state', '$uibModal', 'documentServiceFactory', 'config'];

    function NewDocumentController($scope, $state, $uibModal, documentServiceFactory, config){
        var vm = this;
        vm.service = null;
        vm.gridConfig = {};
        vm.selectedLine = {};
        vm.goBack = goBack;


        init();

        function init(){
            initDocService();
            initGrid();
            console.log('NewDocumentController init');
        }

        function initDocService(){
            vm.service = documentServiceFactory
                .createService(config.docServiceConfig, $scope);    
        }

        function initGrid(){
             $scope.myAppScopeProvider = {
                showInfo : function(row) {
                    //console.log(row);
                },
                selectRow: function(row){
                    vm.selectedLine = row.entity;
                }
            }

            vm.gridConfig = {
                data: vm.service.document.lines,
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

        function goBack(){
            $state.go(config.docServiceConfig.goBackState);
        }   
    }

})();