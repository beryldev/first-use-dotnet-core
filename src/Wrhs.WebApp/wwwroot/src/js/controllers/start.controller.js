(function(){
    "use strict"

    angular
        .module("wrhs")
        .controller("StartCtrl", StartCtrl);

    StartCtrl.$inject = [];

    function StartCtrl(){
        var vm = this;

        init();

        function init(){
            console.log('StartCtrl init');
        }
    }

})();