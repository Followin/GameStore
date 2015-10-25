# CoffeeScript

$('#body-wrapper').on('click', '.answer-to-comment', (e) ->
    $this = $(this)
    if $this.hasClass('answerin-to')
        $('#CreateModel_ParentCommentId').val('')
        $this.removeClass('answerin-to')
    else
        $('.answerin-to').removeClass('answerin-to')
        $('#CreateModel_ParentCommentId').val($this.data('id'))
        $('#CreateModel_Body').val(
            $this.closest('.game-comment').find('.author-name').text() + ', ' +
            $('#CreateModel_Body').val())
        $this.addClass('answerin-to')
    changeFormClass($('#CreateModel_Body'))
)

selection = null

$('#body-wrapper').on('mousedown', (e) ->
    selected = window.getSelection().rangeCount == 1
    if selected
        selection = window.getSelection().getRangeAt(0)
)

$('#body-wrapper').on('click', '.quote-comment', (e) ->
    e.preventDefault()
    $this = $(this)
    if $this.hasClass('quotin')
        $("quote[data-id='#{$this.data('id')}']").remove()
        $this.removeClass('quotin')
    else    
        $gameComment = $this.closest('.game-comment')
        $commentBody = $gameComment.find('.comment-body')
        
        if selection and selection.toString() != "" and
        selection.intersectsNode($commentBody[0]) and
        selection.startContainer == selection.endContainer
            text = selection.toString()
        else
            text = $commentBody.html()
        author = $gameComment.find('.author-name').text()
        $('.quotes').append("<quote data-id='#{$this.data('id')}'>
            <h4 class='quote-author-name'>
            <b>#{author}</b> wrote</h4><div class='quote-body'>#{text}</div></quote>")
        $this.addClass('quotin')
)

$("input[type='submit']").on('click', (e) -> 
    e.preventDefault()
    body = $('textarea')
    
    $.ajax(
        type: "POST"
        url: "/game/dota-2/newcomment"
        contentType: "application/json"
        data: 
            JSON.stringify(
                "CreateModel.ParentCommentId": $("#CreateModel_ParentCommentId").val()
                "CreateModel.Name": $("#CreateModel_Name").val()
                "CreateModel.Quotes": $('.quotes').html()
                "CreateModel.Body": body.val()
            )
        success: ->
            location.reload()
    )
)

$('#body-wrapper').on('click', '.delete-comment-button', ->
    location.href=$(this).data('href')
)

