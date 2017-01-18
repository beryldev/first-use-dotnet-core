(function(){
    'use strict'

    angular
        .module('wrhs')
        .factory('operationServiceFactory', operationServiceFactory);

    operationServiceFactory.$inject = ['$http', '$state', 'messageService'];

    function operationServiceFactory($http, $state, messageService){
        var factory = {
            create: create
        }

        return factory;

        function create(config){
            var service = {
                documentId: config.documentId,
                baseUrl: config.baseUrl,
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
                return $http.post(service.baseUrl, { documentId: service.documentId})
                    .then(onSuccess);

                function onSuccess(response){
                    service.operationStep = config.operationStep;
                    service.guid = response.data;
                    return getState();
                }
            }

            function getState(){
                return $http.get('api/operation/'+service.guid)
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
                var data = {
                    productId: service.operationStep.line.product.id,
                    quantity: service.operationStep.quantity,
                    dstLocation: service.operationStep.dstLocation,
                    srcLocation: service.operationStep.srcLocation
                };

                $http.post(service.baseUrl+'/'+service.guid+'/shift', data)
                    .then(onSuccess);
                
                function onSuccess(response){
                    service.getState();
                }
            }

            function confirmOperation(){
                $http.post('/api/operation/'+service.guid)
                    .then(onSuccess);

                function onSuccess(response){
                    messageService.success(config.successConfirmMessage, "Done");
                    $state.go(config.successConfirmRedirect);
                }
            }
        }
    }
})();