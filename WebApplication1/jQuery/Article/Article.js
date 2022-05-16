$(function () {
    $(document).delegate('#UpdateBtn', 'click', function () {
        $('#UpdateArticleModal form').validate({
            rules: {
                Content: 'required'
            },
            messages: {
                Content: '請輸入文章內容'
            }
        });
        if (!$('#UpdateArticleModal form').valid()) {
            return;
        }
        else {
            $('#UpdateArticleModal form').submit();
        }
    });

    $('#createmessage').click(function () {
        setTimeout(function () {
            location.reload()
        }, 100)
    });
});