$(document).on("click", ".js-bb-confirm-delete", function () {
    var btn = this;

    bootbox.confirm({
        message: "Are you sure want to <b>Delete</b> this record?",
        buttons: {
            confirm: {
                label: "Yes, delete it!",
                className: "btn-brand btn-sm"
            },
            cancel: {
                label: "Cancel",
                className: "btn-label-dark btn-sm"
            }
        },
        callback: function (result) {
            setTimeout(function () {
                checkIfModalOpen();
            }, 500);

            if (result) {
                $(btn).parent("form").submit();
            }
        }
    });
});



$(document).on("click", ".js-bb-confirm-restore", function () {
    var btn = this;

    bootbox.confirm({
        message: "Are you sure want to <b>Restore</b> this record?",
        buttons: {
            confirm: {
                label: "Yes, restore it!",
                className: "btn-brand btn-sm"
            },
            cancel: {
                label: "Cancel",
                className: "btn-label-dark btn-sm"
            }
        },
        callback: function (result) {
            setTimeout(function () {
                checkIfModalOpen();
            }, 500);

            if (result) {
                $(btn).parent("form").submit();
            }

            
        }
    });
});



$(document).on("click", ".js-bb-confirm-custom", function () {
    var btn = this;
    var message = "";
    var callback;
    var btnConfirm;
    var btnCancel;

    //Get Message
    if ($(btn).data("message") != null && $(btn).data("message") !== "") {
        message = $(btn).data("message");
    } else {
        message = "Are you sure want to perform this action?";
    }

    //Get CallBack
    if ($(btn).data("callback") != null && $(btn).data("callback") !== "") {
        callback = $(btn).data("callback");
    }

    //Get btnConfirm
    if ($(btn).data("btn-confirm") != null && $(btn).data("btn-confirm") !== "") {
        btnConfirm = $(btn).data("btn-confirm");
    } else {
        btnConfirm = { "label": "OK", "className": "btn-brand btn-sm" };
    }

    //Get btnCancel
    if ($(btn).data("btn-cancel") != null && $(btn).data("btn-cancel") !== "") {
        btnCancel = $(btn).data("btn-cancel");
    } else {
        btnCancel = { "label": "Cancel", "className": "btn-label-dark btn-sm" };
    }

    //console.log(btnConfirm);

    bootbox.confirm({
        message: message,
        buttons: {
            confirm: {
                label: btnConfirm.label,
                className: btnConfirm.className
            },
            cancel: {
                label: btnCancel.label,
                className: btnCancel.className
            }
        },
        callback: function (result) {
            setTimeout(function () {
                checkIfModalOpen();
            }, 500);

            if (result) {
                if (callback !== undefined) {
                    eval(callback);
                }
            }
        }
    });

    //bootbox.confirm(message,
    //    function (result) {
    //        if (result) {
    //            $(btn).parent("form").submit();
    //        }
    //    });
});