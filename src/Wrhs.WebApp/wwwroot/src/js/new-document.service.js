(function(){
    'use strict'

    angular
        .module('wrhs')
        .factory('newDocServiceFactory', newDocServiceFactory);

    newDocServiceFactory.$inject = ['$http', '$uibModal', '$state', 'messageService'];

    function newDocServiceFactory($http, $uibModal, $state, messageService){
        var factory = {
            createService: createService
        }

        return factory;

        function createService(config){
            var service = {
                document: {},
                baseUrl: '',
                guid: '',
                initNewDoc: initNewDoc,
                openNewLineModal: openNewLineModal,
                openChangeLineModal: openChangeLineModal,
                removeLine: removeLine,
                save: save
            };

            initService();

            return service;

            function initService(){
                service.document = config.documentModel;
                service.baseUrl = config.baseUrl;
            }

            function initNewDoc(){
                return $http.get(service.baseUrl+'/new')
                    .then(onSuccess);

                function onSuccess(response){
                    service.guid = response.data;
                }
            }

            function openNewLineModal(lineModel){
                console.log('open new line modal');
                var content = { 
                    title: 'New line', 
                    line: {quantity: 1} 
                };

                openLineModal(content, addDocLine, lineModel);
            }

            function openChangeLineModal(line){
                console.log('open edit line modal', line);
                var content = {
                    title: 'Change line'
                }

                openLineModal(content, updateDocLine, line);
            }

            function openLineModal(content, closeCallback, lineModel){
                var modalInstance = $uibModal.open({
                    ariaLabelledBy: 'modal-title',
                    ariaDescribedBy: 'modal-body',
                    templateUrl: config.docLineModalTemplateUrl,
                    controller: 'DocLineModalCtrl',
                    controllerAs: 'vm',
                    resolve: {
                        content: function() { return content; },
                        lineModel: function() { return lineModel; }
                    }
                    //size: size,
                    //appendTo: parentElem,        
                });
                modalInstance.close = closeCallback;
            }

            function addDocLine(line){
                var cmd = config.lineToCmd(line);

                $http.post(service.baseUrl+'/new/'+service.guid+'/line', cmd)
                    .then(onSuccess);

                function onSuccess(response){
                    service.document.lines = response.data;
                }
            }

            function updateDocLine(line){
                console.log('update', line);
                $http.put(config.baseUrl + '/new/' + service.guid + '/line', line)
                    .then(onSuccess);

                function onSuccess(response){
                    service.document.lines = response.data;
                }
            }

            function removeLine(line){
                var requestConfig = {
                    data: line,
                    headers: {'Content-Type': 'application/json'}
                };

                $http.delete(config.baseUrl+'/new/'+service.guid+'/line', requestConfig)
                    .then(onSuccess);
                
                function onSuccess(response){
                    service.document.lines = response.data;
                }
            }

            function save(returnRoute){
                $http.post(config.baseUrl + '/new/'+service.guid, service.document)
                    .then(onSuccess);

                function onSuccess(response){
                    messageService.success("", "Created new document");
                    $state.go(returnRoute);
                }
            }
        }
    }
})();