(function(){
    'use strict';

    angular
        .module('wrhs')
        .controller('EditDeliveryDocCtrl', EditDeliveryDocCtrl);

    EditDeliveryDocCtrl.$inject = ['$http', '$stateParams'];

    function EditDeliveryDocCtrl($http, $stateParams){
        var vm = this;
        vm.document = {};
        vm.gridConfig = {};

        init();

        function init(){
            getDocument($stateParams.id);
            console.log('EditDeliveryDocCtrl init');
        }

        function getDocument(id){
             $http.get('/api/document/'+$stateParams.id)
                .then(successCallback);

            function successCallback(resp){
                vm.document = resp.data;
                vm.gridConfig = getGridConfig()
            }
        }

        function getGridConfig(){
            return {
                data: vm.document.lines,
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
            }
        }
    }

})();