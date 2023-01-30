//Current Menu Item Highlighter
$(document).ready(function () {
    var pathname = window.location.pathname; // Returns path only
    //var url = window.location.href;     // Returns full URL

    console.log(pathname);
    $("a[href='" + pathname + "']").first().parents("li").addClass("kt-menu__item--active");
    $("a[href='" + pathname + "']").parents("li.kt-menu__item--submenu").addClass("kt-menu__item--open kt-menu__item--here");
    
});

//$(document).ready(function () {
//    var pathname = window.location.pathname; // Returns path only
//    //var url = window.location.href;     // Returns full URL

//    //$(".nav-item.start.active.open>.nav-link.nav-toggle").first().parents("li").addClass("active open");
//    $(".nav-item.start.active.open>.nav-link.nav-toggle").children('span').addClass('open');
//});