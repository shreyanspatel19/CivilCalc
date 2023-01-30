/********************************************************************************************************
 *  DATE-PICKER, DATE-TIME-PICKER Initialization
 ********************************************************************************************************/
var UI_DateTime_Pickers = function () {

    var handleMonthPickers = function () {
        
    };

    return {
        //main function to initiate the module
        init: function () {
            handleDatePickers();
            handleDateTimePickers();
            handleMonthPickers();
            //handleRemoveDateTimeValidationRule();
        }
    };

}();


/********************************************************************************************************
 *  DATA MASKING Initialization
 ********************************************************************************************************/
var UI_Data_Mask = function () {

    var handleDatamask = function () {
        $('[mask]').each(function (e) {
            $(this).mask($(this).attr('mask'));
        });
    }

    return {
        //main function to initiate the module
        init: function () {
            handleDatamask();
        }
    };

}();


