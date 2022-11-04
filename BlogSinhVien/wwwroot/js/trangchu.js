$(function () {
    // Multiple images preview in browser
    var imagesPreview = function (input, placeToInsertImagePreview) {

        if (input.files) {
            var filesAmount = input.files.length;

            for (i = 0; i < filesAmount; i++) {
                var reader = new FileReader();

                reader.onload = function (event) {
                    $($.parseHTML('<img class ="images" style = "width: 32%; height: auto; margin-bottom:10px;box-shadow: 0 1px 4px 0 rgb(0 0 0 / 37%);">')).attr('src', event.target.result).appendTo(placeToInsertImagePreview);
                }

                reader.readAsDataURL(input.files[i]);
            }
        }

    };

    $('#fileImage').on('change', function () {
        $('.images').replaceWith('');
        document.getElementById('user-post-new').style.display = "block";
        imagesPreview(this, 'div.imagePreview');
    });
});
function close_imagepreview() {
    $('.images').replaceWith('');
    document.getElementById('user-post-new').style.display = "none";
}