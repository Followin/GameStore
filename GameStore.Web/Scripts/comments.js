(function() {
  var selection;

  $('#body-wrapper').on('click', '.answer-to-comment', function(e) {
    var $this;
    $this = $(this);
    if ($this.hasClass('answerin-to')) {
      $('#CreateModel_ParentCommentId').val('');
      $this.removeClass('answerin-to');
    } else {
      $('.answerin-to').removeClass('answerin-to');
      $('#CreateModel_ParentCommentId').val($this.data('id'));
      $('#CreateModel_Body').val($this.closest('.game-comment').find('.author-name').text() + ', ' + $('#CreateModel_Body').val());
      $this.addClass('answerin-to');
    }
    return changeFormClass($('#CreateModel_Body'));
  });

  selection = null;

  $('#body-wrapper').on('mousedown', function(e) {
    var selected;
    selected = window.getSelection().rangeCount === 1;
    if (selected) {
      return selection = window.getSelection().getRangeAt(0);
    }
  });

  $('#body-wrapper').on('click', '.quote-comment', function(e) {
    var $commentBody, $gameComment, $this, author, text;
    e.preventDefault();
    $this = $(this);
    if ($this.hasClass('quotin')) {
      $("quote[data-id='" + ($this.data('id')) + "']").remove();
      return $this.removeClass('quotin');
    } else {
      $gameComment = $this.closest('.game-comment');
      $commentBody = $gameComment.find('.comment-body');
      if (selection && selection.toString() !== "" && selection.intersectsNode($commentBody[0]) && selection.startContainer === selection.endContainer) {
        text = selection.toString();
      } else {
        text = $commentBody.html();
      }
      author = $gameComment.find('.author-name').text();
      $('.quotes').append("<quote data-id='" + ($this.data('id')) + "'> <h4 class='quote-author-name'> <b>" + author + "</b> wrote</h4><div class='quote-body'>" + text + "</div></quote>");
      return $this.addClass('quotin');
    }
  });

  $("input[type='submit']").on('click', function(e) {
    var body;
    e.preventDefault();
    body = $('textarea');
    return $.ajax({
      type: "POST",
      url: $('form').attr('action'),
      contentType: "application/json",
      data: JSON.stringify({
        "CreateModel.ParentCommentId": $("#CreateModel_ParentCommentId").val(),
        "CreateModel.Name": $("#CreateModel_Name").val(),
        "CreateModel.Quotes": $('.quotes').html(),
        "CreateModel.Body": body.val()
      }),
      success: function() {
        return location.reload();
      }
    });
  });

  $('#body-wrapper').on('click', '.delete-comment-button', function() {
    var href, modal;
    href = $(this).data('href');
    modal = new Modal({
      headerText: "Confirm",
      message: "Are you sure, you wanna delete this comment?"
    }, {
      success: function() {
        location.href = href;
        return $('#body-wrapper').removeClass('blured');
      },
      cancel: function() {
        return $('#body-wrapper').removeClass('blured');
      }
    });
    $('#body-wrapper').addClass('blured');
    return modal.open();
  });

}).call(this);

//# sourceMappingURL=comments.js.map
