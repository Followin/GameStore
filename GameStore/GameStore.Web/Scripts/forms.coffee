# CoffeeScript

jQuery.fn.insertFormValidation = ->
    $form = $(this)
    $form.validateParse()
    changeFormClass = (inputEl) ->
    
        el = $(inputEl)
        formGroup = el.closest('.form-group')
        if el.is(':focus')  then formGroup.addClass('has-focus') else formGroup.removeClass('has-focus')
        if el.val() == '' then formGroup.addClass('is-empty') else formGroup.removeClass('is-empty')
        
    window.changeFormClass = changeFormClass


    $form.find('.form-group:has("textarea")').addClass('text-area-group')
    $form.find('textarea').autogrow()         

    $form.find('input[type="radio"]').closest('.form-group').addClass('radio-group')
    $form.find('select').closest('.form-group').addClass('select-group')
    $form.find("input[type='checkbox']").closest('.form-group').addClass('checkbox-group')
    $form.find("input[type='file']").closest('.form-group').addClass('file-group')


    inputValGroups = $form.find('.form-group:has("input"), .form-group:has("textarea")')


    for group in inputValGroups
        input = $(group).find("input[type='text'], input[type='password'],
         input[type='email'], input[type='number'], input[type='date'],
          input[type='time'], input[type='tel'], textarea")
        input.on('focus', -> changeFormClass(this))
        input.on('blur', -> changeFormClass(this))
        changeFormClass(input)
        


$('body').insertFormValidation()

$(".action-button").clickRippleEffect()
