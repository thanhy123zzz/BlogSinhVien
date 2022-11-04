"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var user1 = document.getElementById("userInput").value;
//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message, MaC, imageDataURL) {
    if (document.getElementById("MaC").value == MaC) {
        var li = document.createElement("li");
        if (user == user1) {
            li.className = "me";
            $(li).append("<figure></figure><p>" + message + "</p >");
        }
        else {
            li.className = "you";
            $(li).append("<figure><img src='" + imageDataURL +"'></figure><p>" + message + "</p >");
        }
        document.getElementById("messagesList").appendChild(li);
        var objDiv = document.getElementById("messagesList");
        objDiv.scrollTop = objDiv.scrollHeight;
    }
    $("#messageInput").val('');
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    var MaC = document.getElementById("MaC").value;
    connection.invoke("SendMessage", user1, message, MaC).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

// load mess box
function loadMesBox(MaC) {
    document.getElementById('MaC').value = MaC;
    $.ajax({
        type: 'POST',
        url: '/load-nameSV',
        data: "MaC=" + MaC,
        success: function (result) {
            $('#headerMessage').replaceWith(result);
        },
        error: function (result) {
            alert('Lỗi!');
        }
    })
	$.ajax({
		type: 'POST',
        url: '/load-messagebox',
        data: "MaC=" + MaC,
        success: function (result) {
            $('#messagesList').replaceWith(result);
            
		},
		error: function (result) {
			alert('Lỗi!');
		}
    })
}