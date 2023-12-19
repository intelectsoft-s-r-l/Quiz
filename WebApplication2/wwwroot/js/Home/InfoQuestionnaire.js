function DetailMore(index) {
    var detailId = "#detail_" + index;

    // Анимированно скрываем или отображаем детали
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


$(document).on("click", ".edit", function () {
    var container = $(this).closest(".row");
    var titleElement = container.find("h1");

    // Создаем input для редактирования
    var titleInput = $("<input type='text' class='form-control' id='defaultconfig' style='width: 30vw;' value='" + titleElement.text() + "' />");

    // Заменяем h1 на input
    titleElement.replaceWith(titleInput);

    // Заменяем кнопку Edit на Save
    var saveButton = $("<a class='btn btn-secondary  waves-effect waves-light save' title='Save'><i class='fas fa-save font-size-18'></i></a>").on("click", function () {
        var updatedTitle = titleInput.val();

        if (updatedTitle.trim() === "") {
            alert("Пожалуйста, введите непустое сообщение.");
            return;
        }

        var Quest = {
            oid: $('#mainId').val(),
            name: $('#defaultconfig').val(),
        };
        $.ajax({
            url: '/Home/UpsertQuestionnaire',
            cache: false,
            type: "POST",
            data: JSON.stringify(Quest),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.statusCode === 200) {
                    // Заменяем input на h3
                    titleInput.replaceWith("<h1 class='text-center'>" + updatedTitle + "</h1>");

                    // Заменяем кнопку Save на Edit
                    saveButton.replaceWith("<a class='btn btn-secondary  waves-effect waves-light edit' title='Edit'><i class='fas fa-pencil-alt font-size-18'></i></a>");
                } else {

                }
            },

        });

    });

    $(this).replaceWith(saveButton);
});