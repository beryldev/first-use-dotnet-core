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
                lines: [],
                remarks: '',
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
                
            }

            function openNewLineModal(lineModel){
                var content = { 
                    title: 'New line', 
                    line: {quantity: 1} 
                };

                openLineModal(content, addDocLine, lineModel);
            }

            function openChangeLineModal(line){
                var content = {
                    title: 'Change line'
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
                    //size: size,
                    //appendTo: parentElem,        
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