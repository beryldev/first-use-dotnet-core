(function(){
    'use strict';

    angular
        .module('wrhs')
        .controller('EditDeliveryDocCtrl', EditDeliveryDocCtrl);

    EditDeliveryDocCtrl.$inject = ['$http', '$stateParams', '$scope', '$state', 'messageService', 'modalService', 'documentServiceFactory'];

    function EditDeliveryDocCtrl($http, $stateParams, $scope, $state, messageService, modalService, documentServiceFactory){
        var vm = this;
        vm.document = {};
        vm.rules = {};
        vm.gridConfig = {};
        vm.selectedLine = null;
        vm.addLine = addLine;
        vm.removeSelected = removeSelected;
        vm.saveAndBack = saveAndBack;
        vm.deleteDocument = deleteDocument;
        vm.confirmDocument = confirmDocument;
        vm.cancelDocument = cancelDocument;
        vm.service = null;

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

            var type = 'delivery';
            vm.service = documentServiceFactory.createService({
                baseUrl: 'api/document/'+type,
                goToAfterSave: 'documents.'+type,
                docLineModalTemplateUrl: 'templates/'+type+'/'+type+'DocLineModal.html'
            });
            
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
            vm.service.getDocument($stateParams.id)
                .then(successCallback);

            function successCallback(){
                vm.document = vm.service.document; 
                vm.gridConfig.data = vm.document.lines;
                vm.rules = {
                    canBeginOperation: vm.document.state===1,
                    canConfirm: vm.document.state === 0,
                    canDelete: vm.document.state === 0,
                    canCancel: vm.document.state === 1,
                    canEdit: vm.document.state === 0,
                    hasAction: vm.document.state !== 2 && vm.document.state !== 3
                }             
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

        function saveAndBack(){
            $state.go('documents.delivery');
        }

        function deleteDocument(){

            modalService.showConfirmModal({
                title: 'Document remove',
                message: 'Please confirm remove this document.',
                onConfirm: confirmDelete,
                onCancel: function(){return false;}
            });
            
            function confirmDelete(){
                var id = vm.document.id;
                $http.delete('api/document/delivery/'+id)
                    .then(successCallback);

                function successCallback(resp){
                    messageService.success("", "Document was successfully deleted");
                    $state.go('documents.delivery');
                }
            }     
        }

        function confirmDocument(){
            var id = vm.document.id;
            $http.put('api/document/delivery/'+id+'/state?state=1')
                .then(successCallback);

            function successCallback(resp){
                loadData();
            }
        }

        function cancelDocument(){
            modalService.showConfirmModal({
                title: 'Cancel document',
                message: 'Please confirm cancellation of this document.',
                onConfirm: confirmCancel,
                onCancel: function(){return false;}
            });

            function confirmCancel(){
                var id = vm.document.id;
                $http.put('api/document/delivery/'+id+'/state?state=3')
                    .then(successCallback);

                function successCallback(resp){
                    loadData();
                }
            }
            
        }
    }

})();