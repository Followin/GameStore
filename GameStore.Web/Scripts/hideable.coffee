# CoffeeScript

    console.dir $('.hideable ul')
    $(".hideable ul:not(.hideable > ul)").before($("<button type='button' class='hideable-toggle'></button>"))
    $('.hideable-toggle').on('click', ->
        $(this).next('ul').slideToggle()
        $(this).toggleClass('hidden')
    )
