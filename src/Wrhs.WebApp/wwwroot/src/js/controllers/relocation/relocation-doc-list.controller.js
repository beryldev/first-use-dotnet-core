(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('RelocationDocListCtrl', RelocationDocListCtrl);

    RelocationDocListCtrl.$inject = ['$scope', '$http', '$state', 'messageService', 'documentListFactory'];

    function RelocationDocListCtrl($scope, $http, $state, messageService, documentListFactory){
        var vm = this;
        vm.filter = {};
        vm.gridConfig = {}

        init();

        function init(){
            console.log('RelocationDocListCtrl init');
            var service = documentListFactory.createService($scope, 'api/document/relocation', onRowDoubleClick);
            vm.filter = service.filter;
            vm.gridConfig = service.gridConfig;
        }

        function onRowDoubleClick(row){
            $state.go('operation.relocation', {id: row.entity.id});
        }
    }
})();