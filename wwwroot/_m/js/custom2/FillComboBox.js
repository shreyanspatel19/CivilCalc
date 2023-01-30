/***********************************************************
 * Fill Specialization By Course
 * @param {number} CourseID Required for Filter.
 * @param {string} comboboxSelector Required for filling combobox on this selector.
 ***********************************************************/
function Common_fn_FillSpecializationByFilter(CourseID, comboboxSelector) {
    if (CourseID !== "") {
        $(comboboxSelector).empty();
        $(comboboxSelector).append($("<option></option>").val("").html("--"));
        $.ajax(
            {
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Common/FillSpecializationByFilter?&CourseID=" + CourseID,
                data: {},
                dataType: "json",
                success: function(Result) {
                    $.each(Result,
                        function(key, value) {
                            $(comboboxSelector).append($("<option></option>").val(value.SpecializationID)
                                .html(value.SpecializationName + " (" + value.SpecializationGTUCode + ")"));
                        });
                },
                error: function(r) {
                    alert("Error while loading combobox.");
                }
            });
    } else {
        alert('Value of \'CourseID\' not found.');
    }
}


/***********************************************************
 * Fill Semester By Course
 * @param {number} CourseID Required for Filter.
 * @param {string} comboboxSelector Required for filling combobox on this selector.
 ***********************************************************/
function Common_fn_FillCourseWiseSemester(CourseID, comboboxSelector) {
    if (CourseID !== "") {
        $(comboboxSelector).empty();
        $(comboboxSelector).append($("<option></option>").val("").html("--"));
        $.ajax(
            {
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Common/FillCourseWiseSemester?&CourseID=" + CourseID,
                data: {},
                dataType: "json",
                success: function (Result) {
                    $.each(Result,
                        function (key, value) {
                            $(comboboxSelector).append($("<option></option>").val(value.Semester)
                                .html(value.Semester));
                        });
                },
                error: function (r) {
                    alert("Error while loading combobox.");
                }
            });
    } else {
        alert('Value of \'CourseID\' not found.');
    }
}
