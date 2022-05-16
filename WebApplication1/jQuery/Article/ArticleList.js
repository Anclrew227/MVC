$(function () {
    $(document).delegate('#CreateBtn', 'click', function () {
        $('#CreateArticleModal form').validate({
            rules: {
                Title: {
                    required: true,
                    maxlength: 50
                },
                Content: 'required'
            },
            messages: {
                Title: {
                    required: '請輸入文章標題',
                    maxlength: '文章標題最多50字元'
                },
                Content: '請輸入文章內容'
            }
        });
        if (!$('#CreateArticleModal form').valid()) {
            return;
        }
        else {
            $('#CreateArticleModal form').submit();
        }
    });
})