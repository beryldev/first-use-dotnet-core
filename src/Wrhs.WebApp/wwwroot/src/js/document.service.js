(function(){
    'use strict'

    angular
        .module('wrhs')
        .factory('documentServiceFactory', DocumentServiceFactory);

    DocumentServiceFactory.$inject = ['$http', '$uibModal', '$state', 'messageService', 'modalService'];

    function DocumentServiceFactory($http, $uibModal, $state, messageService, modalService){
        var factory = {
            createService: createService
        }

        return factory;

        function createService(config, $scope){
            var service = {
                baseUrl: config.baseUrl,
                guid: '',
                openNewLineModal: openNewLineModal,
                openChangeLineModal: openChangeLineModal,
                removeLine: removeLine,
                save: save,
                update: update,
                getDocument: getDocument,
                deleteDocument: deleteDocument,
                confirmDocument: confirmDocument,
                cancelDocument: cancelDocument,
                document: {},
                rules: {}
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

                $scope.documentBusy = $http.post(config.baseUrl, document).then(onSuccess);

                function onSuccess(response){
                    messageService.success("", "Created new document");
                    $state.go(config.goBackState);
                }
            }

            function update(back){
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

                $scope.documentBusy = $http.put(config.baseUrl + '/' + service.document.id, document)
                    .then(successCallback);
                
                function successCallback(resp){
                    messageService.success('', 'Document saved');
                    if(back)
                        $state.go(config.goBackState);
                }
            }

            function getDocument(id){
                return $http.get('api/document/'+id)
                    .then(successCallback);

                function successCallback(resp){
                    service.document = resp.data;
                    service.rules = {
                        canBeginOperation: service.document.state===1,
                        canConfirm: service.document.state === 0,
                        canDelete: service.document.state === 0,
                        canCancel: service.document.state === 1,
                        canEdit: service.document.state === 0,
                        hasAction: service.document.state !== 2 && service.document.state !== 3
                    }    
                }
            }

            function deleteDocument(id){
                modalService.showConfirmModal({
                    title: 'Document remove',
                    message: 'Please confirm remove this document.',
                    onConfirm: confirmDelete,
                    onCancel: function(){return false;}
                });

                function confirmDelete(){
                    $scope.documentBusy = $http.delete(config.baseUrl+'/'+id)
                        .then(successCallback);

                    function successCallback(resp){
                        messageService.success('', 'Document was successfully deleted');
                        $state.go(config.goBackState);
                    }
                }
            }

            function confirmDocument(id){
                 modalService.showConfirmModal({
                    title: 'Confirm document',
                    message: 'Please confirm this operation.',
                    onConfirm: confirmDoc,
                    onCancel: function(){return false;}
                });

                function confirmDoc(){
                     $scope.documentBusy = $http.put(config.baseUrl+'/'+id+'/state?state=1')
                        .then(successCallback);

                    function successCallback(resp){
                        messageService.success('', "Document was confirmed");
                        service.getDocument(id);
                    }
                }
            }

            function cancelDocument(id){
                 modalService.showConfirmModal({
                    title: 'Cancel document',
                    message: 'Please confirm cancellation of this document.',
                    onConfirm: confirmCancel,
                    onCancel: function(){return false;}
                });

                function confirmCancel(){
                    $scope.documentBusy = $http.put(config.baseUrl+'/'+id+'/state?state=3')
                    .then(successCallback);

                    function successCallback(){
                        messageService.success('', "Document was cancelled");
                        service.getDocument(id);
                    }
                }
            }
        }


    }
})();