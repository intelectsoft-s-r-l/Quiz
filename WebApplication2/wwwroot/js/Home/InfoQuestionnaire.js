
function DetailMore(index) {
    var detailId = "#detail_" + index;

    // јнимированно скрываем или отображаем детали
    $(detailId).animate({
        height: 'toggle'
    });

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

