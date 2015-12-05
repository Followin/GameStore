(function() {
  var checkFilters, filters, filtersOnly, gamesApp, gamesLink;

  gamesLink = $('#gamesLink').val();

  filters = $('form').serialize();

  filtersOnly = $('form.filters-form').clone();

  checkFilters = function() {
    var forms;
    forms = $('form');
    if (filters === forms.serialize() || !forms.valid()) {
      $('input[type=submit]').prop('disabled', true);
      return;
    }
    return $('input[type=submit]').prop('disabled', false);
  };

  $('.filters-form input').on('change', function() {
    return checkFilters();
  });

  $('.filters-form input').on('input', function() {
    return checkFilters();
  });

  $('.filters-form select').on('change', function() {
    return checkFilters();
  });

  gamesApp = angular.module('gamesApp', []);

  gamesApp.controller('gameController', function($scope, gameService) {
    var changeScope, getGames;
    $scope.detailsLinkTemplate = $('#detailsLinkTemplate').val();
    changeScope = function(games, pageInfo) {
      return $scope.$apply(function() {
        var i, j, ref;
        $scope.games = games;
        if (pageInfo.TotalPages !== null && pageInfo.TotalPages > 1) {
          $scope.pages = [];
          for (i = j = 1, ref = pageInfo.TotalPages; 1 <= ref ? j <= ref : j >= ref; i = 1 <= ref ? ++j : --j) {
            $scope.pages.push(i);
          }
          $scope.current = pageInfo.CurrentPage;
        } else {
          $scope.pages = [];
        }
      });
    };
    getGames = function() {
      return gameService.getGames().success(function(games) {
        return $scope.games = games.DisplayModel;
      }).error(function(error) {
        return $scope.state = error;
      });
    };
    getGames();
    $('.filter-button').on('click', function(e) {
      var forms, serializedForms;
      e.preventDefault();
      forms = $('form');
      serializedForms = forms.serialize();
      return $.get(gamesLink + "?" + serializedForms).success(function(data, statusText) {
        filters = serializedForms;
        filtersOnly = $('form.filters-form');
        changeScope(data.DisplayModel, data.PageModel);
        return $('input[type=submit]').prop('disabled', true);
      });
    });
    $('#body-wrapper').on('click', '.page-link', function(e) {
      var serializedForms;
      e.preventDefault();
      serializedForms = filtersOnly.add('form.pagin-form').serialize();
      return $.get(gamesLink + "?filterModel.page=" + ($(this).data('page')) + "&" + serializedForms).success(function(data, statusText) {
        return changeScope(data.DisplayModel, data.PagingInfo);
      });
    });
    return $('.pagin-form').on('change', 'select', function() {
      var serializedForms;
      serializedForms = filtersOnly.add('form.pagin-form').serialize();
      return $.get(gamesLink + "?" + serializedForms).success(function(data, statusText) {
        return changeScope(data.DisplayModel, data.PagingInfo);
      });
    });
  });

  gamesApp.factory('gameService', [
    '$http', function($http) {
      var gameService;
      gameService = {};
      gameService.getGames = function() {
        return $http.get("" + gamesLink);
      };
      return gameService;
    }
  ]);

}).call(this);

//# sourceMappingURL=gameClient.js.map
