var UI_Selectize = function () {

    var handleSelectize = function () {

        var $element = $('select').not('.no-select2').not('.dataTables_length select').not('.kt-select2-data-ajax').not('.kt-select2-data-ajax1').not('.kt-select2-data-ajax2');

        
        $element.selectize({
            selectOnTab: true,
            allowEmptyOption: true,
            placeholder: "Type to Filter",
            //inputClass: 'form-control selectize-input',
            //dropdownParent: "body"
        });
    }

    return {
        //main function to initiate the module
        init: function () {
            handleSelectize();
        }
    };

}();



$(document).ready(function () {
    UI_Selectize.init();
});

$(document).ajaxStart(function () {
}).ajaxStop(function () {
    UI_Selectize.init();
});




/*==================================================================================================*/
/* Begin:: fnSelectize_UpdatedComboBox
 * This function is not required if we are using fnSelectize_GetComboDataJSon for
 * getting JSon Data and directly add option to selectize.
 */
function fnSelectize_UpdatedComboBox(target, optionArr) {
    $(target).selectize()[0].selectize.destroy();
    $(target).selectize({
        selectOnTab: true,
        allowEmptyOption: true,
        options: optionArr,
        inputClass: 'form-control selectize-input',
        //dropdownParent: "body"
    });
}
/*==================================================================================================*/




/*==================================================================================================*/
/* Begin:: fnSelectize_UpdatedComboBox
 */
function fnSelectize_GetComboDataJSon(url, method = 'GET') {
    var vJsonReturn = new Array();
    $.ajax(
        {
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: url,
            data: {},
            dataType: "json",
            success: function (Result) {
                for (var i in Result)
                    vJsonReturn.push([Result[i]]);
            },
            error: function (r) {
                alert("Error while loading ComboBox.");
            }
        });
    return vJsonReturn;
}
/*==================================================================================================*/




/*==================================================================================================*/
/* Begin:: Get Department by InstituteID
 */
function fnSelectize_FillCombo_DepartmentByInstituteID(InstituteID, target) {
    var selectize = $(target)[0].selectize;

    var vJsonData = [];
    if (InstituteID !== "" && InstituteID !== null) {
        vJsonData = fnSelectize_GetComboDataJSon("/Common/FillDepartment/" + InstituteID);
    }

    setTimeout(function () {
        if (vJsonData.length > 0) {
            selectize.setValue('', false);
            selectize.clearOptions();
            $.each(vJsonData,
                function(key, value) {
                    selectize.addOption({
                        value: value[0].DepartmentID,
                        text: value[0].DepartmentShortName
                    });
                });
        } else {
            selectize.setValue('', false);
            selectize.clearOptions();
        }
    },20);
}
/*==================================================================================================*/