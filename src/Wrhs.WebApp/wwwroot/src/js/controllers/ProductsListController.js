(function(){
    "use strict"

    angular
        .module("wrhs")
        .controller("ProductsListCtrl", ProductsListCtrl);

    //ProductsListCtrl.$inject = [];

    function ProductsListCtrl(){
        var vm = this;

        init();

        function init(){
            console.log("ProductsListCtrl init");
        }
    }
})();