/********************************************************************************************************
 *  DATE-PICKER, DATE-TIME-PICKER Initialization
 ********************************************************************************************************/
var UI_TinyMCE = function () {

    var handleTinyMCEFull = function () {


        $('textarea.kt-tinymce-full').each(function (key, value) {
            //console.log($(value).attr("id"));
            var elementID = $(value).attr("id");
            tinyMCE.execCommand('mceRemoveEditor', false, elementID);
        });
        tinyMCE.execCommand('mceRemoveEditor', false, 'textarea.kt-tinymce-full');

        var defaultConfig = {
            selector: 'textarea.kt-tinymce-full',
            height: 300,
            setup: function (editor) {
                editor.on('change',
                    function () {
                        tinymce.triggerSave();
                    });
            },
            onchange_callback: "myCustomOnChangeHandler",
            menubar: false,
            //plugins:
            //    'table advlist autolink link image lists charmap print preview code codesample hr charmap emoticons wordcount tiny_mce_wiris',
            //toolbar: [
            //    'undo redo cut copy paste pastetext | formatselect | bold italic underline strikethrough subscript superscript code | forecolor backcolor | removeformat ' +
            //    '| alignleft aligncenter alignright alignjustify | outdent indent ' +
            //    '| bullist numlist blockquote  advlist | image link table code codesample tiny_mce_wiris_formulaEditor tiny_mce_wiris_formulaEditorChemistry hr charmap emoticons ' +
            //    '| print preview code wordcount'
            //],
            plugins:
                'table advlist autolink link lists charmap preview code hr wordcount',
            toolbar: [
                'undo redo cut copy paste pastetext | formatselect | bold italic underline strikethrough subscript superscript | forecolor backcolor | removeformat ' +
                '| alignleft aligncenter alignright alignjustify | outdent indent ' +
                '| bullist numlist | table ' +
                '| preview code'
            ],
            //content_css: "/mycontent.css",
            block_formats: 'Paragraph=p;Header 1=h1;Header 2=h2;Header 3=h3',
            style_formats:
                [
                    {
                        title: 'Headers',
                        items: [
                            {
                                title: 'Header 1',
                                format: 'h3'
                            },
                            {
                                title: 'Header 2',
                                format: 'h4'
                            }
                        ]
                    },
                    {
                        title: 'Inline',
                        items: [
                            {
                                title: 'Bold',
                                icon: 'bold',
                                format: 'bold'
                            },
                            {
                                title: 'Italic',
                                icon: 'italic',
                                format: 'italic'
                            },
                            {
                                title: 'Underline',
                                icon: 'underline',
                                format: 'underline'
                            },
                            {
                                title: 'Strikethrough',
                                icon: 'strikethrough',
                                format: 'strikethrough'
                            },
                            {
                                title: 'Superscript',
                                icon: 'superscript',
                                format: 'superscript'
                            },
                            {
                                title: 'Subscript',
                                icon: 'subscript',
                                format: 'subscript'
                            },
                            {
                                title: 'Code',
                                icon: 'code',
                                format: 'code'
                            }
                        ]
                    },
                    {
                        title: 'Blocks',
                        items: [
                            {
                                title: 'Paragraph',
                                format: 'p'
                            },
                            {
                                title: 'Blockquote',
                                format: 'blockquote'
                            },
                            {
                                title: 'Div',
                                format: 'div'
                            },
                            {
                                title: 'Pre',
                                format: 'pre'
                            }
                        ]
                    },
                    {
                        title: 'Alignment',
                        items: [
                            {
                                title: 'Left',
                                icon: 'alignleft',
                                format: 'alignleft'
                            },
                            {
                                title: 'Center',
                                icon: 'aligncenter',
                                format: 'aligncenter'
                            },
                            {
                                title: 'Right',
                                icon: 'alignright',
                                format: 'alignright'
                            },
                            {
                                title: 'Justify',
                                icon: 'alignjustify',
                                format: 'alignjustify'
                            }
                        ]
                    }
                ]
        };

        setTimeout(function () {
            tinymce.init(defaultConfig);
        }, 100);
    };



    var handleTinyMCEBasic = function () {
        $('textarea.kt-tinymce-basic').each(function (key, value) {
            //console.log($(value).attr("id"));
            var elementID = $(value).attr("id");
            tinyMCE.execCommand('mceRemoveEditor', false, elementID);
        });
        tinyMCE.execCommand('mceRemoveEditor', false, 'textarea.kt-tinymce-basic');

        var defaultConfig = {
            selector: 'textarea.kt-tinymce-basic',
            height: 150,
            setup: function (editor) {
                editor.on('change',
                    function () {
                        tinymce.triggerSave();
                    });
            },
            menubar: false,
            plugins:
                'table advlist autolink link image lists charmap print preview code codesample hr charmap emoticons wordcount tiny_mce_wiris',
            toolbar: [
                'bold italic underline strikethrough subscript superscript | forecolor backcolor | removeformat ' +
                '| bullist numlist | hr charmap emoticons ' + '| preview code'
            ],
            block_formats: 'Paragraph=p;Header 1=h1;Header 2=h2;Header 3=h3',
        };

        setTimeout(function () {
            tinymce.init(defaultConfig);
        }, 100);
    };


    return {
        //main function to initiate the module
        init: function () {
            handleTinyMCEFull();
            handleTinyMCEBasic();
        }
    };

}();









