(function() {
  $('.ship-order-button').click(function(e) {
    var $this, href;
    e.preventDefault();
    e.stopPropagation();
    $this = $(this);
    href = $this.attr('href');
    return $.ajax({
      type: "POST",
      url: href,
      success: function(data, statusText) {
        if (data.success === true) {
          $this.closest('tr').find('.shipped-date').text(data.date);
          return $this.parent().html(data.orderStatus);
        }
      }
    });
  });

}).call(this);

//# sourceMappingURL=orders.js.map
