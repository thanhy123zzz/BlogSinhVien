function active(a) {
    tablinks = document.getElementsByClassName("menu-manager");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" activel", "");
    }
    var tabcontent = document.getElementsByClassName("contain");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    document.getElementById("contain_" + a).style.display = "block";
    document.getElementById(a).classList.add("activel");
}
function searchManager() {
    var key = $('#search_manager').val();
    $.ajax({
        type: 'POST',
        url: '/managerment/search-manager',
        data: "key=" + key,
        success: function (result) {
            $("#contain_quanlybaiviet").replaceWith(result);
            active("quanlybaiviet");
        },
        error: function (result) {
            alert('Lỗi!');
        }
    })

}
function searchManagerEnter(event) {
    if (event.keyCode == 13) {
        var key = $('#search_manager').val();
        $.ajax({
            type: 'POST',
            url: '/managerment/search-manager',
            data: "key=" + key,
            success: function (result) {
                $("#contain_quanlybaiviet").replaceWith(result);
                active("quanlybaiviet");
            },
            error: function (result) {
                alert('Lỗi!');
            }
        })
    }
}
function randomAccount() {
    var emailEdu = document.getElementById("EmailEdu").value;
    $.ajax({
        type: 'POST',
        url: '/managerment/check-emailEdu',
        data: "emailEdu=" + emailEdu,
        success: function (result) {
            if (result) {
                document.getElementById("validation-emailEdu").style.display = "block";
                document.getElementById("btn-add").disabled = true;
            }
            else {
                document.getElementById("validation-emailEdu").style.display = "none";
                document.getElementById("btn-add").disabled = false;
                document.getElementById("taiKhoan").value = emailEdu.substring(0, 10);
                document.getElementById("MatKhau").value = Math.floor(Math.random() * (999999 - 100000)) + 100000;
            }
        },
        error: function (result) {
            alert('Lỗi!');
        }
    });
}

function checkMSSV() {
    var mssv = document.getElementById("MSSV").value;
    $.ajax({
        type: 'POST',
        url: '/managerment/check-mssv',
        data: "mssv=" + mssv,
        success: function (result) {
            if (result) {
                document.getElementById("validation-mssv").style.display = "block";
                document.getElementById("btn-add").disabled = true;
            }
            else {
                document.getElementById("validation-mssv").style.display = "none";
                document.getElementById("btn-add").disabled = false;
            }
        },
        error: function (result) {
            alert('Lỗi!');
        }
    });
}

function checkEmailEdu() {
    var emailEdu = document.getElementById("EmailEdu").value;
    
}