(function() {
  console.dir($('.hideable ul'));

  $(".hideable ul:not(.hideable > ul)").before($("<button type='button' class='hideable-toggle'></button>"));

  $('.hideable-toggle').on('click', function() {
    $(this).next('ul').slideToggle();
    return $(this).toggleClass('hidden');
  });

}).call(this);

//# sourceMappingURL=hideable.js.map
