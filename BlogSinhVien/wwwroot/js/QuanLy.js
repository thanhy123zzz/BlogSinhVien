function active(a) {
    tablinks = document.getElementsByClassName("menu-manager");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" activel", "");
    }
    document.getElementById(a).classList.add("activel");
}

