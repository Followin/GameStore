(function() {
  var Modal;

  Modal = (function() {
    var __isAnimating, __modal, __opened;

    function Modal(options, callbacks) {
      var callbacksList;
      callbacksList = callbacks || {};
      this.message = options.message;
      this.headerText = options.headerText;
      this.success = callbacksList.success || function() {};
      this.cancel = callbacksList.cancel || function() {};
      this.close = callbacksList.close || function() {};
    }

    __opened = false;

    __modal = null;

    __isAnimating = false;

    Modal.prototype.open = function() {
      var bodyDiv, buttonsDiv, cancelButton, headerDiv, okayButton;
      if (__isAnimating) {
        return;
      }
      __isAnimating = true;
      __modal = $("<div class='modal'></div>");
      headerDiv = $("<div class=header>" + this.headerText + "</div>");
      bodyDiv = $("<div class='body'>" + this.message + "</div>");
      okayButton = $("<button type='button' class='modal-button okay-button'>Sure</button>");
      cancelButton = $("<button type='button' class='modal-button cancel-button'>Oops</button>");
      buttonsDiv = $("<div class='modal-buttons'></div>");
      buttonsDiv.append(okayButton);
      buttonsDiv.append(cancelButton);
      bodyDiv.append(buttonsDiv);
      __modal.append(headerDiv);
      __modal.append(bodyDiv);
      $('body').append(__modal).css('overflow', 'hidden');
      okayButton.click((function(_this) {
        return function() {
          __modal.remove();
          return _this.success();
        };
      })(this));
      cancelButton.click((function(_this) {
        return function() {
          __modal.remove();
          return _this.cancel();
        };
      })(this));
      return __isAnimating = false;
    };

    return Modal;

  })();

  window.Modal = Modal;

}).call(this);

//# sourceMappingURL=modal.js.map
