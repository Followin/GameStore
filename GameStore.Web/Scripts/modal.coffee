# CoffeeScript
class Modal
    constructor: (options, callbacks) ->
        callbacksList = callbacks || {}
        @message = options.message
        @headerText = options.headerText
        
        @success = callbacksList.success || ->
        @cancel = callbacksList.cancel || ->
        @close = callbacksList.close || ->
        
    __opened = false
    __modal = null
    __isAnimating = false
    
    open: ->
        return if __isAnimating     
               
        __isAnimating = true
        __modal = $("<div class='modal'></div>")
        headerDiv = $("<div class=header>#{@headerText}</div>")
        
        bodyDiv = $("<div class='body'>#{@message}</div>")
        
        okayButton = $("<button type='button' class='modal-button okay-button'>Sure</button>")
        
        cancelButton = $("<button type='button' class='modal-button cancel-button'>Oops</button>") 
        
        buttonsDiv = $("<div class='modal-buttons'></div>")
        buttonsDiv.append(okayButton)
        buttonsDiv.append(cancelButton)
        
        bodyDiv.append(buttonsDiv)
        
        __modal.append(headerDiv)
        __modal.append(bodyDiv)
            
        $('body').append(__modal).css('overflow', 'hidden')
        
        okayButton.click( =>
            @success()
            __modal.remove()
            $('body').css('overflow', '')
        )
        
        cancelButton.click( =>
            @cancel()
            __modal.remove()
            $('body').css('overflow', '')
        )
        
        __isAnimating = false
        
        
window.Modal = Modal    