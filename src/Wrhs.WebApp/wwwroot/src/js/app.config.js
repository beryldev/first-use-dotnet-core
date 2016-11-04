(function(){
    'use strict'

    angular
        .module('wrhs')
        .config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider', '$locationProvider'];

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
                        templateUrl: 'templates/productsList.html',
                        controller: 'ProductsListCtrl as vm'
                    }
                }
            })
            .state('products.new', {
                url: '/new',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/newProduct.html',
                        controller: 'NewProductCtrl as vm'
                    }
                }
            })
            .state('products.details', {
                url: '/:id',
                views: {
                    'wrapper@':{
                        templateUrl: 'templates/productDetails.html',
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
                        templateUrl: 'templates/deliveryDocList.html',
                        controller: 'DeliveryDocListCtrl as vm'
                    }
                }
            })
            .state('documents.delivery.new', {
                url: '/new',
                views: {
                    'wrapper@': {
                        templateUrl: 'templates/newDeliveryDoc.html',
                        controller: 'NewDeliveryDocCtrl as vm'
                    }
                }
            });
    }

})();