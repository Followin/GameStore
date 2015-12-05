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
    $('.basket-link').on('mouseenter', function() {
      var width;
      width = 50;
      $.each($('.basket-link .basket-text *'), function() {
        return width += $(this).get(0).scrollWidth;
      });
      console.log(width);
      return $('.basket-link a').animate({
        width: width
      }, 200);
    });
    $('.basket-link').on('mouseleave', function() {
      return $('.basket-link a').animate({
        width: '30px'
      }, 200);
    });
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
    (updateGamesCount = function() {
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
    })();
    window.updateBasket = function() {
      var p;
      p = $.get('/api/orders');
      p.success(function(data) {
        $('.items-count').html("" + data.OrderDetails.length);
        return $('.basket-sum').html("(" + data.Price + " $)");
      });
      return p;
    };
    updateBasket();
    return $('.buy-link').on('click', function(e) {
      var id;
      e.preventDefault();
      id = $(this).data('id');
      return $.post("/api/orders/" + id).success(updateBasket);
    });
  });

}).call(this);

//# sourceMappingURL=layout.js.map
