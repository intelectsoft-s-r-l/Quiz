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


