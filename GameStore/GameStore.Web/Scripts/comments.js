(function() {
  $(function() {
    var commentsHub, selection;
    commentsHub = $.connection.commentsHub;
    commentsHub.client.addComment = function(id, parentId, name, quotes, body) {
      var commentLi, parentLi, ul;
      if (parentId !== '') {
        parentLi = $(".game-comment[data-id=" + parentId + "]").closest('li');
        parentLi.append($("<button type='button' class='hideable-toggle'></button>"));
        if (!parentLi.is(":has('ul')")) {
          parentLi.append($("<ul></ul>"));
        }
        ul = parentLi.find('ul');
      } else {
        ul = $('.comments-list > ul');
      }
      commentLi = $("<li> <div class='game-comment' data-id=" + id + "> <button class='delete-comment-button'>x</button> <h3 class='author-name'>" + name + "</h3> <div class='comment-body'>" + (quotes + body) + "</div> <div class='comment-buttons'> <a class='comment-button answer-to-comment' data-id='" + id + "'>Answer</a> <a class='comment-button quote-comment' data-id='" + id + "'>Quote</a> </div> </div> </li>");
      return ul.append(commentLi);
    };
    commentsHub.client.deleteComment = function(id) {
      var parentLi, parentUl;
      parentLi = $(".game-comment[data-id=" + id + "]").closest('li');
      parentUl = parentLi.parent();
      parentLi.remove();
      if (parentUl.is(":empty")) {
        parentUl.prev('.hideable-toggle').remove();
        if (!parentUl.parent().is('.comments-list')) {
          return parentUl.remove();
        }
      }
    };
    commentsHub.client.changeCommentBody = function(id, text) {
      var commentDiv;
      commentDiv = $(".game-comment[data-id=" + id + "]");
      return commentDiv.find('.comment-body').text(text);
    };
    $.connection.hub.start().done(function() {
      commentsHub.server.joinGroup($('#GameId').val());
      window.onbeforeunload = function() {
        commentsHub.server.leaveGroup($('#GameId').val());
        return window.close();
      };
      $("input[type='submit']").on('click', function(e) {
        var body;
        e.preventDefault();
        body = $('textarea');
        if ($('form').valid()) {
          commentsHub.server.createComment($("#GameId").val(), $("#CreateModel_ParentCommentId").val(), $("#CreateModel_Name").val(), $(".quotes").html(), body.val());
          $("#CreateModel_ParentCommentId").val('');
          $("#CreateModel_Name").val('');
          $(".quotes").html('');
          body.val('');
          $('.answerin-to').removeClass('answerin-to');
          return $('.quotin').removeClass('quotin');
        }
      });
      return $('#body-wrapper').on('click', '.delete-comment-button', function() {
        var id, modal;
        id = $(this).closest('.game-comment').data('id');
        modal = new Modal({
          headerText: "Confirm",
          message: "Are you sure, you wanna delete this comment?"
        }, {
          success: function() {
            commentsHub.server.deleteComment($("#GameId").val(), id);
            return $('#body-wrapper').removeClass('blured');
          },
          cancel: function() {
            return $('#body-wrapper').removeClass('blured');
          }
        });
        $('#body-wrapper').addClass('blured');
        return modal.open();
      });
    });
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
    return $('#body-wrapper').on('click', '.quote-comment', function(e) {
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
  });

}).call(this);

//# sourceMappingURL=comments.js.map
