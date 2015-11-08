# CoffeeScript
$(document).ready(->
    $(".hideable ul:not(.hideable > ul)").before($("<button type='button' class='hideable-toggle'></button>"))
    $('body').on('click', '.hideable-toggle', ->
        $(this).next('ul').slideToggle()
        $(this).toggleClass('hidden')
    )
)