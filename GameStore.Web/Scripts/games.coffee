# CoffeeScript

filters = $('form').serialize()
filtersOnly = $('form.filters-form').clone()
indexPostLink = $('form').attr('action')

$('.filter-button').on('click', (e) ->
    e.preventDefault()
    forms = $('form')
    serializedForms = forms.serialize()
    $.ajax(
        url: forms.attr('action')
        type: "POST"
        data: forms.serialize()
        success: (data, statusText) ->
            filters = serializedForms
            filtersOnly = $('form.filters-form')
            $('.games-list').html(data)
            $('input[type=submit]').prop('disabled', true)
    )
)

$('#body-wrapper').on('click', '.page-link', (e) ->
    e.preventDefault()
    $.ajax(
        url: "#{indexPostLink}?filterModel.page=#{$(this).data('page')}"
        type: "POST"
        data: filtersOnly.add('form.pagin-form').serialize()
        success: (data, statusText) ->
            $('.games-list').html(data)
            
    )
)

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

$('.pagin-form').on('change', 'select', ->
    $.ajax(
        url: indexPostLink
        type: "POST"
        data: filtersOnly.add('form.pagin-form').serialize()
        success: (data, statusText) ->
            $('.games-list').html(data)
   )
)

$('.filterin-block-switch').on('click', ->
    $filterinBlock = $('.filterin-block')
    $arrow = $(this).find('.arrow')
    if $filterinBlock.hasClass('minified')
        $filterinBlock.removeClass('minified')
        $arrow.removeClass('right').addClass('left')
    else
        $filterinBlock.addClass('minified')
        $arrow.removeClass('left').addClass('right')
)