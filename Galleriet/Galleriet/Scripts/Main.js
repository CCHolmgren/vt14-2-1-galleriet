/*$(function () {
    "use strict";

    console.log(location);
    var anchor = $('#imageswitcher a');

    anchor.each(function (index) {
        console.log($(this));
        var href = $(this).attr('href');
        if (href === decodeURIComponent(location.search).split('&').filter(function (s) { return s.indexOf('file=') !== -1 }).sort()[0])
            $(this).parent.classList.add('current');
    });
})*/
$(function () {
    'use strict';

    $('#successbutton').click(function () {
        $(this).parent().remove();
    });

    var anchor = $('#imageswitcher a');
    var images = 0;
    anchor.each(function (index) {
        var href = $(this).attr('href');
        images += 1;
        //console.log($(this));

        var list = decodeURIComponent(location.search).split('&').filter(function (s) { return s.indexOf('file=') !== -1 }).sort()[0];
        if (href === list) {
            $(this).addClass('current');
            console.log(".liimageswitcher.scrollLeft ",$('.liimageswitcher').scrollLeft(images * 45));
        }
    });
});