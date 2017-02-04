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
                lines: [],
                remarks: '',
                openNewLineModal: openNewLineModal,
                openChangeLineModal: openChangeLineModal,
                removeLine: removeLine,
                save: save
            };

            return service;

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
                    index: service.lines.indexOf(line)
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
                service.lines.push(line);
            }

            function updateDocLine(line){
                service.lines[line.index] = line;
                line = null;
            }

            function removeLine(line){
                service.lines.splice(service.lines.indexOf(line), 1);
            }

            function save(returnRoute){
                var document = {
                    lines: service.lines.map(function(line){
                        return {
                            quantity: line.quantity,
                            productId: line.product.id,
                            srcLocation: line.srcLocation,
                            dstLocation: line.dstLocation
                        }
                    }),
                    remarks: service.remarks
                };

                $http.post(config.baseUrl, document)
                    .then(onSuccess);

                function onSuccess(response){
                    messageService.success("", "Created new document");
                    $state.go(returnRoute);
                }
            }
        }
    }
})();