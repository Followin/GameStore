# CoffeeScript

$('.filter-button').on('click', (e) ->
    e.preventDefault()
    forms = $('form')
    $.ajax(
        url: forms.attr('action')
        type: "POST"
        data: forms.serialize()
        success: (data, statusText) ->
            $('.games-list').html(data)
    )
)

$('#body-wrapper').on('click', '.page-link', (e) ->
    e.preventDefault()
    forms = $('form')
    $.ajax(
        url: forms.attr('action') + "?filterModel.page=#{$(this).data('page')}"
        type: "POST"
        data: forms.serialize()
        success: (data, statusText) ->
            $('.games-list').html(data)
    )
)