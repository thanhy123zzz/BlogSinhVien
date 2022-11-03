const inputs = document.querySelectorAll(".input");


function addcl() {
	let parent = this.parentNode.parentNode;
	parent.classList.add("focus");
}

function remcl() {
	let parent = this.parentNode.parentNode;
	if (this.value == "") {
		parent.classList.remove("focus");
	}
}


inputs.forEach(input => {
	input.addEventListener("focus", addcl);
	input.addEventListener("blur", remcl);
});

// Get the modal
var modal = document.getElementById("myModal");
var modal2 = document.getElementById("myModal2");
var modal3 = document.getElementById("myModal3");
// Get the button that opens the modal
var btn = document.getElementById("myBtn");

// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];
// When the user clicks the button, open the modal 
btn.onclick = function () {
	modal.style.display = "block";
}

// When the user clicks on <span> (x), close the modaládfa sfdadfsafSFADF
span.onclick = function () {
	modal.style.display = "none";
}
// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
	if (event.target == modal || event.target == modal2 || event.target == modal3) {
		modal.style.display = "none";
		modal2.style.display = "none";
		modal3.style.display = "none";
	}
}

var btn2 = document.getElementById("sendBtn");

function confirmCode(code) {
	var codeXN = $('#code').val();

	$.ajax({
		type: 'POST',
		url: '/confirm-code',
		data: "Code=" + codeXN,
		success: function (result) {
			if (result) {
				modal2.style.display = "none";
				modal3.style.display = "block";
				document.getElementById('errorCode').style.display = "none";
			}
			else {
				document.getElementById('errorCode').style.display = "block";
            }
		},
		error: function (result) {
			alert('Lỗi!');
		}
	})
}
function AnTBCode() {
	document.getElementById('errorCode').style.display = "none";
}

// gửi request về controller để gửi email
btn2.onclick = function () {
	var email2 = $('#emailForm').val();
	const isEmail = /^[0-9]{10}@sv.hcmunre.edu.vn$/i.test(email2);

	if (isEmail) {
		document.getElementById('loader').style.display = "block";
		document.getElementById('sendBtn').style.display = "none";
		document.getElementById('messEmail').style.display = "none";
		$.ajax({
			type: 'POST',
			url: '/send-mail',
			data: "Email=" + email2,
			success: function (result) {
				if (result) {
					alert('Đã gửi mã xác nhận vào email: ' + email2);
					modal.style.display = "none";
					modal2.style.display = "block";
					document.getElementById('errorEmail').style.display = "none";
				}
				else {
					document.getElementById('errorEmail').style.display = "block";
				}
				document.getElementById('loader').style.display = "none";
				document.getElementById('sendBtn').style.display = "block";
			},
			error: function () {
				alert('Lỗi!');
				document.getElementById('loader').style.display = "none";
				document.getElementById('sendBtn').style.display = "block";
			}
		})
	} else {
		//hiện thông báo
		document.getElementById('messEmail').style.display = "block";
    }
}
function AnThongBao() {
	document.getElementById('messEmail').style.display = "none";
	document.getElementById('loader').style.display = "none";
	document.getElementById('errorEmail').style.display = "none";
}

function DoiMatKhau() {
	var email2 = $('#emailForm').val();
	var pass = $('#pass').val();
	var passconfirm = $('#passconfirm').val();
	document.getElementById('loader2').style.display = "block";
	$.ajax({
			type: 'POST',
			url: '/change-pass',
			data: "Email=" + email2 + "&Pass=" + pass,
			success: function (result) {
				if (result) {
					alert("Đổi mật khẩu thành công")
					modal3.style.display = "none";
					document.getElementById("btnDoi").disabled = true;
					document.getElementById("btnDoi").style.cursor = "auto";
					document.getElementById('loader2').style.display = "none";
				}
			},
			error: function () {
				alert('Lỗi!');
			}
		})
}

function close_modal() {
	modal2.style.display = "none";
	modal3.style.display = "none";
}

$('#pass, #passconfirm').on('keyup', function () {
		if ($('#pass').val() == $('#passconfirm').val()) {
			document.getElementById("btnDoi").disabled = false;
			document.getElementById("btnDoi").style.cursor = "pointer";
			document.getElementById('errorPass').style.display = "none";
		}
		else {
			document.getElementById("btnDoi").disabled = true;
			document.getElementById("btnDoi").style.cursor = "auto";
			document.getElementById('errorPass').style.display = "block";
		}
});