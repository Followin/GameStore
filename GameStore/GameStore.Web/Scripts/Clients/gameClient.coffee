# CoffeeScript



gamesLink = $('#gamesLink').val()
filters = $('form').serialize()
filtersOnly = $('form.filters-form').clone()

checkFilters = ->
    forms = $('form')
    if filters == forms.serialize() || !forms.valid()
        $('input[type=submit]').prop('disabled', true)
        return 
    $('input[type=submit]').prop('disabled', false)

$('.filters-form input').on('change', ->
    do checkFilters
)
$('.filters-form input').on('input', ->
    do checkFilters
)
$('.filters-form select').on('change', ->
    do checkFilters
)

gamesApp = angular.module('gamesApp', [])
gamesApp.controller('gameController', ($scope, gameService) ->
    
    
    $scope.detailsLinkTemplate = $('#detailsLinkTemplate').val()
    
    changeScope = (games, pageInfo) ->
        $scope.$apply(() ->
            $scope.games = games
            if pageInfo.TotalPages isnt null and pageInfo.TotalPages > 1
                $scope.pages = []
                for i in [1..pageInfo.TotalPages]
                    $scope.pages.push(i)
                $scope.current = pageInfo.CurrentPage
            else
                $scope.pages = []
            return
         )
    
    getGames = () ->
        gameService.getGames()
            .success((games)->
                $scope.games = games.DisplayModel
             )
            .error((error) ->
                $scope.state = error
             )
    do getGames
    
    $('.filter-button').on('click', (e) ->
        e.preventDefault()
        forms = $('form')
        serializedForms = forms.serialize()
        $.get("#{gamesLink}?#{serializedForms}")
        .success((data, statusText) ->
            filters = serializedForms
            filtersOnly = $('form.filters-form')
            changeScope(data.DisplayModel, data.PageModel)
            $('input[type=submit]').prop('disabled', true)
         )
     )
     
    $('#body-wrapper').on('click', '.page-link', (e) ->
        e.preventDefault()
        serializedForms = filtersOnly.add('form.pagin-form').serialize()
        $.get("#{gamesLink}?filterModel.page=#{$(this).data('page')}&#{serializedForms}")
        .success((data, statusText) ->
            changeScope(data.DisplayModel, data.PagingInfo)
         )
     )
     
    $('.pagin-form').on('change', 'select', ->
        serializedForms = filtersOnly.add('form.pagin-form').serialize()
        $.get("#{gamesLink}?#{serializedForms}")
        .success((data, statusText) ->
            changeScope(data.DisplayModel, data.PagingInfo)
         )
     )     
)




gamesApp.factory('gameService', ['$http', ($http)->
    gameService = {}
    gameService.getGames = () -> 
        $http.get("#{gamesLink}")
    return gameService
])