(function() {
  $('#body-wrapper').on('click', '.answer-to-comment', function(e) {
    var $this;
    $this = $(this);
    $('#CreateModel_ParentCommentId').val($this.data('id'));
    return $('#CreateModel_Body').val('').val($this.closest('.game-comment').find('.author-name').text() + ', ');
  });

}).call(this);

//# sourceMappingURL=comments.js.map
