using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication2.Filter;
using WebApplication2.Interface;
using WebApplication2.Models.API.Questionnaires;
using WebApplication2.Models.Enum;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{

    [Authorize]
    [Culture]
    public class HomeController : BaseController
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IUserRepository _userRepository;
        //private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IQuizRepository quizRepository, IUserRepository userRepository/*, IStringLocalizer<HomeController> localizer*/)
        {
            _quizRepository = quizRepository;
            _userRepository = userRepository;
            //_localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string token = GetToken();
            var questionnaireData = await _quizRepository.GetQuestionnaires(token);
            if (questionnaireData.errorCode == 143)
            {
                await RefreshToken();
                return await Index();
            }
            else if (questionnaireData.errorCode == 118)
            {
                return View("~/Views/Account/Login.cshtml");
            }
            else if (questionnaireData.errorCode == 0)
            {

                return View("~/Views/Home/Index.cshtml", questionnaireData.questionnaires);
            }
            else
                return View("Error");

        }

        [HttpGet]
        public IActionResult Detail(int id) => View("~/Views/Home/Detail.cshtml", id);


        public async Task<DetailQuestionnaire> QuestionnaireDetail(int id)
        {
            string token = GetToken();
            var questionnaireData = await _quizRepository.GetQuestionnaire(token, id);
            if (questionnaireData.errorCode == 143)
            {
                await RefreshToken();
                return await QuestionnaireDetail(id);
            }
            else if (questionnaireData.errorCode == 0)
                return questionnaireData;
            else
                return new DetailQuestionnaire { errorCode = -1 };

        }

        public async Task<DetailQuestions> QuestionsDetail(int id)
        {
            string token = GetToken();
            var questionsData = await _quizRepository.GetQuestions(token, id);
            if (questionsData.errorCode == 143)
            {
                await RefreshToken();
                return await QuestionsDetail(id);
            }
            else if (questionsData.errorCode == 0)
                return questionsData;
            else
                return new DetailQuestions { errorCode = -1 };
        }


        [HttpGet]
        public async Task<IActionResult> GetInfoQuestionnaire(int id)
        {

            var questionnaireData = await QuestionnaireDetail(id);
            var questionsData = await QuestionsDetail(id);

            if (questionnaireData.errorCode == 0 && questionsData.errorCode == 0)
            {
                var detailQuestionnaireVm = new QuestionnaireViewModel();
                detailQuestionnaireVm.oid = id;
                detailQuestionnaireVm.Title = questionnaireData.questionnaire.name;
                detailQuestionnaireVm.Questions = questionsData.questions;
                //return View("~/Views/Home/Detail.cshtml", id);
                return PartialView("~/Views/Home/_Info.cshtml", detailQuestionnaireVm);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestions(int id)
        {

            var questionsData = await QuestionsDetail(id);

            if (questionsData.errorCode == 0)
            {
                return PartialView("~/Views/Home/_Questions.cshtml", questionsData);
            }
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> UpsertQuestionnaire([FromBody] UpsertQuestionnaire upsertQuestionnaireVM)
        {
            string token = GetToken();

            var userData = await _userRepository.getProfileInfo(token);

            if (userData.ErrorCode == 143)
            {
                await RefreshToken();
                return await UpsertQuestionnaire(upsertQuestionnaireVM);
            }
            else if (userData.ErrorCode == 118)
            {
                return View("~/Views/Account/Login.cshtml");
            }
            else if (userData.ErrorCode == 0)
            {
                upsertQuestionnaireVM.companyOid = userData.User.CompanyID;
                upsertQuestionnaireVM.company = userData.User.Company;
                upsertQuestionnaireVM.token = token;

                var questionnaireBaseResponsed = await _quizRepository.UpsertQuestionnaire(upsertQuestionnaireVM);

                if (questionnaireBaseResponsed.errorCode == 143)
                {
                    await RefreshToken();
                    return await UpsertQuestionnaire(upsertQuestionnaireVM); ///![Get data FromBody]
                }
                else if (questionnaireBaseResponsed.errorCode == 0)
                {
                    return Json(new { StatusCode = 200 });
                    //return RedirectToAction("Index");
                }
            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> UpsertQuestion([FromBody] UpsertQuestionsViewModel upsertQuestionVM)
        {
            string token = GetToken();

            var deserializeResponseData = JsonConvert.DeserializeObject<ResponseObject>(upsertQuestionVM.responseVariant);

            UpsertQuestions upsertQuestions = new UpsertQuestions();
            upsertQuestions.questions = new List<QuestionViewModel>
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
            };
            upsertQuestions.token = token;



            var dataResponse = await _quizRepository.UpsertQuestions(upsertQuestions);
            if (dataResponse.errorCode == 0)
                return Json(new { StatusCode = 200 });
            else if (dataResponse.errorCode == 143)
            {
                await RefreshToken();
                return await UpsertQuestion(upsertQuestionVM);
            }

            return View("Error");
        }


        //[HttpGet]
        public IActionResult CreateQuestionnaire()
        {
            var upsertVm = new QuestionnaireViewModel();
            upsertVm.oid = 0;
            upsertVm.Questions = new List<QuestionViewModel>();
            return View("~/Views/Home/Upsert.cshtml", upsertVm);

        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestionnaire([FromBody] UpsertQuestionnareViewModel upsertQuestionnaireVM)
        {

            string token = GetToken();

            var userData = await _userRepository.getProfileInfo(token);

            if (userData.ErrorCode == 143)
            {
                await RefreshToken();
                return await CreateQuestionnaire(upsertQuestionnaireVM);
            }
            else if (userData.ErrorCode == 118)
            {
                return View("~/Views/Account/Login.cshtml");
            }
            else if (userData.ErrorCode == 0)
            {

                //Correct model for post
                //Data from body(scritp post)
                UpsertQuestionnaire upsertQuestionnaire = new UpsertQuestionnaire()
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


                UpsertQuestions questions = new UpsertQuestions()
                {
                    questions = questionsVM.questions,
                    token = token
                };



                var questionsBaseResponsed = await _quizRepository.UpsertQuestions(questions);

                if (questionnaireBaseResponsed.errorCode == 0 && questionsBaseResponsed.errorCode == 0)
                    return Json(new { StatusCode = 200 });
                    //return RedirectToAction("Index");
                
            }
            return View();

        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            var deleteVm = new QuestionnaireViewModel();
            var questionnaireData = await QuestionnaireDetail(id);
            var questionsData = await QuestionsDetail(id);

            if (questionnaireData.errorCode == 0 && questionsData.errorCode == 0)
            {
                deleteVm.oid = id;
                deleteVm.Title = questionnaireData.questionnaire.name;
                deleteVm.Questions = questionsData.questions;
                return PartialView("~/Views/Home/Delete.cshtml", deleteVm);
            }
            return View("Error");
        }

        //[HttpDelete, ActionName("Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteQuestionnaire([FromBody] int oid)
        {
            string token = GetToken();
            var baseResponse = await _quizRepository.DeleteQuestionnaire(token, oid);
            if (baseResponse.errorCode == 143)
            {
                await RefreshToken();
                return await DeleteQuestionnaire(oid);
            }
            else if (baseResponse.errorCode == 118)
                return View("~/Views/Account/Login.cshtml");
            else if (baseResponse.errorCode == 0)
                return Json(new { StatusCode = 200, Message = "Ok" });
            else
                return Json(new { StatusCode = 500, Message = baseResponse.errorMessage });

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteQuestion([FromBody] int id)
        {
            string token = GetToken();
            var baseResponse = await _quizRepository.DeleteQuestion(token, id);
            if (baseResponse.errorCode == 143)
            {
                await RefreshToken();
                return await DeleteQuestion(id);
            }
            else if (baseResponse.errorCode == 118)
                return View("~/Views/Account/Login.cshtml");
            else if (baseResponse.errorCode == 0)
                return Json(new { StatusCode = 200, oid = id });
            else
                return Json(new { StatusCode = 500, Message = baseResponse.errorMessage });
        }

    }
}

