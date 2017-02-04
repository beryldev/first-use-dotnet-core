(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('NewDeliveryDocCtrl', NewDeliveryDocCtrl)
        .controller('DocLineModalCtrl', DocLineModalCtrl);

    NewDeliveryDocCtrl.$inject = ['$scope', '$http', '$uibModal', '$state', 'messageService', 'documentServiceFactory'];

    function NewDeliveryDocCtrl($scope, $http, $uibModal, $state, messageService, documentServiceFactory){
        var vm = this;
        vm.service = null;
        vm.saveDocument = null;
        vm.gridConfig = {};
        vm.selectedLine = {};


        init();

        function init(){
            initDocService();
            console.log('NewDeliveryDocCtrl init');
        }

        function initDocService(){
            var config = {
                baseUrl: 'api/document/delivery',
                go
                docLineModalTemplateUrl: 'templates/delivery/deliveryDocLineModal.html'
            };

            vm.service = documentServiceFactory.createService(config);
            vm.saveDocument = saveDocument;

             $scope.myAppScopeProvider = {
                showInfo : function(row) {
                    console.log(row);
                },
                selectRow: function(row){
                    vm.selectedLine = row.entity;
                    console.log(vm.selectedLine);
                }
            }
            vm.gridConfig = getGridConfig();
        }

        function saveDocument(){
            vm.service.save('documents.delivery');
        }

        function getGridConfig(){
            return {
                data: vm.service.lines,
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
    }


    DocLineModalCtrl.$inject = ['$http', '$uibModalInstance', 'messageService', 'content', 'lineModel'];

    function DocLineModalCtrl($http, $uibModalInstance, messageService, content, lineModel){
        var vm = this;
        vm.title = '';
        vm.okClick = okClick;
        vm.cancelClick = cancelClick;
        vm.products = [];
        vm.refreshProducts = refreshProducts;
        vm.model = null;

        init();

        function init(){
            console.log('content', content);
            vm.title = content.title;
            vm.model = lineModel;
        }

        function cancelClick(){
            $uibModalInstance.dismiss('cancelClick');
        }

        function okClick(){
            $uibModalInstance.dismiss('okClick');
            $uibModalInstance.close(vm.model);
        }

        function refreshProducts(value){
            var filter = {
                name: value,
                page: 1,
                pageSize: 10
            };

            $http.get('api/product', {params: filter})
                .then(onSuccess);

            function onSuccess(response){
                vm.products = response.data.items;
            }
        }
    }
})();