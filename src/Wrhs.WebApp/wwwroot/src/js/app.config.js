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
                        templateUrl: 'templates/delivery/deliveryDocList.html',
                        controller: 'DeliveryDocListCtrl as vm'
                    },
                    'context@': {
                        templateUrl: 'templates/delivery/deliveryDocListContext.html'
                    }
                }
            })
            .state('documents.delivery.new', {
                url: '/new',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/delivery/newDeliveryDoc.html',
                        controller: 'NewDeliveryDocCtrl as vm'
                    },
                    'context@': {
                        template: '<strong class="navbar-text context-title"><i class="fa fa-arrow-down margin-r" aria-hidden="true"></i> New delivery document</strong>'
                    }
                }
            })
            .state('documents.relocation', {
                url: '/relocation',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/relocation/relocationDocList.html',
                        controller: 'RelocationDocListCtrl as vm'
                    },
                    'context@': {
                        templateUrl: 'templates/relocation/relocationDocListContext.html'
                    }
                }
            })
            .state('documents.relocation.new', {
                url: '/new',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/relocation/newRelocationDoc.html',
                        controller: 'NewRelocationDocCtrl as vm'
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
                        templateUrl: 'templates/release/releaseDocList.html',
                        controller: 'ReleaseDocListCtrl as vm'
                    },
                    'context@': {
                        templateUrl: 'templates/release/releaseDocListContext.html'
                    }
                }
            })
            .state('documents.release.new', {
                url: '/new',
                views: { 
                    'wrapper@': {
                        templateUrl: 'templates/release/newReleaseDoc.html',
                        controller: 'NewReleaseDocCtrl as vm'
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

})();