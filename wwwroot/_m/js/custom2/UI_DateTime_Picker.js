/********************************************************************************************************
 *  DATE-PICKER, DATE-TIME-PICKER Initialization
 ********************************************************************************************************/
var UI_DateTime_Pickers = function () {

    //var handleRemoveDateTimeValidationRule = function () {
    //    setTimeout(function () {
    //        //$('.js-date-picker, .js-date-time-picker, .js-month-picker').each(function () {
    //        //    $(this).rules("remove", "date");
    //        //});

    //        //$(".js-date-picker, .js-date-time-picker, .js-month-picker").attr("readonly", "");
    //    }, 0);
    //}

    $.validator.methods.date = function(value, element) {
        var pattern = /(\d{2})-(\d{2})-(\d{4})/;
        var dt = new Date(value.replace(pattern, '$3-$2-$1'));
        return this.optional(element) || !/Invalid|NaN/.test(new Date(dt).toString());
    };


    var arrows = {
        leftArrow: '<i class="la la-angle-left"></i>',
        rightArrow: '<i class="la la-angle-right"></i>'
    }

    var handleDatePickers = function() {
        $('.js-date-picker').datepicker({
            format: "dd-mm-yyyy",
            todayBtn: "linked",
            orientation: "bottom auto",
            //daysOfWeekDisabled: "0,6",
            daysOfWeekHighlighted: "0",
            autoclose: true,
            clearBtn: true,
            todayHighlight: true,
            templates: arrows
        }).on("changeDate",
            function(e) {
                this.focus();
            });
    };

    var handleDatePickersDiffFormat = function () {
        $('.js-date-picker-diff-format').datepicker({
            format: "yyyy-mm-dd",
            todayBtn: "linked",
            orientation: "bottom auto",
            //daysOfWeekDisabled: "0,6",
            daysOfWeekHighlighted: "0",
            autoclose: true,
            clearBtn: true,
            todayHighlight: true,
            templates: arrows
        }).on("changeDate",
            function (e) {
                this.focus();
            });
    };

    var handleDateTimePickers = function() {
        $('.js-date-time-picker').datetimepicker({
            todayBtn: "linked",
            format: 'dd-mm-yyyy hh:ii',
            orientation: "bottom auto",
            //daysOfWeekDisabled: "0,6",
            daysOfWeekHighlighted: "0",
            autoclose: true,
            clearBtn: true,
            todayHighlight: true,
            showMeridian: true
        }).on("changeDate",
            function(e) {
                this.focus();
            });
        //$('.datetimepicker .glyphicon.glyphicon-arrow-left').addClass('fa fa-angle-left');
        //$('.datetimepicker .glyphicon.glyphicon-arrow-left').removeClass('glyphicon glyphicon-arrow-left');
        //$('.datetimepicker .glyphicon.glyphicon-arrow-right').addClass('fa fa-angle-right');
        //$('.datetimepicker .glyphicon.glyphicon-arrow-right').removeClass('glyphicon glyphicon-arrow-right');
    };

    var handleMonthPickers = function() {
        $('.js-month-picker').datepicker({
            format: "M-yyyy",
            todayBtn: "linked",
            orientation: "bottom auto",
            //daysOfWeekDisabled: "0,6",
            daysOfWeekHighlighted: "0",
            autoclose: true,
            clearBtn: true
        }).on("changeDate",
            function(e) {
                this.focus();
            });
    };


    var handleDateRangeList = function () {
        $('.kt-daterange-list-item').on("click", function() {
            //console.log(this);
            //console.log($(this).html());

            var vParentID = $(this).closest('div').attr("data-parent-id");
            var vFromDateID = $(this).closest('div').attr("data-from-date-id");
            var vToDateID = $(this).closest('div').attr("data-to-date-id");

            var vFromDateValue = $(this).attr("data-from-date-value");
            var vToDateValue = $(this).attr("data-to-date-value");

            $("[data-daterange-button='" + vParentID + "'] span").html($(this).html());

            if (typeof $(this).data('isCustom') !== 'undefined') {
                //console.log("Custom Range");
                $("#" + vParentID + " #" + vFromDateID).prop('readonly', false);
                $("#" + vParentID + " #" + vToDateID).prop('readonly', false);

                $("#" + vParentID + " #" + vFromDateID).removeClass('g-bg-gray-light-v5');
                $("#" + vParentID + " #" + vToDateID).removeClass('g-bg-gray-light-v5');

                $(this).closest('div').find("[name='" + vFromDateID + "']").prop('readonly', true);
                $(this).closest('div').find("[name='" + vToDateID + "']").prop('readonly', true);
            } else {
                //console.log(vParentID);
                //console.log(vFromDateID);
                //console.log(vFromDateValue);
                //console.log(vToDateID);
                //console.log(vToDateValue);

                $("#" + vParentID + " #" + vFromDateID).val(vFromDateValue);
                $("#" + vParentID + " #" + vToDateID).val(vToDateValue);

                $("#" + vParentID + " #" + vFromDateID).prop('readonly', true);
                $("#" + vParentID + " #" + vToDateID).prop('readonly', true);

                $("#" + vParentID + " #" + vFromDateID).addClass('g-bg-gray-light-v5');
                $("#" + vParentID + " #" + vToDateID).addClass('g-bg-gray-light-v5');

                //$("#" + vParentID + " #" + vFromDateID).trigger("change");
                $(this).closest('div').find("[name='" + vFromDateID + "']").prop('readonly', false);
                $(this).closest('div').find("[name='" + vToDateID + "']").prop('readonly', false);

                $(this).closest('div').find("[name='" + vFromDateID + "']").val(vFromDateValue);
                $(this).closest('div').find("[name='" + vToDateID + "']").val(vToDateValue);
            }
        });
    };

    return {
        //main function to initiate the module
        init: function () {
            handleDatePickers();
            handleDateTimePickers();
            handleMonthPickers();
            handleDateRangeList();
            handleDatePickersDiffFormat();
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
            var mask = $(this).attr('mask');
            var vAutoUnmask;

            if ($(this).data('autoUnmask') !== undefined && $(this).data('autoUnmask') !== "") {
                vAutoUnmask = true;
            }

            console.log(vAutoUnmask);
            $(this).inputmask(mask, {
                autoUnmask: vAutoUnmask,
                "clearIncomplete": true
            });
        });

        


        $("#kt_inputmask_1").inputmask("99/99/9999", {
            "placeholder": "mm/dd/yyyy",
            autoUnmask: true
        });
    }

    return {
        //main function to initiate the module
        init: function () {
            handleDatamask();
        }
    };

}();


