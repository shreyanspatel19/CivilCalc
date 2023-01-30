/***********************************************************
 * Common_fn_GetAjaxResult
 * @param {string} url url.
 * @param {object} data data
 * @return {object} result object
 ***********************************************************/
function Common_fn_GetAjaxResult(url, data) {
    //a = typeof a !== 'undefined' ? a : 42;
    data = typeof data !== 'undefined' ? data : {};
    var vReturn = null;
    if (url !== "" && url !== null && url !== undefined) {
        vReturn =
            $.ajax(
                {
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: url,
                    data: data,
                    dataType: "json",
                    success: function (Result) {
                    },
                    error: function (r) {
                        return null;
                    }
                });
    }
    return vReturn;
}