(function(){
    'use strict'

    angular
        .module('wrhs')
        .factory('httpInterceptor', httpInterceptor)
        .config(config);

    httpInterceptor.$inject = ['$q' ,'messageService'];
    function httpInterceptor($q, messageService){
        return {
            responseError: responseError
        }

        function responseError(rejection){
            messageService.requestError(rejection);
            return $q.reject(rejection);
        }
    }

    config.$inject = ['$httpProvider'];
    function config($httpProvider){
       $httpProvider.interceptors.push('httpInterceptor');
    }

})();