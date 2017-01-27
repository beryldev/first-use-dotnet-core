(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('DeliveryDocListCtrl', DeliveryDocListCtrl);

    DeliveryDocListCtrl.$inject = ['$scope', '$http', '$state', 'messageService', 'documentListFactory'];

    function DeliveryDocListCtrl($scope, $http, $state, messageService, documentListFactory){
        var vm = this;
        vm.filter = {};
        vm.gridConfig = {}

        init();

        function init(){
            console.log('DeliveryDocListCtrl init');
            var service = documentListFactory.createService($scope, 'api/document/delivery', onRowDoubleClick);
            vm.filter = service.filter;
            vm.gridConfig = service.gridConfig;
        }

        function onRowDoubleClick(row){
            $state.go('documents.delivery.edit', {id: row.entity.id});
        }
    }
})();