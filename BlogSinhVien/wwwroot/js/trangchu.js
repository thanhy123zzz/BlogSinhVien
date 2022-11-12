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

    // Multiple images preview in browser
    var imagesPreview = function (input, placeToInsertImagePreview,maBD) {

        if (input.files) {
            var filesAmount = input.files.length;

            for (i = 0; i < filesAmount; i++) {
                var reader = new FileReader();

                reader.onload = function (event) {
                    $($.parseHTML('<img class="itemsComment_' + maBD + '" style="width:15%;box-shadow: 0 1px 4px 0 rgb(0 0 0 / 37%);margin-right:5px;margin-bottom:10px">')).attr('src', event.target.result).appendTo(placeToInsertImagePreview);
                }

                reader.readAsDataURL(input.files[i]);
            }
        }

};

function previewImageCmt(event, maBD) {
    console.log(maBD);
    document.getElementById("imageComment_" + maBD).style.display = "block";
    imagesPreview(event.target, "div.imageCommentPreview_" + maBD, maBD);
};


//close item conmment
function close_imageCommentpreview(maBD) {
    document.getElementById("imageComment_" + maBD).style.display = "none";
    $('.itemsComment_' + maBD).replaceWith('');
    $("#inputComment_" + maBD).val('');
}


function Comment(event, MaBD) {
    if (event.keyCode == 13) {
        var maSV = document.getElementById("userInput").value;
        var content = $("#contentcomments_" + MaBD).val();
        connection.invoke("CommentInsert", maSV, content, MaBD).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    }
}

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("DisplayComment", function (maSV, tenSV, content, MaBD, imageDataURL, date, MaCmt,MaUser) {
    var parent = jQuery("#showmore_" + MaBD).parent("li");
    var count_cmt = parseInt(document.getElementById("sl_cmt_" + MaBD).textContent);
    document.getElementById("sl_cmt_" + MaBD).textContent = count_cmt + 1;
    var comment_HTML = '<li style="display: flex"><div class="comet-avatar"><img style="width:50px; height:50px;" src="' + imageDataURL + '" alt=""></div><div class="we-comment" style="max-width:83.4%; width:83.4%;"><div class="coment-head"><h5><a href="/profile/' + maSV + '" title="' + tenSV + '">' + tenSV + '</a></h5><span>' + date + '</span><i class="fa fa-thumbs-up" style="cursor:pointer;" onclick="vote(' + MaBD +',' + MaCmt + ','+"'"+'' + MaUser + ''+"'"+')"></i></div><p>' + content + '</p></div><div style="max-width:6%; width:6%;display:inline-grid;justify-content:center;margin:auto;"><div class="vote-item"><i class="ti-angle-up"></i></div><div class="vote-item" style="margin-bottom:0;margin-left:2px"><h5 id="vote_' + MaCmt + '" class="vote-h">0</h5></div><div class="vote-item"><i class="ti-angle-down"></i></div></div> </li>';
    $(comment_HTML).insertBefore(parent);
    $("#contentcomments_" + MaBD).val('');
});

function load_More_Cmt(event,MaBD,loai) {
    var count_cmt = jQuery('#count_cmt_' + MaBD).val();
    $.ajax({
        type: 'POST',
        url: '/load-more-cmt',
        data: "MaBD=" + MaBD + "&sl=" + count_cmt + "&loai=" + loai,
        success: function (result) {
            $('#list_cmt_' + MaBD).replaceWith(result);
        },
        error: function (result) {
            alert('Lỗi!');
        }
    })
    event.preventDefault();
}

function load_More_BD() {
    var sl_bd = $('#sl_bd').val();
    console.log(sl_bd);
    $.ajax({
        type: 'POST',
        url: '/load-more-bd',
        data: "slbd=" + sl_bd,
        success: function (result) {
            $('#loadMore').replaceWith(result);
        },
        error: function (result) {
            alert('Lỗi!');
        }
    })
}

function vote(MaBD, MaCmt, MaUser) {
    connection.invoke("Vote", MaBD, MaCmt, MaUser).catch(function (err) {
        return console.error(err.toString());
    });
}

connection.on("IncreateVote", function (MaBD, MaCmt, MaUser, sl) {
    document.getElementById('vote_' + MaCmt).textContent = sl;
});

$(document).ready(function () {
    $('#table-sv').DataTable();
});