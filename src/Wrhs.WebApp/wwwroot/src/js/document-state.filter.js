(function(){
    'use strict';

    angular
        .module('wrhs')
        .filter('documentState', DocumentStateFilter);

    function DocumentStateFilter(){
        var states = [
            'open',
            'confirmed',
            'executed'
        ];

        return function(input){
             return states[input];
        }
    }
})();