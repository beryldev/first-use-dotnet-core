(function(){
    "use strict"

    angular
        .module("wrhs")
        .controller("ProductsListCtrl", ProductsListCtrl);

    ProductsListCtrl.$inject = [];

    function ProductsListCtrl(){
        var vm = this;
        vm.data = [];

        init();

        function init(){
            vm.data = [
                {
                    code: "PROD1",
                    name: "Product 1",
                    ean: "11111111",
                    sku: "111",
                    description: "some desc"
                },
                {
                    code: "PROD2",
                    name: "Product 2",
                    ean: "11111112",
                    sku: "112",
                    description: "some desc"
                }
            ];
            console.log("ProductsListCtrl init");
        }
    }
})();