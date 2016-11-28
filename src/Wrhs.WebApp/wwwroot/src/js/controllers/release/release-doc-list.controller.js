(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('ReleaseDocListCtrl', ReleaseDocListCtrl);

    ReleaseDocListCtrl.$inject = ['$scope', '$http', '$state', 'messageService', 'documentListFactory'];

    function ReleaseDocListCtrl($scope, $http, $state, messageService, documentListFactory){
        var vm = this;
        vm.filter = {};
        vm.gridConfig = {};

        init();

        function init(){
            var service = documentListFactory.createService($scope, 'api/document/release', onRowDoubleClick);
            vm.filter = service.filter;
            vm.gridConfig = service.gridConfig;
            console.log('ReleaseDocListCtrl init');
        }

        function onRowDoubleClick(row){
            $state.go('operation.release', { id: row.entity.id });
        }
    }
})();