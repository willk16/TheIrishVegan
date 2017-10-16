



$(function () {
    var ajaxForSubmit = function () {
        var $form = $(this);
    };

    var createAutocomplete = function () {
        var $input = $(this);

        var options = {
            source: $input.attr("data-otf-autocomplete")
        };

        $input.autocomplete(options);
    }

    $("form[data-otf-ajax='true']").submit(ajaxFormSubmit);
    $("input[data-otf-autocomplete]").each(createAutocomplete);
});