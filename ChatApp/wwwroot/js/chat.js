"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (currentUser, message) {

    var li = document.createElement("li");

    document.getElementById("messagesList").appendChild(li);

    let content = `<img src='/images/${currentUser.imageUrl}'  style='border-radius:50%;width:100px;height:100px;'`;
    content += `${currentUser.userName} says ${message}`;

    li.innerHTML = content;

});

connection.on("Connect", function (info) {

    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
   // li.innerHTML = `<span style='color:green;'>${info}</span>`;
});
connection.on("Disconnect", function (info) {

    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
   // li.innerHTML = `<span style='color:red;'>${info}</span>`;
  ////  location.reload();

});


connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});



document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = "";
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    })

    event.preventDefault();

});