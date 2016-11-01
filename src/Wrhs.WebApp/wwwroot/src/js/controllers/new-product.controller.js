(function(){
    "use strict"

    angular
        .module("wrhs")
        .controller("NewProductCtrl", NewProductCtrl);

    NewProductCtrl.$inject = ["$http", "$state", "messageService"];

    function NewProductCtrl($http, $state, messageService){
        var vm = this;
        vm.save = save;
        vm.product = {
                code: "",
                name: "",
                ean: "",
                sku: "",
                description: ""
            };
        
        init();

        function init(){
            console.log("NewProductCtrl init");
        }

        function save(){
            $http.post("api/product", vm.product)
                .then(onSuccess, onError);

            function onSuccess(response){
                messageService.success("", "Added new product");
                $state.go('products');
            }

            function onError(error){
                messageService.requestError(error);          
            }
        }
    }

})();