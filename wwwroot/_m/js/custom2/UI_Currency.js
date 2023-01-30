var UI_Currency = function () {

  
    var handleIndianCurrencySaperation = function () {
        $('.js-inr').each(function () {
            var item = $(this).text().replace(",", "");
            var num = Number(item).toLocaleString('en-IN');
            $(this).text(num);
        });
    }

    return {
        //main function to initiate the module
        init: function () {
            handleIndianCurrencySaperation();
        }
    };

}();


function ConvertToDefaultCurrency(str) {
    return Number(str.replace(",", "")).toLocaleString('en-IN');
}