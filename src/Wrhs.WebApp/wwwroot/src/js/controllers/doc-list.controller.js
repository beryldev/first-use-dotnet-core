(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('DocListCtrl', DocListCtrl);

    DocListCtrl.$inject = ['$scope', '$http', '$state', 'messageService', 'documentListFactory', 'config'];

    function DocListCtrl($scope, $http, $state, messageService, documentListFactory, config){
        var vm = this;
        vm.service = {};
        vm.filter = {};
        vm.gridConfig = {};
        vm.newDocState='';
        

        init();

        function init(){      
            vm.service = documentListFactory.createService($scope, config.dataSourceUrl, onRowDoubleClick);
            vm.filter = vm.service.filter;
            vm.gridConfig = vm.service.gridConfig;
            vm.newDocState = config.newDocState;
            console.log('DocListCtrl init');
        }

        function onRowDoubleClick(row){
            $state.go(config.editDocState, {id: row.entity.id});
        }
    }
})();