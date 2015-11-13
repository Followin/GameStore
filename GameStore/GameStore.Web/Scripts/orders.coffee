# CoffeeScript

$('.ship-order-button').click( (e) ->
    e.preventDefault()
    e.stopPropagation()
    $this = $(this)
    href = $this.attr('href')
    $.ajax(
        type: "POST"
        url: href
        success: (data, statusText) ->
            if(data.success == true)
                $this.closest('tr').find('.shipped-date').text(data.date)
                $this.parent().html(data.orderStatus)
    )
)