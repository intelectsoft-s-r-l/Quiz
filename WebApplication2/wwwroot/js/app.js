function drawPatrialView(url, divId) {
    //debugger
    $.ajax({
        url: url,
        cache: false,
        type: "GET",
        dataType: "html",
        statusCode: {
            302: function (data) {
                window.location.href = '/Account/Logout/';
            },
        },
        success: function (result, e) {
            //debugger
            $('#' + divId).html(result);

        },
    });
}

//function postPatrialView(form) {
//    debugger
//    var $form = $("form[name='" + form +"']");
//    var url = $form.attr('action');
//    var test = $form.serialize();
//    $.ajax({
//        url: url,
//        cache: false,
//        type: "POST",
//        dataType: "html",
//        data: test,
//        contentType: "application/json; charset=utf-8",
//        statusCode: {
//            302: function (data) {
//                window.location.href = '/Account/Logout/';
//            },
//            200: function (data) {

//            },
//            500: function (data) {

//            }
//        },
//    });
//}


//function open(parameters) {
//    const id = parameters.data;
//    const url = parameters.url;

//    if (id == undefined || url == undefined) {
//        alert('Óïññ...')
//        return;
//    }

//    $.ajax(
//        {
//            type: 'POST',
//            url: url,
//            data: { "ConfirmPassword": id.ConfirmPassword, "NewPassword": id.NewPassword, "OldPassword": id.OldPassword },
//            success: function (response) {

//            },
//            failure: function () {
                
//            },
//            error: function (response) {
//                alert(response.responseText)
//            }
//        });
//};

