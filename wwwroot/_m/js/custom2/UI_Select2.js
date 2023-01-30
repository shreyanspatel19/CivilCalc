$.fn.modal.Constructor.prototype.enforceFocus = function () { };

var UI_Select2 = function () {

    var handleSelect2_kt = function () {
        var query = {};
        var $elements = $('.kt-select2');

        var isNonHtmlOptions = true;

        if ($elements.data("htmlOption") !== undefined && $elements.data("htmlOption") !== "") {
            isNonHtmlOptions = false;
        }

        $elements.select2({
            width: '100%',
            templateResult: function (item) {
                // No need to template the searching text
                if (item.loading) {
                    return item.text;
                }

                var term = query.term || '';
                var $result = markMatch2(item.text, term);

                return $result;
            },
            language: {
                    searching: function (params) {
                        // Intercept the query as it is happening
                        //query = params;
                        if (isNonHtmlOptions) {
                            query = params;  
                        }

                        // Change this to be appropriate for your application
                        return 'Searching…';
                    }
            }
            //language: {
            //    searching: function (params) {
            //        // Intercept the query as it is happening
            //        query = params;

            //        // Change this to be appropriate for your application
            //        return 'Searching…';
            //    }
            //}
        });


        function markMatch2(text, term) {
            // Find where the match is
            var match = text.toUpperCase().indexOf(term.toUpperCase());

            var $result = $('<span></span>');

            // If there is no match, move on
            if (match < 0) {
                return $result.text(text);
            }

            // Put in whatever text is before the match
            $result.text(text.substring(0, match));

            // Mark the match
            var $match = $('<span class="select2-rendered__match"></span>');
            $match.text(text.substring(match, match + term.length));

            // Append the matching text
            $result.append($match);

            // Put in whatever is after the match
            $result.append(text.substring(match + term.length));

            return $result;
        }
        
    }
    var handleSelect2 = function () {
        //console.log('handleSelect2 called');
        /********************************************************************************************
          STEP-01   Define Select2 Element
        *********************************************************************************************/
        var query = {};
        var $elements = $('.kt-select2');
        //var $elements = $('select').not('.no-select2').not('.dataTables_length select').not('.kt-select2-data-ajax').not('.kt-select2-data-ajax1').not('.kt-select2-data-ajax2');
        var $element1 = $('select');

        //$.each($elements,
        //    function(key, value) {
        //        $(value).select2('destroy');
        //    });
        //$elements.select2('destroy');


        /********************************************************************************************
          STEP-02   Check if Element is Multi-Select
        *********************************************************************************************/
        var vIsSelectMultiple = true;
        var attr = $elements.attr('multiple');
        if (typeof attr !== typeof undefined && attr !== false) {
            vIsSelectMultiple = false;
        }


        /********************************************************************************************
          STEP-03   Apply Bootstrap Theme
        *********************************************************************************************/
        $elements.select2.defaults.set("theme", "bootstrap");


        /********************************************************************************************
          STEP-04   Initialize Select2
        *********************************************************************************************/
        $elements.select2({
            selectOnClose: vIsSelectMultiple,
            width: '100%',
            templateResult: function (item) {
                // No need to template the searching text
                if (item.loading) {
                    return item.text;
                }

                var term = query.term || '';
                var $result = markMatch(item.text, term);

                return $result;
            },
            //templateSelection: function (selection) {
            //    var $result = $('<span>'+selection.text+'</span>');
            //    return $result;
            //},
            language: {
                searching: function (params) {
                    // Intercept the query as it is happening
                    query = params;

                    // Change this to be appropriate for your application
                    return 'Searching…';
                }
            }
        });


        function markMatch(text, term) {
            // Find where the match is
            var match = text.toUpperCase().indexOf(term.toUpperCase());

            var $result = $('<span></span>');

            // If there is no match, move on
            if (match < 0) {
                return $result.text(text);
            }

            // Put in whatever text is before the match
            $result.text(text.substring(0, match));

            // Mark the match
            var $match = $('<span class="select2-rendered__match"></span>');
            $match.text(text.substring(match, match + term.length));

            // Append the matching text
            $result.append($match);

            // Put in whatever is after the match
            $result.append(text.substring(match + term.length));

            return $result;
        }



        



        /********************************************************************************************
          STEP-05   select2:focus Event
        *********************************************************************************************/
        $elements.on('select2:focus',
            function (evt) {
                $(this).select2('open');
                //console.log('select2:focus called');
            });



        


        
    }

    return {
        //main function to initiate the module
        init: function () {
            handleSelect2_kt();
            //handleSelect2();
        }
    };

}();


