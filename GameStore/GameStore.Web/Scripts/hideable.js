(function() {
  $(document).ready(function() {
    $(".hideable ul:not(.hideable > ul)").before($("<button type='button' class='hideable-toggle'></button>"));
    return $('body').on('click', '.hideable-toggle', function() {
      $(this).next('ul').slideToggle();
      return $(this).toggleClass('hidden');
    });
  });

}).call(this);

//# sourceMappingURL=hideable.js.map
