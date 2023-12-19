function drawPatrialView(url, divId) {
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
            $('#' + divId).html(result);

        },
    });
}

function drawPatrialViewWithLoad(url, divId) {
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
        beforeSend: function () {
            $('#' + divId).html('<div id="spinner-content" class="d-flex justify-content-center">' +
                '<div class="" name="Growing-Spinners">' +
                '<div class="spinner-grow text-primary" role = "status" style="height:3rem;width:3rem;" >' +
                '<span class="sr-only">Loading...</span>' +
                '</div >' +
                '<div class="spinner-grow text-secondary" role="status" style="height:3rem;width:3rem;">' +
                ' <span class="sr-only">Loading...</span>' +
                '</div>' +
                '<div class="spinner-grow text-success" role="status" style="height:3rem;width:3rem;">' +
                '<span class="sr-only">Loading...</span>' +
                '</div>' +
                '<div class="spinner-grow text-danger" role="status"style="height:3rem;width:3rem;">' +
                '<span class="sr-only">Loading...</span>' +
                '</div>' +
                '<div class="spinner-grow text-warning" role="status" style="height:3rem;width:3rem;">' +
                '<span class="sr-only">Loading...</span>' +
                '</div>' +
                '<div class="spinner-grow text-info" role="status" style="height:3rem;width:3rem;">' +
                '<span class="sr-only">Loading...</span>' +
                '</div>' +
                '</div >' +
                '</div >');

        },
        success: function (result, e) {
            $('#' + divId).html(result);

        },
    });
}
