(function () {
    'use strict';

    angular
        .module('fileExplorerApp')
        .controller('fileExplorerController', fileExplorerController);

    fileExplorerController.$inject = ['$scope', 'fileExplorerPromiseService', 'fileExplorerApiService'];

    function fileExplorerController($scope, fileExplorerPromiseService, fileExplorerApiService) {

        // this model contains state of fetched objects from web api
        $scope.ready = {
            entries: false,
            count: false
        };

        $scope.countFiles = {};
        $scope.location = '';
        $scope.entries = [];

        $scope.path = [];

        $scope.initialize = function () {
            getDrives();
        };

        // root locations means when just have list of drives
        $scope.isLocationRoot = function () {
            return $scope.location === '';
        };

        $scope.locateClick = function (destination, cached) {
            if (destination.typeKey === 'File')
                return;

            locate(destination.name, cached);
        };

        $scope.locateParentClick = function (cached) {
            locateParent(cached);
        };

        $scope.update = function () {
            update();
        };

        $scope.jumpToLocation = function (folder) {

            // zero depth means drive
            if (folder.depth === 0)
                return jumpToLocation(folder.name + '\\');

            var path = '';

            $scope.path.filter(function (dir) {
                return dir.depth <= folder.depth;
            }).forEach(function (dir) {
                path = pathCombine(path, dir.name);
            });

            jumpToLocation(path);
        };

        $scope.jumpToRoot = function () {
            getDrives();
            $scope.location = '';
            $scope.path = [];
        };

        var getDrives = function (cached) {
            $scope.ready.entries = false;

            var request = {
                cached: cached
            };

            fileExplorerPromiseService.createPromise(fileExplorerApiService.getDrives, request, function (drives) {
                if (drives) {
                    $scope.ready.entries = true;
                    $scope.entries = drives;
                };
            });
        };

        var getEntries = function (path, cached) {
            $scope.ready.entries = false;

            var request = {
                cached: cached,
                path: path
            };

            fileExplorerPromiseService.createPromise(fileExplorerApiService.getEntries, request, function (entries) {
                if (entries) {
                    $scope.ready.entries = true;
                    $scope.entries = entries;

                    // in case if directory is empty
                    if (entries.length === 0)
                        return setLocation(path);

                    setLocation(entries[0].location);
                };
            });
        };

        var getParentEntries = function (cached) {
            $scope.ready.entries = false;

            var request = {
                cached: cached,
                path: $scope.location
            };

            fileExplorerPromiseService.createPromise(fileExplorerApiService.getParentEntries, request, function (entries) {
                if (entries) {
                    $scope.ready.entries = true;
                    $scope.entries = entries;

                    if (entries[0].typeKey !== 'Drive')
                        return setLocation(entries[0].location);

                    $scope.path = [];
                    $scope.location = '';
                };
            });
        };

        var countFiles = function (path, cached) {
            $scope.ready.count = false;

            var request = {
                cached: cached,
                path: path
            };

            fileExplorerPromiseService.createPromise(fileExplorerApiService.getCountFiles, request, function (result) {
                // in case if we had sent multiple requests and we got response which we don't need anymore
                if (result && !$scope.ready.count) {
                    $scope.ready.count = true;
                    $scope.count = result;
                };
            });
        };

        var parentCountFiles = function (cached) {
            $scope.ready.count = false;

            var request = {
                cached: cached,
                path: $scope.location
            };

            fileExplorerPromiseService.createPromise(fileExplorerApiService.getParentCountFiles, request, function (result) {
                if (result && !$scope.ready.count) {
                    $scope.ready.count = true;
                    $scope.count = result;
                };
            });
        };

        var locate = function (destination, cached) {
            var destinationPath = pathCombine($scope.location, destination);
            getEntries(destinationPath, cached);
            countFiles(destinationPath, cached);
        };

        var locateParent = function (cached) {
            getParentEntries(cached);
            parentCountFiles(cached);
        };

        var jumpToLocation = function (path) {
            getEntries(path);
            countFiles(path);
        };

        var update = function () {
            var cached = false;
            locate('', cached);
        };

        var pathCombine = function (path1, path2) {
            if (!path1)
                return path2;

            if (!path2)
                return path1;

            if (!path1.endsWith('\\'))
                path1 += '\\';

            return path1 + path2;
        };

        var setLocation = function (location) {
            var depth = 0;
            $scope.path = [];

            var ierarchy = location.split(/[\\\/]/).filter(function (n) { return n ? true : false; }); // deleting not necessery items
            var drive = ierarchy.shift();
            $scope.path.push(createIerarchyObject(drive, 'Drive', ierarchy.length === 0, depth++));

            var last = ierarchy.pop();

            ierarchy.forEach(function (item) {
                $scope.path.push(createIerarchyObject(item, 'Folder', false, depth++));
            });

            if(last)
                $scope.path.push(createIerarchyObject(last, 'Folder', true, depth));

            $scope.location = location;
        };

        function createIerarchyObject(name, type, last, depth) {
            return {
                name: name,
                type: type,
                last: last,
                depth: depth
            };
        };
    };
})();