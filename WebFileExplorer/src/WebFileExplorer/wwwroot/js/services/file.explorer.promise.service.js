(function () {
    'use strict';

    angular
        .module('fileExplorerApp')
        .service('fileExplorerPromiseService', fileExplorerPromiseService);

    fileExplorerPromiseService.$inject = ['$q'];

    function fileExplorerPromiseService($q) {

        this.createPromise = function (call, request, thenCallback) {
            $q(function (resolve, reject) {
                call(request)
                .success(function () {
                    resolve();
                }).error(function (response) {
                    console.error(call);
                    console.error(request);
                    console.error(response);
                    console.error(thenCallback);
                    reject();
                }).then(function (response) {
                    if (response)
                        thenCallback(response.data);
                    else
                        thenCallback();
                });
            });
        };
    }
})();