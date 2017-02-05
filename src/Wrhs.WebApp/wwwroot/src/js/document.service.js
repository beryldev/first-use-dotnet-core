(function(){
    'use strict'

    angular
        .module('wrhs')
        .factory('documentServiceFactory', DocumentServiceFactory);

    DocumentServiceFactory.$inject = ['$http', '$uibModal', '$state', 'messageService'];

    function DocumentServiceFactory($http, $uibModal, $state, messageService){
        var factory = {
            createService: createService
        }

        return factory;

        function createService(config){
            var service = {
                baseUrl: config.baseUrl,
                guid: '',
                openNewLineModal: openNewLineModal,
                openChangeLineModal: openChangeLineModal,
                removeLine: removeLine,
                save: save,
                update: update,
                getDocument: getDocument,
                document: {},
            };

            initService();

            return service;

            function initService(){
                service.document = {
                    remarks: '',
                    lines: [],
                    state: null,
                    issueDate: null,
                    fullNumber: ''
                };
            }

            function openNewLineModal(){
                var content = { 
                    title: 'New document line', 
                    line: {quantity: 1} 
                };

                var lineModel = {
                    lp: 0,
                    product: null,
                    quantity: 1,
                    srcLocation: '',
                    dstLocation: ''
                };

                openLineModal(content, addDocLine, lineModel);
            }

            function openChangeLineModal(line){
                var content = {
                    title: 'Change document line'
                }

                var editLine  = {
                    product: line.product,
                    quantity: line.quantity,
                    srcLocation: line.srcLocation,
                    dstLocation: line.dstLocation,
                    index: service.document.lines.indexOf(line)
                };

                openLineModal(content, updateDocLine, editLine);
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
                });
                modalInstance.close = closeCallback;
            }

            function addDocLine(line){
                service.document.lines.push(line);
            }

            function updateDocLine(line){
                service.document.lines[line.index] = line;
                line = null;
            }

            function removeLine(line){
                service.document.lines.splice(service.document.lines.indexOf(line), 1);
            }

            function save(){
                var document = {
                    lines: service.document.lines.map(function(line){
                        return {
                            quantity: line.quantity,
                            productId: line.product.id,
                            srcLocation: line.srcLocation,
                            dstLocation: line.dstLocation
                        }
                    }),
                    remarks: service.document.remarks
                };

                $http.post(config.baseUrl, document)
                    .then(onSuccess);

                function onSuccess(response){
                    messageService.success("", "Created new document");
                    $state.go(config.goToAfterSave);
                }
            }

            function update(){
                var document = {
                    lines: service.document.lines.map(function(line){
                        return {
                            quantity: line.quantity,
                            productId: line.product.id,
                            srcLocation: line.srcLocation,
                            dstLocation: line.dstLocation
                        }
                    }),
                    remarks: service.document.remarks
                }

                $http.put(config.baseUrl + '/' + service.document.id, service.document)
                    .then(successCallback);
                
                function successCallback(resp){
                    console.log('success');
                }
            }

            function getDocument(id){
                return $http.get('api/document/'+id)
                    .then(successCallback);

                function successCallback(resp){
                    service.document = resp.data;
                }
            }
        }


    }
})();