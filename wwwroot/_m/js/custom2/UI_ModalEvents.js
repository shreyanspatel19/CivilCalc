// FUN: Generate HTML Modal
function funCreateBlankModal(modalId, modalSize, modalContent) {
    var $modalHtml = "" +
        "<div class='modal fade' id='" + modalId + "' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'>" +
        "<div class='modal-dialog modal-" + modalSize + "'>" +
        "<div class='modal-content'>" +
        modalContent +
        "<span id='hd-modal-id' style='display: none;'>#" + modalId + "</span>" +
        "</div>" +
        "</div>" +
        "</div>";
    $('#modal-placeholder').append($modalHtml);
    $("#" + modalId).modal({
        backdrop: 'static',
        keyboard: false
    });
    $("#" + modalId).modal('show');

    $("#" + modalId).on('hidden.bs.modal',
        function (e) {
            $(this).remove();

            var modelplaceholderLength = $('#modal-placeholder').html().length;
            if (modelplaceholderLength > 0) {
                $("body").addClass("modal-open");
            } else {
                $("body").removeClass("modal-open");
            }
        });
}

// FUN: Modal Button Click Event
$(document).on("click", ".js-stkModal-btn", function () {
    var btn = this;
    var formId = $(btn).attr("data-form-id");
    var url = $(btn).attr("data-url");
    var modalSize = $(btn).attr("data-modal-size");

    var modalId = "myModal" + (url.split('?')[0]).split("/").join("-");

    $.ajax({
        url: url,
        method: "GET",
        dataType: "html",
        success: function (result, status, xhr) {
            funCreateBlankModal(modalId, modalSize, xhr.responseText);
            OnSuccess(formId, xhr.responseText);
        },
        error: function (xhr, status, error) {
            OnFailure(xhr, status, error);
        }
    });
});




$(document).on("click", ".js-btn-modal-form-submit", function (e) {
    console.log('clicked .js-btn-modal-form-submit');
    e.preventDefault();

    var btn = this;
    var hdModalId = $(btn).closest('.modal-content').find("#hd-modal-id").html();

    var vSubmitModalForm = true;

    // Check Custom Form Validation JS before submit 
    // if attribute "data-check-before-submit" is set on "Submit" Button
    if ($(btn).data("check-before-submit") !== null && $(btn).data("check-before-submit") !== "" && $(btn).data("check-before-submit") !== undefined) {
        vSubmitModalForm = eval($(btn).data("check-before-submit"));  // this function returns true/false
    }

    if (vSubmitModalForm) {
        $(btn).closest('form').submit(function (event) {
            if ($(this).valid()) {
                $(hdModalId).modal('hide');
            }
        });
        $(btn).closest('form').submit();
    }
});


// Adjusting Z-index of Modal
$(document).on('show.bs.modal', '.modal', function (event) {
    var zIndex = 1040 + (10 * $('.modal:visible').length);
    $(this).css('z-index', zIndex);
    setTimeout(function () {
        $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
    }, 0);
});

//____________________________________________________________________________________________


$(".modal").on("hidden", function () {
    console.log("on modal close");
    var modelContentLength = $('#modal-placeholder').html().length;
});




