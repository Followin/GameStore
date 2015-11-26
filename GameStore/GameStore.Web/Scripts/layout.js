(function() {
  $(document).ready(function() {
    var bodyHeight, messageTimeout, objHeight, updateGamesCount, viewportHeight;
    window.onscroll = function() {
      var scrolled;
      scrolled = window.pageYOffset || document.documentElement.scrollTop;
      if (scrolled > 5) {
        return $('header').addClass('mini');
      } else {
        return $('header').removeClass('mini');
      }
    };
    objHeight = 0;
    if ($('.paper-block')[0] !== void 0) {
      $.each($('paper-block').children(":not(:hidden)"), function() {
        return objHeight += $(this).height();
      });
    }
    bodyHeight = $('body').height();
    viewportHeight = document.documentElement.clientHeight || window.innerHeight || 0;
    $('body').height(Math.max(viewportHeight, objHeight, bodyHeight));
    messageTimeout = function(message) {
      return setTimeout((function(_this) {
        return function() {
          message.slideUp();
          return setTimeout(function() {
            return message.remove();
          }, 500);
        };
      })(this), 5000);
    };
    $.each($('.temp-message'), function() {
      $(this).slideDown();
      if (!$(this).is('.temp-message-error')) {
        return messageTimeout($(this));
      }
    });
    window.addMessage = function(type, message) {
      var $message, $tempMessagesContainer;
      $tempMessagesContainer = $('.temp-messages');
      $message = $("<div class='temp-message temp-message-" + type + "'> " + message + "</div>");
      $tempMessagesContainer.prepend($message);
      $message.slideDown();
      if (type !== 'error') {
        return messageTimeout($message);
      }
    };
    $.each($('.validation-summary-errors li'), function() {
      return addMessage('error', $(this).html());
    });
    $('#close-messages-button').click(function() {
      return $('.temp-message').slideUp();
    });
    $('body').on('click', '.clickable-row', function() {
      return window.document.location = $(this).data('href');
    });
    updateGamesCount = function() {
      $('#games-count .loader').css('display', '');
      $.ajax({
        type: "GET",
        url: $('#games-count').data('href'),
        datatype: "json",
        success: function(data, statusText) {
          $('#games-count .count').text(data);
          return $('#games-count .loader').css('display', 'none');
        }
      });
      return setTimeout((function() {
        return updateGamesCount();
      }), 60000);
    };
    return updateGamesCount();
  });

}).call(this);

//# sourceMappingURL=layout.js.map
