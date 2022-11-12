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
            $("#loadMore").replaceWith(result);
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
                $("#loadMore").replaceWith(result);
            },
            error: function (result) {
                alert('Lỗi!');
            }
        })
    }
}
