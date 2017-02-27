(function(){
    'use strict';

    angular
        .module('wrhs')
        .filter('documentState', DocumentStateFilter);

    function DocumentStateFilter(){
        var states = [
            'open',
            'confirmed',
            'executed',
            'canceled'
        ];

        return function(input){
             return states[input];
        }
    }
})();