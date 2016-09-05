(function () {
    'use strict';

    angular
        .module('fileExplorerApp')
        .service('fileExplorerApiService', fileExplorerApiService);

    fileExplorerApiService.$inject = ['$q','$http'];

    function fileExplorerApiService($q, $http) {

        // service is singleton, so constcutor will be called only one time
        constructor();

        var url = "/api/FileExplorer";
        var appToken;
        var countRequestCanceller;
        var countRequestSended = false;

        this.getDrives = function (request) {
            var requestUrl = getUrl(request, "Drives");
            return $http.get(requestUrl);
        };

        this.getEntries = function (request) {
            var requestUrl = getUrl(request, "Entries");
            return $http.get(requestUrl);
        };

        this.getCountFiles = function (request) {
            cancellPrevivous();

            var requestUrl = getUrl(request, "CountFiles");
            return $http.get(requestUrl, { timeout: countRequestCanceller.promise });
        };

        this.getParentEntries = function (request) {
            var requestUrl = getUrl(request, "ParentEntries");
            return $http.get(requestUrl);
        };

        this.getParentCountFiles = function (request) {
            cancellPrevivous();

            var requestUrl = getUrl(request, "ParentCountFiles");
            return $http.get(requestUrl, { timeout: countRequestCanceller.promise });
        };

       function constructor() {
            // for entire life of page will be generated token
            // all requested will be identified by this token
           appToken = guid();

           // when user browse files, app can sent multiple count requests
           // this will cancell previvous requests, because we don't need them
           countRequestCanceller = $q.defer();
        };

        var encodeBase64 = function (string) {
            return base64.encode(string);
        };

        var getUrl = function (request, action) {
            request.cached = request.cached == undefined? true : false; // to force using cached responses

            var urlForRequest = url + '/' + action + '/' + appToken + '/cached/' + request.cached;
            if (request.path)
                urlForRequest += '/path/' + encodeBase64(request.path);

            return urlForRequest;
        };

        function guid() {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                  .toString(16)
                  .substring(1);
            }

            return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
              s4() + '-' + s4() + s4() + s4();
        };

        var cancellPrevivous = function () {
            if (countRequestSended) {
                countRequestCanceller.resolve("new request");
                countRequestSended = true;
            }

            countRequestCanceller = $q.defer();
        };
    }
})();