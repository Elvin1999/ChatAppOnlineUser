"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

connection.start().then(function () {
    console.log("CONNECTED");
}).catch(function (err) {
    return console.error(err.toString());
});

//Disable the send button until connection is established.


connection.on("ReceiveMessage", function (date, message) {

    var li = document.createElement("li");

    document.getElementById("userMessages").appendChild(li);

    let content = ` <li class="clearfix">
                                        <div class="message-data text-right">
                                            <span class="message-data-time">${date}</span>
                                        </div>
                                        <div class="message other-message float-right"> ${message} </div>
                                    </li>`;
    li.innerHTML = content;

});





connection.on("Connect", function (info) {
    var li = document.createElement("li");
    console.log("I got message");
    //document.getElementById("messagesList").appendChild(li);
    // li.innerHTML = `<span style='color:green;'>${info}</span>`;
});
connection.on("Disconnect", function (info) {

    var li = document.createElement("li");
    //document.getElementById("messagesList").appendChild(li);
    // li.innerHTML = `<span style='color:red;'>${info}</span>`;
    ////  location.reload();

});




function SendMessageUser(receiverId, senderId) {
    console.log("Message");
    console.log(receiverId + "   " + senderId);

    let message = document.getElementById("messageBox");

    connection.invoke("SendMessageUser", senderId, receiverId, message.value).catch(function (err) {
        return console.error(err.toString());
    })

    event.preventDefault();


}


document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = "";
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    })

    event.preventDefault();

});