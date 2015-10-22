# CoffeeScript

$('#body-wrapper').on('click', '.answer-to-comment', (e) ->
    $this = $(this)
    $('#CreateModel_ParentCommentId').val($this.data('id'))
    $('#CreateModel_Body').val('').val(
        $this.closest('.game-comment').find('.author-name').text() + ', ')
        
    
)