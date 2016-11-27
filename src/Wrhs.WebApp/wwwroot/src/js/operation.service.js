(function(){
    'use strict'

    angular
        .module('wrhs')
        .factory('operationServiceFactory', operationServiceFactory);

    operationServiceFactory.$inject = ['$http', 'messageService'];

    function operationServiceFactory($http, messageService){
        var factory = {
            create: create
        }

        return factory;

        function create(baseUrl, documentId, operationStep){
            var service = {
                documentId: documentId,
                baseUrl: baseUrl,
                guid: '',
                state: {},
                initOperation: initOperation,
                getState: getState,
                quantityFocus: false,
                selectLine: selectLine,
                operationStep: {},
                addOperationStep: addOperationStep,
                confirmOperation: confirmOperation
            };

            return service;

            function initOperation(){
                return $http.get(service.baseUrl+'/new/'+ service.documentId)
                    .then(onSuccess);

                function onSuccess(response){
                    service.operationStep = operationStep;
                    service.guid = response.data;
                    return getState();
                }
            }

            function getState(){
                return $http.get(service.baseUrl+'/'+service.guid)
                    .then(onSuccess);

                function onSuccess(response){
                    service.state = response.data;
                }
            }

            function selectLine(line){
                service.operationStep.line = line;
                service.quantityFocus = true;
            }

            function addOperationStep(){
                $http.post(service.baseUrl+'/'+service.guid+'/step', 
                    service.operationStep).then(onSuccess);
                
                function onSuccess(response){
                    service.state = response.data;
                }
            }

            function confirmOperation(){
                $http.post(service.baseUrl+'/'+service.guid)
                    .then(onSuccess);

                function onSuccess(response){
                    messageService.success("", "Done");
                }
            }
        }
    }
})();