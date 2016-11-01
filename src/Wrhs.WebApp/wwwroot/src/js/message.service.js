(function(){
    "use strict"

    angular
        .module("wrhs")
        .factory("messageService", messageService);

    messageService.$inject = [];

    function messageService(){
        var service = {
            error           : error,
            info            : info,
            requestError    : requestError,
            success         : success,
            warning         : warning
        };

        return service;

        function error(message, title){
            toastr.error(message, title);
        }

        function info(message, title){
            toastr.info(message, title);
        }

        function success(message, title){
            toastr.success(message, title);
        }

        function warning(message, title){
            toastr.warning(message, title);
        }

        function requestError(errorResponse){
            if(errorResponse.status === 404){
                error("Not found", "Operation failed");
            } else if(errorResponse.status === 400){
                var message = "";
                errorResponse.data.forEach(function(item){
                    message += item.message + "<br>";
                });
                error(message, "Operation failed");
            } else {
                error("Unexpected error.", "Operation failed");
            }
        }
    }
})();