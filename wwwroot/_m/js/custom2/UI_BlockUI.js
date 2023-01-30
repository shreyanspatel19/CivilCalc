// BlockUI on Ajax Event --------------------------------------------------------------------------------------------
//Chage Style
function funBlockUI() {
    KTApp.blockPage({
        overlayColor: '#000000',
        type: 'v2',
        state: 'danger',
        message: '<b class="text-dark">Processing</b> . . .'
    });
}

function funUnblockUI() {
    KTApp.unblockPage();
    //setTimeout(function () {
    //    KTApp.unblockPage();
    //}, 100000);
}




