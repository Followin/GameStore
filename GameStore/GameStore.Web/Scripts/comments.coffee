# CoffeeScript
$(->
    commentsHub = $.connection.commentsHub
    
    commentsHub.client.addComment = (id, parentId, name, quotes, body) ->
        if parentId isnt ''
            parentLi = $(".game-comment[data-id=#{parentId}]").closest('li')
            parentLi.append($("<button type='button' class='hideable-toggle'></button>"))
            parentLi.append($("<ul></ul>")) if not parentLi.is(":has('ul')")
            ul = parentLi.find('ul')
        else ul = $('.comments-list > ul')
        
        commentLi = $("
            <li>
                <div class='game-comment' data-id=#{id}>
                    <button class='delete-comment-button'>x</button>
                    <h3 class='author-name'>#{name}</h3>
                    <div class='comment-body'>#{quotes + body}</div>
                    <div class='comment-buttons'>
                        <a class='comment-button answer-to-comment' data-id='#{id}'>Answer</a>
                        <a class='comment-button quote-comment' data-id='#{id}'>Quote</a>
                    </div>
                </div>
            </li>")
        ul.append(commentLi)
    
    commentsHub.client.deleteComment = (id) ->
        parentLi = $(".game-comment[data-id=#{id}]").closest('li')
        parentUl = parentLi.parent()
        parentLi.remove()
        if parentUl.is(":empty")
            parentUl.prev('.hideable-toggle').remove()
            if not parentUl.parent().is('.comments-list')
                parentUl.remove()
        
    commentsHub.client.changeCommentBody = (id, text) ->
        commentDiv = $(".game-comment[data-id=#{id}]")
        commentDiv.find('.comment-body').text(text)
        
    $.connection.hub.start().done( ->
        commentsHub.server.joinGroup($('#GameId').val())
        
        window.onbeforeunload = ->
            commentsHub.server.leaveGroup($('#GameId').val())
            window.close()
            
        $("input[type='submit']").on('click', (e) -> 
            e.preventDefault()
            body = $('textarea')
            if $('form').valid()
                commentsHub.server.createComment(
                    $("#GameId").val(),
                    $("#CreateModel_ParentCommentId").val(),
                    $("#CreateModel_Name").val(),
                    $(".quotes").html(),
                    body.val()
                 )
                 
                $("#CreateModel_ParentCommentId").val('')
                $("#CreateModel_Name").val('')
                $(".quotes").html('')
                body.val('')
                $('.answerin-to').removeClass('answerin-to')
                $('.quotin').removeClass('quotin')
         )
         
        $('#body-wrapper').on('click', '.delete-comment-button', ->
            id = $(this).closest('.game-comment').data('id')
            modal = new Modal(
                {
                    headerText: "Confirm"
                    message: "Are you sure, you wanna delete this comment?"
                }
                ,
                {
                    success: ->
                        commentsHub.server.deleteComment($("#GameId").val(),id)
                        $('#layout').removeClass('blured')
                    cancel: ->
                        $('#layout').removeClass('blured')
                }
            )
            $('#layout').addClass('blured')
            modal.open()
            )
     )
    
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


    
)

