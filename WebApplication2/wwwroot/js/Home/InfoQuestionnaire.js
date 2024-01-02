
function DetailMore(index) {
    var detailId = "#detail_" + index;

    // јнимированно скрываем или отображаем детали
    $(detailId).animate({
        height: 'toggle'
    });

}

function toggleEye(icon) {
    if (icon.getAttribute('data-status') === 'open') {
        icon.classList.remove('uil-eye');
        icon.classList.add('uil-eye-slash', 'uil-eye-closed');
        icon.setAttribute('data-status', 'closed');
    } else {
        icon.classList.remove('uil-eye-slash', 'uil-eye-closed');
        icon.classList.add('uil-eye');
        icon.setAttribute('data-status', 'open');
    }
}

function DeleteQuestion(index, oid) {

    var container = $("#questionN_" + index);

    $.ajax({
        url: '/Home/DeleteQuestion',
        cache: false,
        type: "DELETE",
        data: JSON.stringify(container.find('#QuestionId').val()),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            document.getElementById('editBtn').style.display = 'block';

            var cancelBtn = document.getElementById('cancelBtn');
            cancelBtn.classList.remove('col-lg-12');
            cancelBtn.classList.add('col-lg-3');
            drawPatrialView("/Home/GetQuestions/" + oid, "questions");
        },

    });


}

