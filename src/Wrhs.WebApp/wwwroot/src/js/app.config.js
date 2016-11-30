(function(){
    'use strict'

    angular
        .module('wrhs')
        .config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider', '$locationProvider', '$httpProvider'];

    function config($stateProvider, $urlRouterProvider, $locationProvider){

        $urlRouterProvider.otherwise('/start');

        $stateProvider
            .state('start', {
                url: '/start',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/start.html',
                        controller: 'StartCtrl as vm'
                    }
                }
            })
            .state('products', {
                url: '/products',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/product/productsList.html',
                        controller: 'ProductsListCtrl as vm'
                    }
                }
            })
            .state('products.new', {
                url: '/new',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/product/newProduct.html',
                        controller: 'NewProductCtrl as vm'
                    }
                }
            })
            .state('products.details', {
                url: '/:id',
                views: {
                    'wrapper@':{
                        templateUrl: 'templates/product/productDetails.html',
                        controller: 'ProductDetailsCtrl as vm'
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
                    }
                }
            })
            .state('documents.delivery.new', {
                url: '/new',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/delivery/newDeliveryDoc.html',
                        controller: 'NewDeliveryDocCtrl as vm'
                    }
                }
            })
            .state('documents.relocation', {
                url: '/relocation',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/relocation/relocationDocList.html',
                        controller: 'RelocationDocListCtrl as vm'
                    }
                }
            })
            .state('documents.relocation.new', {
                url: '/new',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/relocation/newRelocationDoc.html',
                        controller: 'NewRelocationDocCtrl as vm'
                    }
                }
            })
            .state('documents.release', {
                url: '/release',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/release/releaseDocList.html',
                        controller: 'ReleaseDocListCtrl as vm'
                    }
                }
            })
            .state('documents.release.new', {
                url: '/new',
                views: { 
                    'wrapper@': {
                        templateUrl: 'templates/release/newReleaseDoc.html',
                        controller: 'NewReleaseDocCtrl as vm'
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
                    }
                }
            })
            .state('operation.relocation', {
                url: '/relocation/:id',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/relocation/operation.html',
                        controller: 'RelocationOperationCtrl as vm'
                    }
                }
            })
            .state('operation.release', {
                url: '/release/:id',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/release/operation.html',
                        controller: 'ReleaseOperationCtrl as vm'
                    }
                }
            })
            .state('stocks', {
                url: '/stocks',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/stocksList.html',
                        controller: 'StocksListCtrl as vm'
                    }
                }
            });
    }

})();