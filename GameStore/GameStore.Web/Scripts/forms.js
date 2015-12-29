(function() {
  jQuery.fn.insertFormValidation = function() {
    var $form, changeFormClass, group, i, input, inputValGroups, len, results;
    $form = $(this);
    $form.validateParse();
    changeFormClass = function(inputEl) {
      var el, formGroup;
      el = $(inputEl);
      formGroup = el.closest('.form-group');
      if (el.is(':focus')) {
        formGroup.addClass('has-focus');
      } else {
        formGroup.removeClass('has-focus');
      }
      if (el.val() === '') {
        return formGroup.addClass('is-empty');
      } else {
        return formGroup.removeClass('is-empty');
      }
    };
    window.changeFormClass = changeFormClass;
    $form.find('.form-group:has("textarea")').addClass('text-area-group');
    $form.find('textarea').autogrow();
    $form.find('input[type="radio"]').closest('.form-group').addClass('radio-group');
    $form.find('select').closest('.form-group').addClass('select-group');
    $form.find("input[type='checkbox']").closest('.form-group').addClass('checkbox-group');
    $form.find("input[type='file']").closest('.form-group').addClass('file-group');
    inputValGroups = $form.find('.form-group:has("input"), .form-group:has("textarea")');
    results = [];
    for (i = 0, len = inputValGroups.length; i < len; i++) {
      group = inputValGroups[i];
      input = $(group).find("input[type='text'], input[type='password'], input[type='email'], input[type='number'], input[type='date'], input[type='time'], input[type='tel'], textarea");
      input.on('focus', function() {
        return changeFormClass(this);
      });
      input.on('blur', function() {
        return changeFormClass(this);
      });
      results.push(changeFormClass(input));
    }
    return results;
  };

  $('body').insertFormValidation();

  $(".action-button").clickRippleEffect();

}).call(this);

//# sourceMappingURL=forms.js.map
