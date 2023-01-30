var InitMain = function () {
    return {
        //main function to initiate the module
        init: function () {
            //UI_DateTime_Pickers.init();
            //UI_Data_Mask.init();
            //UI_Select2.init();
            //UI_TinyMCE.init();
            //UI_Selectize.init();

            //Other small function
            $('form').each(function () {
                $(this).attr('autocomplete', 'off');
            });
            //$('[data-toggle="kt-tooltip"]').tooltip();
        }
    };

}();

$(document).ready(function () {
    InitMain.init();
    //UI_Currency.init();
});

$(document).ajaxStart(function () {

    //funBlockUI();

}).ajaxStop(function () {

    //funUnblockUI();

    InitMain.init();


    //UI_Currency.init();

    //For validating Form in Modal
    $.validator.unobtrusive.parse($("form"));

    KTApp.init(KTAppOptions);


    //if (typeof fn_redirectToLogout !== 'undefined' && $.isFunction(fn_redirectToLogout)) {
    if (typeof fn_redirectToLogout !== 'undefined') {
        //execute it

        // similar behavior as an HTTP redirect
        //window.location.replace("http://stackoverflow.com");

        // similar behavior as clicking on a link
        //window.location.href = "http://stackoverflow.com";
    }

});