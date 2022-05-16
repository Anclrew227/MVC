$(function () {
    $('.UpdateMessage').click(function () {
        $('.sentMessage').off('click');
        $(this).closest('tr').prev('tr').prev('tr').children('td:eq(1)').html('<input class="editinput">');
        $(this).html('送出修改');
        $('.UpdateMessage').off('click');
        $(this).removeClass('UpdateMessage');
        $(this).addClass('sentMessage');
        $('.sentMessage').click(function () {
            var Content = $(this).closest('tr').prev('tr').prev('tr').children('td:eq(1)').children('input').val();
            var url = $(this).next('button').attr('onclick');
            url = url.replace('DeleteMessage', 'UpdateMessage');
            url = url.replace("'; return false;", '&Content=' + Content + "'; return false;");
            $(this).parents('td').append('<button class="ImFaker" style="display:none"></button>');
            $(this).next('button').next('button').attr('onclick', url);
            $('.ImFaker').click();
        })
    })
})