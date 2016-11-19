(function(){
    'use strict'

    var app = angular.module('wrhs', [
        'ngSanitize', 
        'ngAnimate',
        'ui.router', 
        'ui.bootstrap', 
        'ui.grid', 
        'ui.grid.pagination', 
        'ui.grid.selection', 
        'ui.select'
    ]);

    app.directive('focusMe', function() {
    return {
        scope: { trigger: '=focusMe' },
        link: function(scope, element) {
        scope.$watch('trigger', function(value) {
            if(value === true) { 
                element[0].focus();
                scope.trigger = false;
            }
        });
        }
    };
    });
})();