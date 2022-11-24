// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



function SendRequest(id) {
    $.ajax({
        url: "/Home/SendFollow/" + id,
        method: "GET",
        success: function (data) {
            let content = "";
            let item = `<div class="alert alert-success" role="alert">
  Your friend request sent successfully
</div>`;
            content += item;
            $("#request").html(content);
        },
        error: function (err) {
            console.log(err);
        }


    });
}



function DeclineRequest(id, requestId) {
    $.ajax({
        url: "/Home/DeclineRequest?idSender=" + id + "&requestId=" + requestId,
        method: "GET",
        success: function (data) {
            console.log(id);
            let content = "";
            let item = `<div class="alert alert-warning" role="alert">
  You declined request successfully
</div>`;
            content += item;
            $("#request").html(content);
        },
        error: function (err) {
            console.log(err);
        }


    });
}


function AcceptRequest(id, requestId) {
    $.ajax({
        url: "/Home/AcceptRequest?id=" + id + "&requestId=" + requestId,
        method: "GET",
        success: function (data) {
            console.log(id);
            let content = "";
            let item = `<div class="alert alert-warning" role="alert">
  You accept request successfully
</div>`;
            content += item;
            $("#request").html(content);
        },
        error: function (err) {
            console.log(err);
        }


    });
}



function DeleteRequest(id) {
    $.ajax({
        url: "/Home/DeleteRequest?id=" + id,
        method: "DELETE",
        success: function (data) {
            console.log(id);
            let content = "";
            let item = `<div class="alert alert-warning" role="alert">
  You delete request successfully
</div>`;
            content += item;
            $("#request").html(content);
        },
        error: function (err) {
            console.log(err);
        }


    });
}

function UnFollow(id) {
    $.ajax({
        url: "/Home/UnFollow?id=" + id,
        method: "GET",
        success: function (data) {
            console.log(id);
            let content = "";
            let item = `<div class="alert alert-warning" role="alert">
  You unfollow friend successfully
</div>`;
            content += item;
            $("#request").html(content);
        },
        error: function (err) {
            console.log(err);
        }


    });
}



function GetMyFriends() {
    $.ajax({
        url: "/Home/GetMyFriends",
        method: "GET",
        success: function (data) {
            let content = "";
            for (var k = 0; k < data.length; k++) {
                let subContent = "";
                if (data[k].lastMessageCount > 0) {
                    subContent = `<span style='background-color:#00bfff;' class="badge">${data[k].lastMessageCount}</span></h6>`;
                }
                content += `<div class='card' style='width:17rem;margin:5px;'>
                <div style='display:flex;flex-direction:row;'>
                    <img style='width:80px;height:80px;'  class='card-img-top' src='/images/${data[k].yourFriend.imageUrl}' alt='Card image cap' />

                <h6 class='card-title text-success' style='margin-top:30px;margin-left:3px;'>${data[k].yourFriend.userName}

         ${subContent}
                <h4 class='card-title text-info' style='margin-top:30px;margin-left:3px;'></h6>
</div>
            <div class='card-body'>
                <button onclick="UnFollow('${data[k].id}')" class='btn btn-outline-info'  >UnFollow</button>
                <a class='btn btn-outline-info' href="/Home/GoChat/${data[k].yourFriendId}" >Send Message</a>
                </div>
</div>`;
            }
            console.log(content);
            $("#friends").html(content);
        },
        error: function (err) {
            console.log(err);
        }


    });
}


function GetUsers() {
    setInterval(function () {

        $.ajax({
            url: "/Home/GetAllActiveUsers",
            method: "GET",
            success: function (data) {
                let content = "";
                for (var i = 0; i < data.length; i++) {
                    let item = `<div style='display:flex;' >
                        <img  class='card-img-top' style='width:60px;height:60px;border-radius:50%;border:2px solid green;' src='/images/${data[i].imageUrl}' />

                            <div class='card-body m-2'>
                                <h5 class='card-title'  >${data[i].userName}</h5>
                                <span class='btn btn-info'>Connect ${data[i].connectTime}</span>
                                </div>

                                </div>`;
                    content += item;
                }
                $("#activeUsers").html(content);
            },
            error: function (err) {
                console.log(err);
            }


        });



        $.ajax({
            url: "/Home/GetAllUsers",
            method: "GET",
            success: function (data) {
                let content = "";
                for (var i = 0; i < data.length; i++) {
                    let disconnectDate = new Date(data[i].disConnectTime);
                    let subContent = ``;
                    if (data[i].hasRequestPending) {
                        subContent = ` <button class='btn btn-outline-primary' >Already Sent</button>`;
                    }
                    else {
                        subContent = ` <button onclick="SendRequest('${data[i].id}')" class='btn btn-outline-primary' >Follow</button>`;
                    }

                    let style = "";
                    let dateContent = "";
                    if (data[i].isOnline) {
                        style = 'border:5px solid green;';
                    }
                    else {
                        style = 'border:5px solid red;';
                        let currentDate = new Date();
                        let diffTime = Math.abs(currentDate - disconnectDate);
                        let diffMinutes = Math.ceil(diffTime / (1000 * 60));
                        if (diffMinutes >= 60) {
                            diffMinutes = Math.ceil(diffMinutes / 60);
                            dateContent = `<span class='btn btn-warning'data:image/pjpeg;base64,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>left ${diffMinutes} hours ago</span>`;
                        }
                        else {

                            dateContent = `<span class='btn btn-warning'data:image/pjpeg;base64,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>left ${diffMinutes} minutes ago</span>`;
                        }
                    }

                    let item = `<div class='card' style='width:14rem;margin:5px;'>
                    <img style='${style}width:220px;height:250px;'  class='card-img-top' src='/images/${data[i].imageUrl}' alt='Card image cap' />
            <div class='card-body'>
                <h5 class='card-title'>${data[i].userName}</h5>
                <p class='card-text'  >${data[i].email}</p>
               ${subContent}
                <p class='card-text mt-2'  >${dateContent}</p>
                 <div id='request' class='card-text' ></div>
                </div>
</div>`;
                    content += item;


                }
                $("#allUsers").html(content);
            }, error: function (err) {
                console.log(err);
            }
        });
        GetRequests();
        GetMyFriends();
    }, 1000);

}

function GetRequests() {
    $.ajax({
        url: "/Home/GetAllRequests",
        method: "GET",
        success: function (data) {
            let content = "";
            let subContent = "";
            for (var i = 0; i < data.length; i++) {
                if (data[i].status == "Request") {
                    subContent = ` <div class="card-body">
      <button onclick="AcceptRequest('${data[i].senderId}','${data[i].id}')" class='btn btn-success' >Accept</button>
      <button onclick="DeclineRequest('${data[i].senderId}','${data[i].id}')" class='btn btn-outline-secondary' >Decline</button>
  </div>`;
                }
                else {

                    subContent = ` <div class="card-body">
      <button onclick="DeleteRequest('${data[i].id}')" class='btn btn-warning' >Delete</button>
  </div>`;
                }

                let item = `<div class="card" style="width: 15rem;">
  <div class="card-body">
    <h5 class="card-title">Friend Request </h5>
  </div>
  <ul class="list-group list-group-flush"> 
    <li class="list-group-item">${data[i].content}</li>
  </ul>
  ${subContent}

</div>`;
                content += item;
            }


            $("#requests").html(content);
        },
        error: function (err) {
            console.log(err);
        }


    });
}
GetUsers();
GetRequests();