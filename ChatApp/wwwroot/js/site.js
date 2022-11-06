// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



function SendRequest(id) {
    alert("Send Request");
    $.ajax({
        url: "/Home/SendFollow/" + id,
        method: "GET",
        success: function (data) {
            alert("Send Request success");
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

function GetUsers() {
    setInterval(function () {

        $.ajax({
            url: "/Home/GetAllActiveUsers",
            method: "GET",
            success: function (data) {
                let content = "";
                for (var i = 0; i < data.length; i++) {
                    let item = `<div style='display:flex;' >
                        <img  class='card-img-top' style='width:100px;height:100px;border-radius:50%;border:2px solid green;' src='/images/${data[i].imageUrl}' />

                            <div class='card-body'>
                                <h5 class='card-title'  >${data[i].userName}</h5>
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


        //        $.ajax({
        //            url: "/Home/GetAllUsers",
        //            method: "GET",
        //            success: function (data) {
        //                let content = "";
        //                for (var i = 0; i < data.length; i++) {
        //                    let disconnectDate=new Date(data[i].disConnectTime);
        //                    console.log(data[i].disConnectTime);
        //                    console.log(disconnectDate);
        //                    let style = "";
        //                    let dateContent = "";
        //                    if (data[i].isOnline) {
        //                        style = 'border:5px solid green;';
        //                    }
        //                    else {
        //                        style = 'border:5px solid red;';
        //                        let currentDate = new Date();
        //                        let diffTime = Math.abs(currentDate - disconnectDate);
        //                        let diffMinutes = Math.ceil(diffTime / (1000 * 60));

        //                        dateContent = `<span class='btn btn-warning'data:image/pjpeg;base64,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>left ${diffMinutes} minutes ago</span>`;
        //                    }

        //                    let item = `<div class='card' style='width:14rem;'>
        //                    <img style='${style}'  class='card-img-top' src='/images/${data[i].imageUrl}' alt='Card image cap' />
        //            <div class='card-body'>
        //                <h5 class='card-title'>${data[i].userName}</h5>
        //                <p class='card-text'  >${data[i].email}</p>
        //                <a href='/home/FindCurrentUser/${data[i].id}' class='btn btn-outline-primary' >Go Chat</a>
        //                <p class='card-text m-2'  >${dateContent}</p>
        //                </div>
        //</div>`;
        //                    content += item;
        //                }
        //                $("#allUsers").html(content);
        //            }, error: function (err) {
        //                console.log(err);
        //            }
        //        });


        $.ajax({
            url: "/Home/GetAllUsers",
            method: "GET",
            success: function (data) {
                let content = "";
                for (var i = 0; i < data.length; i++) {
                    let disconnectDate = new Date(data[i].disConnectTime);
                    console.log(data[i].disConnectTime);
                    console.log(disconnectDate);
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

                        dateContent = `<span class='btn btn-warning'data:image/pjpeg;base64,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>left ${diffMinutes} minutes ago</span>`;
                    }

                    let item = `<div class='card' style='width:14rem;margin:5px;'>
                    <img style='${style}width:220px;height:250px;'  class='card-img-top' src='/images/${data[i].imageUrl}' alt='Card image cap' />
            <div class='card-body'>
                <h5 class='card-title'>${data[i].userName}</h5>
                <p class='card-text'  >${data[i].email}</p>
                <button onclick="SendRequest('${data[i].id}')" class='btn btn-outline-primary' >Follow</button>
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

    }, 1000);
    setInterval(function () {
        GetRequests();
    }, 4000);
}

function GetRequests() {
    $.ajax({
        url: "/Home/GetAllRequests",
        method: "GET",
        success: function (data) {
            console.log(data);
            let content = "";
            for (var i = 0; i < data.length; i++) {
                let item = `<div class="card" style="width: 15rem;">
  <div class="card-body">
    <h5 class="card-title">Friend Request</h5>
  </div>
  <ul class="list-group list-group-flush">
    <li class="list-group-item">${data[i].content}</li>
  </ul>
  <div class="card-body">
    <a href="#" class="btn btn-primary">Accept</a>
    <a href="#" class="btn btn-primary">Decline</a>
  </div>
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