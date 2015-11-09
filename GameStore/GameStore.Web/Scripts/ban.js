(function() {
  $('#Permanent').on('change', function() {
    if ($(this).is(":checked")) {
      $('.time-form-group').slideUp();
    }
    if ($(this).is(":not(:checked)")) {
      return $('.time-form-group').slideDown();
    }
  });

}).call(this);

//# sourceMappingURL=ban.js.map
