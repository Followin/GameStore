(function() {
  $('.filter-button').on('click', function(e) {
    var forms;
    e.preventDefault();
    forms = $('form');
    return $.ajax({
      url: forms.attr('action'),
      type: "POST",
      data: forms.serialize(),
      success: function(data, statusText) {
        return $('.games-list').html(data);
      }
    });
  });

  $('#body-wrapper').on('click', '.page-link', function(e) {
    var forms;
    e.preventDefault();
    forms = $('form');
    return $.ajax({
      url: forms.attr('action') + ("?filterModel.page=" + ($(this).data('page'))),
      type: "POST",
      data: forms.serialize(),
      success: function(data, statusText) {
        return $('.games-list').html(data);
      }
    });
  });

}).call(this);

//# sourceMappingURL=games.js.map
