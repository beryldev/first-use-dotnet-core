(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('RelocationDocListCtrl', RelocationDocListCtrl);

    RelocationDocListCtrl.$inject = ['$scope', '$http', 'messageService', 'documentListFactory'];

    function RelocationDocListCtrl($scope, $http, messageService, documentListFactory){
        var vm = this;
        vm.filter = {};
        vm.gridConfig = {}

        init();

        function init(){
            console.log('RelocationDocListCtrl init');
            var service = documentListFactory.createService($scope, 'api/document/relocation');
            vm.filter = service.filter;
            vm.gridConfig = service.gridConfig;
        }
    }
})();