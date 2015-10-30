(function() {
  var checkFilters, filters, filtersOnly, indexPostLink;

  filters = $('form').serialize();

  filtersOnly = $('form.filters-form').clone();

  indexPostLink = $('form').attr('action');

  $('.filter-button').on('click', function(e) {
    var forms, serializedForms;
    e.preventDefault();
    forms = $('form');
    serializedForms = forms.serialize();
    return $.ajax({
      url: forms.attr('action'),
      type: "POST",
      data: forms.serialize(),
      success: function(data, statusText) {
        filters = serializedForms;
        filtersOnly = $('form.filters-form').serialize();
        $('.games-list').html(data);
        return $('input[type=submit]').prop('disabled', true);
      }
    });
  });

  $('#body-wrapper').on('click', '.page-link', function(e) {
    e.preventDefault();
    return $.ajax({
      url: indexPostLink + "?filterModel.page=" + ($(this).data('page')),
      type: "POST",
      data: filtersOnly.add('form.pagin-form').serialize(),
      success: function(data, statusText) {
        return $('.games-list').html(data);
      }
    });
  });

  checkFilters = function() {
    var forms;
    forms = $('form');
    if (filters === forms.serialize() || !forms.valid()) {
      $('input[type=submit]').prop('disabled', true);
      return;
    }
    return $('input[type=submit]').prop('disabled', false);
  };

  $('.filters-form input').on('change', function() {
    return checkFilters();
  });

  $('.filters-form input').on('input', function() {
    return checkFilters();
  });

  $('.filters-form select').on('change', function() {
    return checkFilters();
  });

  $('.pagin-form').on('change', 'select', function() {
    return $.ajax({
      url: indexPostLink,
      type: "POST",
      data: filtersOnly.add('form.pagin-form').serialize(),
      success: function(data, statusText) {
        return $('.games-list').html(data);
      }
    });
  });

  $('.filterin-block-switch').on('click', function() {
    var $arrow, $filterinBlock;
    $filterinBlock = $('.filterin-block');
    $arrow = $(this).find('.arrow');
    if ($filterinBlock.hasClass('minified')) {
      $filterinBlock.removeClass('minified');
      return $arrow.removeClass('right').addClass('left');
    } else {
      $filterinBlock.addClass('minified');
      return $arrow.removeClass('left').addClass('right');
    }
  });

}).call(this);

//# sourceMappingURL=games.js.map
