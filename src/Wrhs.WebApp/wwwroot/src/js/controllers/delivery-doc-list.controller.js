(function(){
    'use strict'

    angular
        .module('wrhs')
        .controller('DeliveryDocListCtrl', DeliveryDocListCtrl);

    DeliveryDocListCtrl.$inject = [];

    function DeliveryDocListCtrl(){
        var vm = this;

        init();

        function init(){
            console.log('DeliveryDocListCtrl init');
        }
    }
})();