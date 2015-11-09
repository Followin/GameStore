# CoffeeScript

$('#Permanent').on('change', ->
    $('.time-form-group').slideUp() if $(this).is(":checked")
    $('.time-form-group').slideDown() if $(this).is(":not(:checked)")
)