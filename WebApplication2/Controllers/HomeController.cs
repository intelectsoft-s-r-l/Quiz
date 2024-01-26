using ISQuiz.Filter;
using ISQuiz.Interface;
using ISQuiz.Models.API.Questionnaires;
using ISQuiz.Models.Enum;
using ISQuiz.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ISQuiz.Controllers
{

    [Authorize]
    [Culture]
    public class HomeController : BaseController
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IUserRepository _userRepository;


        public HomeController(IQuizRepository quizRepository,
                             IUserRepository userRepository
                             )
        {
            _quizRepository = quizRepository;
            _userRepository = userRepository;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            string token = GetToken();

            var questionnaireData = await _quizRepository.GetQuestionnaires(token);
            return View("~/Views/Home/Index.cshtml", questionnaireData.questionnaires);
        }


        [HttpGet]
        public IActionResult Detail(int id) => View("~/Views/Home/Detail.cshtml", id);


        public async Task<DetailQuestionnaire> QuestionnaireDetail(int id)
        {

            string token = GetToken();
            var questionnaireData = await _quizRepository.GetQuestionnaire(token, id);
            return questionnaireData;


        }

        public async Task<DetailQuestions> QuestionsDetail(int id)
        {
            string token = GetToken();
            var questionsData = await _quizRepository.GetQuestions(token, id);
            return questionsData;


        }


        [HttpGet]
        public async Task<IActionResult> GetInfoQuestionnaire(int id)
        {

            var questionnaireData = await QuestionnaireDetail(id);
            var detailQuestionnaireVm = new QuestionnaireViewModel
            {
                oid = id,
                Title = questionnaireData.questionnaire.name,
                //Questions = questionsData.questions
            };
            //return View("~/Views/Home/Detail.cshtml", id);
            return PartialView("~/Views/Home/_Info.cshtml", detailQuestionnaireVm);

        }

        [HttpGet]
        public async Task<IActionResult> GetQuestions(int id)
        {
            var questionsData = await QuestionsDetail(id);

            return PartialView("~/Views/Home/_Questions.cshtml", questionsData);
        }


        [HttpGet]
        public async Task<IActionResult> QuestionnaireStatistic(int id)
        {


            string token = GetToken();

            var statistic = await _quizRepository.GetQuestionnaireStatistic(token, id);
            return PartialView("~/Views/Home/_Statistic.cshtml", statistic.questionnaireStatistic);

        }


        [HttpPost]
        public async Task<IActionResult> UpsertQuestionnaire([FromBody] UpsertQuestionnaire upsertQuestionnaireVM)
        {



            string token = GetToken();

            var userData = await _userRepository.getProfileInfo(token);
            upsertQuestionnaireVM.companyOid = userData.User.CompanyID;
            upsertQuestionnaireVM.company = userData.User.Company;
            upsertQuestionnaireVM.token = token;

            var questionnaireBaseResponsed = await _quizRepository.UpsertQuestionnaire(upsertQuestionnaireVM);
            return Json(new { StatusCode = 200 });


        }

        [HttpPost]
        public async Task<IActionResult> UpsertQuestion([FromBody] UpsertQuestionsViewModel upsertQuestionVM)
        {

            string token = GetToken();

            var deserializeResponseData = JsonConvert.DeserializeObject<ResponseObject>(upsertQuestionVM.responseVariant);

            UpsertQuestions upsertQuestions = new()
            {
                questions = new List<QuestionViewModel>
                {
                    new QuestionViewModel
                    {
                        id = upsertQuestionVM.id,
                        question = upsertQuestionVM.question,
                        questionnaireId = upsertQuestionVM.questionnaireId,
                        comentary = upsertQuestionVM.comentary,
                        gradingType = (GradingType)upsertQuestionVM.gradingType,
                        responseVariants = deserializeResponseData.ResponseVariants
                    }
                },
                token = token
            };



            var dataResponse = await _quizRepository.UpsertQuestions(upsertQuestions);
            return Json(new { StatusCode = 200 });


        }


        //[HttpGet]
        public IActionResult CreateQuestionnaire()
        {
            var upsertVm = new QuestionnaireViewModel
            {
                oid = 0,
                Questions = new List<QuestionViewModel>()
            };
            return View("~/Views/Home/Upsert.cshtml", upsertVm);

        }


        [HttpPost]
        public async Task<IActionResult> CreateQuestionnaire([FromBody] UpsertQuestionnareViewModel upsertQuestionnaireVM)
        {


            string token = GetToken();

            var userData = await _userRepository.getProfileInfo(token);


            UpsertQuestionnaire upsertQuestionnaire = new()
            {
                oid = upsertQuestionnaireVM.id,
                name = upsertQuestionnaireVM.Title,
                companyOid = userData.User.CompanyID,
                company = userData.User.Company,
                token = token
            };


            var questionnaireBaseResponsed = await _quizRepository.UpsertQuestionnaire(upsertQuestionnaire);

            var questionsVM = JsonConvert.DeserializeObject<QuestionsViewModel>(upsertQuestionnaireVM.Questions);

            questionsVM.questions.ForEach(item => item.questionnaireId = questionnaireBaseResponsed.questionnaireId);


            UpsertQuestions questions = new()
            {
                questions = questionsVM.questions,
                token = token
            };

            var questionsBaseResponsed = await _quizRepository.UpsertQuestions(questions);

            return Json(new { StatusCode = 200 });

        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteVm = new QuestionnaireViewModel();
            var questionnaireData = await QuestionnaireDetail(id);
            var questionsData = await QuestionsDetail(id);
            deleteVm.oid = id;
            deleteVm.Title = questionnaireData.questionnaire.name;
            deleteVm.Questions = questionsData.questions;
            return PartialView("~/Views/Home/Delete.cshtml", deleteVm);


        }

        //[HttpDelete, ActionName("Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteQuestionnaire([FromBody] int oid)
        {

            string token = GetToken();

            var baseResponse = await _quizRepository.DeleteQuestionnaire(token, oid);

            return Json(new { StatusCode = 200, Message = "Ok" });

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteQuestion([FromBody] int id)
        {
            string token = GetToken();
            var baseResponse = await _quizRepository.DeleteQuestion(token, id);
            return Json(new { StatusCode = 200, oid = id });


        }



        [Route("/404")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> NotFound() => PartialView("~/Views/_Shared/_NotFound.cshtml");


    }
}

