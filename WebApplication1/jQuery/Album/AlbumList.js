$(function () {
    $(document).delegate('#UploadModal #UploadBtn', 'click', function () {
        $('#UploadModal form').submit();
    });
    $(document).delegate('#AlbumListBlock .showImgLink', 'click', function (e) {
        $.ajax({
            url: $(this).attr('href'),
            success: function (data) {
                if (data.length > 0) {
                    $('#showImgBlock').removeClass('hidden');
                    $('#showImg').attr('src', data);
                }
                else {
                    $('#showImgBlock').addClass('hidden');
                    $('#showImg').attr('src', '');
                    alert('找無此圖片');
                }
            },
            error: function (jqXHR) {
                $('#showImgBlock').addClass('hidden');
                $('#showImg').attr('src', '');
                alert('找無此圖片');
            }
        });
        e.preventDefault();
        return false;
    });
});