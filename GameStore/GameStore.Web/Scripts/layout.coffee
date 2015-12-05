# CoffeeScript
$(document).ready(->
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


    $('.basket-link').on('mouseenter', ->
        width = 50
        $.each($('.basket-link .basket-text *'), ->
            width += $(this).get(0).scrollWidth
        )
        console.log width
        $('.basket-link a').animate({width: width}, 200 );
    )
    $('.basket-link').on('mouseleave', ->
        $('.basket-link a').animate({width: '30px'}, 200 );
    )

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

    (updateGamesCount = ->
        $('#games-count .loader').css('display', '')
        $.ajax(
            type: "GET",
            url: $('#games-count').data('href')
            datatype: "json"
            success: (data, statusText) ->
                $('#games-count .count').text(data)
                $('#games-count .loader').css('display', 'none'))
        setTimeout((-> updateGamesCount()), 60000))()
        
    window.updateBasket = ->
        p = $.get('/api/orders')
        p.success( (data) ->
            $('.items-count').html("#{data.OrderDetails.length}")
            $('.basket-sum').html("(#{data.Price} $)")
        )
        return p
    updateBasket()
    
    $('.buy-link').on('click', (e) -> 
        e.preventDefault()
        id = $(this).data('id')
        $.post("/api/orders/#{id}")
            .success(updateBasket)
     )
        
)        


     

