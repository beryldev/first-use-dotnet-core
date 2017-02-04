(function(){
    'use strict'

    angular
        .module('wrhs')
        .config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider', '$locationProvider'];

    function config($stateProvider, $urlRouterProvider, $locationProvider){

        $urlRouterProvider.otherwise('/stocks');

        $stateProvider
            .state('products', {
                url: '/products',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/product/productsList.html',
                        controller: 'ProductsListCtrl as vm'
                    },
                    'context@': {
                        templateUrl: 'templates/product/productsListContext.html'
                    }
                }
            })
            .state('products.new', {
                url: '/new',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/product/newProduct.html',
                        controller: 'NewProductCtrl as vm'
                    },
                    'context@': {
                        template: '<strong class="navbar-text context-title"><i class="fa fa-barcode margin-r" aria-hidden="true"></i> New product</strong>'
                    }
                }
            })
            .state('products.details', {
                url: '/:id',
                views: {
                    'wrapper@':{
                        templateUrl: 'templates/product/productDetails.html',
                        controller: 'ProductDetailsCtrl as vm'
                    },
                    'context@': {
                        template: '<strong class="navbar-text context-title"><i class="fa fa-barcode margin-r" aria-hidden="true"></i> Product details</strong>'
                    }
                }
            })
            .state('documents', {
                url: '/documents',
                abstract: true
            })
            .state('documents.delivery', {
                url: '/delivery',
                views: {
                    'wrapper@':{
                        templateUrl: 'templates/docList.html',
                        controller: 'DocListCtrl as vm',
                        resolve: {
                            config: function(){
                                return {
                                    dataSourceUrl: 'api/document/delivery',
                                    newDocState: 'documents.delivery.new',
                                    editDocState: 'documents.delivery.edit'
                                }
                            }
                        }
                    },
                    'context@': {
                        template: '<strong class="navbar-text context-title"><i class="fa fa-arrow-down margin-r" aria-hidden="true"></i> Delivery documents</strong>'
                    }
                }
            })
            .state('documents.delivery.new', {
                url: '/new',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/new-document.html',
                        controller: 'NewDocumentController as vm',
                        resolve: {
                            config: function(){
                                return {
                                    columnDefs: [
                                        { name: 'product.name', displayName: 'Product name', enableColumnMenu: false},
                                        { name: 'product.ean', displayName: 'EAN', enableColumnMenu: false},
                                        { name: 'quantity', displayName: 'Quantity', enableColumnMenu: false},
                                        { name: 'dstLocation', displayName: 'Dst location', enableColumnMenu: false}
                                    ],
                                    docServiceConfig: {
                                        baseUrl: 'api/document/delivery',
                                        goToAfterSave: 'documents.delivery',
                                        docLineModalTemplateUrl: 'templates/delivery/deliveryDocLineModal.html'
                                    }
                                }
                            }
                        }
                    },
                    'context@': {
                        template: '<strong class="navbar-text context-title"><i class="fa fa-arrow-down margin-r" aria-hidden="true"></i> New delivery document</strong>'
                    }
                }
            })
            .state('documents.delivery.edit', {
                url: '/:id',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/delivery/editDeliveryDoc.html',
                        controller: 'EditDeliveryDocCtrl as vm'
                    },
                    'context@': {
                        template: '<strong class="navbar-text context-title"><i class="fa fa-arrow-down margin-r" aria-hidden="true"></i> Edit delivery document</strong>'
                    }
                }
            })
            .state('documents.relocation', {
                url: '/relocation',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/docList.html',
                        controller: 'DocListCtrl as vm',
                        resolve: {
                            config: function(){
                                return {
                                    dataSourceUrl: 'api/document/relocation',
                                    newDocState: 'documents.relocation.new',
                                    editDocState: 'documents.relocation.edit'
                                }
                            }
                        }
                    },
                    'context@': {
                        template: '<strong class="navbar-text context-title"><i class="fa fa-arrow-down margin-r" aria-hidden="true"></i> Relocation documents</strong>'
                    }
                }
            })
            .state('documents.relocation.new', {
                url: '/new',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/new-document.html',
                        controller: 'NewDocumentController as vm',
                        resolve: {
                            config: function(){
                                return {
                                    columnDefs: [
                                        { name: 'product.name', displayName: 'Product name', enableColumnMenu: false},
                                        { name: 'product.ean', displayName: 'EAN', enableColumnMenu: false},
                                        { name: 'quantity', displayName: 'Quantity', enableColumnMenu: false},
                                        { name: 'srcLocation', displayName: 'Src location', enableColumnMenu: false},
                                        { name: 'dstLocation', displayName: 'Dst location', enableColumnMenu: false}
                                    ],
                                    docServiceConfig: {
                                        baseUrl: 'api/document/relocation',
                                        goToAfterSave: 'documents.relocation',
                                        docLineModalTemplateUrl: 'templates/relocation/relocationDocLineModal.html'
                                    }
                                };
                            }      
                        }
                    },
                     'context@': {
                        template: '<strong class="navbar-text context-title"><i class="fa fa-refresh margin-r" aria-hidden="true"></i> New relocation document</strong>'
                    }
                }
            })
            .state('documents.release', {
                url: '/release',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/docList.html',
                        controller: 'DocListCtrl as vm',
                        data: { value: 'test'},
                         resolve: {
                            config: function(){
                                return {
                                    dataSourceUrl: 'api/document/release',
                                    newDocState: 'documents.release.new',
                                    editDocState: 'documents.release.edit'
                                }
                            }
                        }
                    },
                    'context@': {
                        template: '<strong class="navbar-text context-title"><i class="fa fa-arrow-down margin-r" aria-hidden="true"></i> Release documents</strong>'
                    }
                }
            })
            .state('documents.release.new', {
                url: '/new',
                views: { 
                    'wrapper@': {
                        templateUrl: 'templates/new-document.html',
                        controller: 'NewDocumentController as vm',
                        resolve: {
                            config: function(){
                                return {
                                    columnDefs: [
                                        { name: 'product.name', displayName: 'Product name', enableColumnMenu: false},
                                        { name: 'product.ean', displayName: 'EAN', enableColumnMenu: false},
                                        { name: 'quantity', displayName: 'Quantity', enableColumnMenu: false},
                                        { name: 'srcLocation', displayName: 'Src location', enableColumnMenu: false}
                                    ],
                                    docServiceConfig: {
                                        baseUrl: 'api/document/release',
                                        goToAfterSave: 'documents.release',
                                        docLineModalTemplateUrl: 'templates/release/releaseDocLineModal.html'
                                    }
                                }
                            }
                        }
                    },
                     'context@': {
                        template: '<strong class="navbar-text context-title"><i class="fa fa-arrow-up margin-r" aria-hidden="true"></i> New release document</strong>'
                    }
                }
            })
            .state('operation', {
                abstract: true,
                url: '/operation'
            })
            .state('operation.delivery', {
                url: '/delivery/:id',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/delivery/operation.html',
                        controller: 'DeliveryOperationCtrl as vm'
                    },
                    'context@': {
                        templateUrl: 'templates/delivery/operationContext.html'
                    }
                }
            })
            .state('operation.relocation', {
                url: '/relocation/:id',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/relocation/operation.html',
                        controller: 'RelocationOperationCtrl as vm'
                    },
                    'context@': {
                        templateUrl: 'templates/relocation/operationContext.html'
                    }
                }
            })
            .state('operation.release', {
                url: '/release/:id',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/release/operation.html',
                        controller: 'ReleaseOperationCtrl as vm'
                    },
                    'context@': {
                        templateUrl: 'templates/release/operationContext.html'
                    }
                }
            })
            .state('stocks', {
                url: '/stocks',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/stocksList.html',
                        controller: 'StocksListCtrl as vm'
                    },
                    'context@': {
                        template: '<strong class="navbar-text context-title"><i class="fa fa-cubes margin-r" aria-hidden="true"></i> Stocks</strong>'
                    }
                }
            });
    }

    angular.module('wrhs').value('cgBusyDefaults',{
        //message:'Loading Stuff',
       // backdrop: false,
        //templateUrl: 'my_custom_template.html',
        delay: 300,
        //minDuration: 700,
        //wrapperClass: 'my-class my-class2'
    });

})();
