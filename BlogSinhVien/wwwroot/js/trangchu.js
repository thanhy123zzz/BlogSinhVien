$(function () {
    // Multiple images preview in browser
    var imagesPreview = function (input, placeToInsertImagePreview) {

        if (input.files) {
            var filesAmount = input.files.length;
            for (i = 0; i < filesAmount; i++) {
                console.log(input.files[i].type);
                var type = input.files[i].type;
                type = type.substr(0, type.indexOf("/"));
                if (type == "image") {
                    var reader = new FileReader();
                    reader.onload = function (event) {
                        $($.parseHTML('<img class ="items-att" style = "width: 32%; height: auto; margin-bottom:10px;box-shadow: 0 1px 4px 0 rgb(0 0 0 / 37%);">')).attr('src', event.target.result).appendTo(placeToInsertImagePreview);
                    }
                    reader.readAsDataURL(input.files[i]);
                }

            }
        }

    };

    $('#fileImage').on('change', function () {
/*        $('.items-att').replaceWith('');*/
        document.getElementById('user-post-new').style.display = "block";
/*        document.getElementById('fileVideo').disabled = true;
        document.getElementById('fileDocs').disabled = true;*/
        imagesPreview(this, 'div.imagePreview');
    });

});
function close_imagepreview() {
    $('.items-att').replaceWith('');
    document.getElementById('user-post-new').style.display = "none";
/*    document.getElementById('fileVideo').disabled = false;
    document.getElementById('fileDocs').disabled = false;
    document.getElementById('fileImage').disabled = false;*/
}

$("#fileVideo").on("change", function (event) {
/*    $('.items-att').replaceWith('');*/
    document.getElementById('user-post-new').style.display = "block";
/*    document.getElementById('fileImage').disabled = true;
    document.getElementById('fileDocs').disabled = true;*/

    var numberOfVideos = event.target.files.length;
    for (var i = 0; i < numberOfVideos; i++) {
        var file = event.target.files[i];

        var type = file.type;
        type = type.substr(0, type.indexOf("/"));

        if (type == "video") {
            var blobURL = URL.createObjectURL(file);
            var video = document.createElement('video');
            video.src = blobURL;
            video.setAttribute("controls", "");
            video.style.width = "32%";
            video.className = "items-att";
            video.style.height = "auto";
            video.style.marginBottom = "10px";
            video.style.boxShadow = "0 1px 4px 0 rgb(0 0 0 / 37%)";
            var videos = document.getElementById("imagePreview");
            videos.appendChild(video);
        }
    }
});

$("#fileDocs").on("change", function (event) {
    document.getElementById('user-post-new').style.display = "block";

    var numberOfVideos = event.target.files.length;
    for (var i = 0; i < numberOfVideos; i++) {
        var file = event.target.files[i];

        var type = file.type;
        type = type.substr(0, type.indexOf("/"));

        if (type == "application") {
            var docs = document.createElement('button');
            var icon = document.createElement('i');

            if (file.name.match(".docx")) { icon.className = "fa fa-file-word-o"; file.type = ".docx" }
            if (file.name.match(".pdf")) { icon.className = "fa fa-file-pdf-o"; file.type = ".pdf" }
            if (file.name.match(".xlsx")) { icon.className = "fa fa-file-excel-o"; file.type = ".xlsx" }
            if (file.name.match(".pptx")) { icon.className = "fa fa-file-powerpoint-o"; file.type = ".pptx" }

            docs.className = "items-att btn btn-light";
            docs.disabled = true;
            docs.style.width = "32%";
            docs.style.whiteSpace = "normal";

            docs.textContent = file.name + " ";
            docs.appendChild(icon);

            var videos = document.getElementById("imagePreview");
            videos.appendChild(docs);
        }
    }
});

// add item conmment




/** Post a Comment **/
jQuery(".post-comt-box textarea").on("keydown", function (event) {

    if (event.keyCode == 13) {
        var comment = jQuery(this).val();
        var parent = jQuery(".showmore").parent("li");
        var comment_HTML = '	<li><div class="comet-avatar"><img src="images/resources/comet-1.jpg" alt=""></div><div class="we-comment"><div class="coment-head"><h5><a href="time-line.html" title="">Jason borne</a></h5><span>1 year ago</span><a class="we-reply" href="#" title="Reply"><i class="fa fa-reply"></i></a></div><p>' + comment + '</p></div></li>';
        $(comment_HTML).insertBefore(parent);
        jQuery(this).val('');
    }
});