angular
  .module("umbraco")
  .constant("kompairComparePath", "/umbraco/backoffice/Kompair/KompairDashboard/Compare")
  .controller("kompair.dashboardController",
    [
      "$scope", "$http", "$location", "kompairComparePath",
      function($scope, $http, $location, kompairComparePath) {
        var vm = this;
        vm.sourceSite = $location.protocol() + "://" + $location.host() + ($location.port() === 80 ? "" : (":" + $location.port()));
        vm.targetSite = null;
        vm.results = {
          SourceDocumentTypes: [],
          TargetDocumentTypes: [],
          SourcePropertyGroups: [],
          TargetPropertyGroups: [],
          SourceProperties: [],
          TargetProperties: [],
          SourcePropertyEditors: [],
          TargetPropertyEditors: []
        };

        vm.hasError = false;
        vm.errorMessage = null;

        var hostRegex =
        /^http(s?):\/\/(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])(:\d+)?(\/?)$/i;
        
        $scope.vm = vm;
        $scope.compare = function () {
          if (!vm.targetSite) {
            vm.hasError = true;
            vm.errorMessage = "Please enter a target site.";
            return;
          }

          if (!hostRegex.test(vm.targetSite)) {
            vm.hasError = true;
            vm.errorMessage = "Target Site not valid. Ensure the url provided starts with http:// or https://.\n https:// is required if authentication has not been disabled";
            return;
          }

          vm.hasError = false;
          vm.results = {
            SourceDocumentTypes: [],
            TargetDocumentTypes: [],
            SourcePropertyGroups: [],
            TargetPropertyGroups: [],
            SourceProperties: [],
            TargetProperties: [],
            SourcePropertyEditors: [],
            TargetPropertyEditors: []
          };

          $http({
              url: kompairComparePath,
              method: "POST",
              data: { targetUrl: vm.targetSite }
            })
            .success(function (result) {
              vm.results = result;
            })
            .error(function () {
              vm.hasError = true;
              vm.errorMessage = "Something went wrong. Please try again";
            });
        };

        $scope.propertyEditorOrderBy = function(propertyEditor) {
          return propertyEditor.Alias.replace("Umbraco.", "");
        }
      }
    ]);