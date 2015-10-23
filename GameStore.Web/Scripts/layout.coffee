# CoffeeScript

window.onscroll = ->
    scrolled = window.pageYOffset || document.documentElement.scrollTop
    if scrolled > 5
        $('header').addClass('mini')
    else $('header').removeClass('mini')
    
objHeight = 0

if $('.paper-block')[0] isnt undefined
    $.each($('paper-block').children(":not(:hidden)"), ->
        objHeight += $(this).height()
    )

bodyHeight = $('body').height()
viewportHeight = document.documentElement.clientHeight || window.innerHeight || 0
$('body').height(Math.max(viewportHeight, objHeight, bodyHeight))




messageTimeout = (message) ->
    setTimeout( =>
        message.slideUp()
        setTimeout( =>
            message.remove()
        , 500)
    , 5000)

$.each($('.temp-message'), ->
    $(this).slideDown()
    if(!$(this).is('.temp-message-error'))
        messageTimeout($(this))
)

window.addMessage = (type, message) ->
    $tempMessagesContainer = $('.temp-messages')
    $message = $("<div class='temp-message temp-message-#{type}'>
        #{message}</div>")
    $tempMessagesContainer.prepend($message)
    $message.slideDown()
    if (type != 'error')
        messageTimeout($message)
    

$.each($('.validation-summary-errors li'), ->
    addMessage('error', $(this).html())
)      

$('#close-messages-button').click( ->
    $('.temp-message').slideUp()
)

$('body').on('click', '.clickable-row', ->
    window.document.location = $(this).data('href')
)

updateGamesCount = ->
    $.ajax(
        type: "GET",
        url: $('#games-count').data('href')
        success: (data, statusText) ->
            $('#games-count').html('')
            $('#games-count').html(data))
    setTimeout((-> updateGamesCount()), 60000)
    
updateGamesCount()           
     

