(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('DeliveryDocListCtrl', DeliveryDocListCtrl);

    DeliveryDocListCtrl.$inject = ['$scope', '$http', 'messageService', 'documentListFactory'];

    function DeliveryDocListCtrl($scope, $http, messageService, documentListFactory){
        var vm = this;
        vm.filter = {};
        vm.gridConfig = {}

        init();

        function init(){
            console.log('DeliveryDocListCtrl init');
            var service = documentListFactory.createService($scope, 'api/document/delivery');
            vm.filter = service.filter;
            vm.gridConfig = service.gridConfig;
        }
    }
})();