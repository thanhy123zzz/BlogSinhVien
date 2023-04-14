"use strict";

/*var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
//Disable send button until connection is established
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});*/

connection.on("ReceiveMessage", function (user, message, MaC, imageDataURL) {
    var user1 = $('#userInput').val();
    var sound = document.getElementById('soundMessage');
    if ($("#MaC").val() == MaC) {
        var li = document.createElement("li");
        if (user == user1) {
            li.className = "me";
            $(li).append("<figure></figure><p style='max-width: 70 %;overflow-wrap: break-word;'>" + message + "</p >");
        }
        else {
            li.className = "you";
            sound.play();
            $(li).append("<figure><img width='32px' style='max-height: 32px; max-width: 32px;' src='/images/avts/" + imageDataURL + "'></figure><p style='max - width: 70 %;overflow-wrap: break-word;'>" + message + "</p >");
        }
        document.getElementById("messagesList").appendChild(li);
        var objDiv = document.getElementById("messagesList");
        objDiv.scrollTop = objDiv.scrollHeight;
    }
    else {
        sound.play();
        $('#conversation_' + MaC).css("background-color", "aliceblue");
        if (!$('#iconC_' + MaC).length) {
            $('#conversation_' + MaC).find("span").after('<i id="iconC_' + MaC + '" class="ti-bell m-lg-auto"></i>');
        }
    }
    $('#conversation_' + MaC).prependTo('#listConversation');
    $("#messageInput").val(''); 
});



function loadMoreMessage() {
    var p = $('#messagesList').scrollTop();
    var slTN = $('#messagesList li').length;
    var MaC = $("#MaC").val();
    if (p == 0) {
        $.ajax({
            type: 'POST',
            url: '/load-more-message',
            data: "sl=" + slTN + "&MaC=" + MaC,
            success: function (result) {
                $('#messagesList').prepend(result);
            },
            error: function (result) {
                alert('Lỗi!');
            }
        })
    }
}

function sendMessage() {
    var user1 = $('#userInput').val();
    var message = $("#messageInput").val();
    var MaC = $("#MaC").val();
    connection.invoke("SendMessage", user1, message, MaC).catch(function (err) {
        return console.error(err.toString());
    });
}
function sendMessageEnter(event) {
    if (event.keyCode == 13) {
        var user1 = $('#userInput').val();
        var message = $("#messageInput").val();
        var MaC = $("#MaC").val();
        connection.invoke("SendMessage", user1, message, MaC).catch(function (err) {
            return console.error(err.toString());
        });
    }
}
// load mess box
function loadMesBox(MaC) {
    if (MaC != $('#MaC').val() || $('#MaC').val() == undefined) {
        $('#conversation_' + MaC).css("background-color", "");
        $("#iconC_" + MaC).remove();
        $.ajax({
            type: 'POST',
            url: '/load-messagebox',
            data: "MaC=" + MaC,
            success: function (result) {
                $('#chatBox').replaceWith(result);

            },
            error: function (result) {
                alert('Lỗi!');
            }
        });
    }
}
