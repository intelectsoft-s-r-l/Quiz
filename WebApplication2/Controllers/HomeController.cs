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
        private readonly ILogger<HomeController> _logger;
        //private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IQuizRepository quizRepository,
                             IUserRepository userRepository,
                             ILogger<HomeController> logger
                             /*, IStringLocalizer<HomeController> localizer*/)
        {
            _quizRepository = quizRepository;
            _userRepository = userRepository;
            _logger = logger;
            //_localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Home.Index method called.");

            try
            {
                string token = GetToken();

                var questionnaireData = await _quizRepository.GetQuestionnaires(token);

                if (questionnaireData.errorCode == 143)
                {
                    await RefreshToken();
                    return await Index();
                }
                else if (questionnaireData.errorCode != 0)
                {
                    _logger.LogError($"Received unknown errorCode: {questionnaireData.errorCode}");
                    return View("~/Views/Account/Login.cshtml");
                }
                return View("~/Views/Home/Index.cshtml", questionnaireData.questionnaires);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the Home.Index method." + ex.Message);
                throw;
            }
        }


        [HttpGet]
        public IActionResult Detail(int id) => View("~/Views/Home/Detail.cshtml", id);


        public async Task<DetailQuestionnaire> QuestionnaireDetail(int id)
        {
            try
            {
                string token = GetToken();
                var questionnaireData = await _quizRepository.GetQuestionnaire(token, id);
                if (questionnaireData.errorCode == 143)
                {
                    await RefreshToken();
                    return await QuestionnaireDetail(id);
                }
                else if (questionnaireData.errorCode != 0)
                    _logger.LogError($"{questionnaireData}");
                //if err == 0
                return questionnaireData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the QuestionnaireDetail method." + ex.Message);
                throw;
            }


        }

        public async Task<DetailQuestions> QuestionsDetail(int id)
        {
            try
            {
                string token = GetToken();
                var questionsData = await _quizRepository.GetQuestions(token, id);
                if (questionsData.errorCode == 143)
                {
                    await RefreshToken();
                    return await QuestionsDetail(id);
                }
                else if (questionsData.errorCode != 0)
                    _logger.LogError($"{questionsData}");
                //if err == 0
                return questionsData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the QuestionsDetail method." + ex.Message);
                throw;
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetInfoQuestionnaire(int id)
        {

            _logger.LogInformation($"GetInfoQuestionnaire method called. Id = {id}");

            try
            {
                var questionnaireData = await QuestionnaireDetail(id);
                //var questionsData = await QuestionsDetail(id);

                if (questionnaireData.errorCode == 0 /*&& questionsData.errorCode == 0*/)
                {
                    var detailQuestionnaireVm = new QuestionnaireViewModel
                    {
                        oid = id,
                        Title = questionnaireData.questionnaire.name,
                        //Questions = questionsData.questions
                    };
                    //return View("~/Views/Home/Detail.cshtml", id);
                    return PartialView("~/Views/Home/_Info.cshtml", detailQuestionnaireVm);
                }
                else
                    _logger.LogError($"{questionnaireData} \n");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the GetInfoQuestionnaire method." + ex.Message);
                throw;
            }
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> GetQuestions(int id)
        {
            _logger.LogInformation($"GetQuestions method called. Id = {id}");
            try
            {
                var questionsData = await QuestionsDetail(id);

                if (questionsData.errorCode == 0)
                    return PartialView("~/Views/Home/_Questions.cshtml", questionsData);
                else
                    _logger.LogError($"{questionsData}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the GetQuestions method." + ex.Message);
                throw;
            }

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> QuestionnaireStatistic(int id)
        {

            _logger.LogInformation($"QuestionnaireStatistic method called. Id = {id}");
            try
            {
                string token = GetToken();

                var statistic = await _quizRepository.GetQuestionnaireStatistic(token, id);

                if (statistic.errorCode == 143)
                {
                    await RefreshToken();
                    return await QuestionnaireStatistic(id);
                }
                else if (statistic.errorCode == 0)
                    return PartialView("~/Views/Home/_Statistic.cshtml", statistic.questionnaireStatistic);
                else
                    _logger.LogError($"{statistic}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the QuestionnaireStatistic method." + ex.Message);
                throw;
            }


            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UpsertQuestionnaire([FromBody] UpsertQuestionnaire upsertQuestionnaireVM)
        {

            _logger.LogInformation($"UpsertQuestionnaire method called.");

            try
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
                        return await UpsertQuestionnaire(upsertQuestionnaireVM);
                    }
                    else if (questionnaireBaseResponsed.errorCode == 0)
                    {
                        return Json(new { StatusCode = 200 });
                        //return RedirectToAction("Index");
                    }
                    else
                        _logger.LogError($"{upsertQuestionnaireVM}\n{questionnaireBaseResponsed}");
                }
                else
                    _logger.LogError($"{userData.ErrorCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the UpsertQuestionnaire method." + ex.Message);
                throw;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpsertQuestion([FromBody] UpsertQuestionsViewModel upsertQuestionVM)
        {
            _logger.LogInformation($"UpsertQuestion method called.");
            try
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
                if (dataResponse.errorCode == 0)
                    return Json(new { StatusCode = 200 });
                else if (dataResponse.errorCode == 143)
                {
                    await RefreshToken();
                    return await UpsertQuestion(upsertQuestionVM);
                }
                else
                    _logger.LogError($"{dataResponse}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the UpsertQuestionnaire method." + ex.Message);
                throw;
            }


            return View();
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

            _logger.LogInformation($"CreateQuestionnaire method called.");

            try
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

                    if (questionnaireBaseResponsed.errorCode == 0 && questionsBaseResponsed.errorCode == 0)
                        return Json(new { StatusCode = 200 });
                    //return RedirectToAction("Index");

                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the CreateQuestionnaire method.");
                return View("Error");
            }
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Delete methodGET called.");
            try
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the Delete method.");
            }
            return View("Error");
        }

        //[HttpDelete, ActionName("Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteQuestionnaire([FromBody] int oid)
        {
            _logger.LogInformation($"DeleteQuestionnaire methodPOST called.");
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the DeleteQuestionnaire method.");
                return View("Error");
            }


        }

        [HttpDelete]
        public async Task<IActionResult> DeleteQuestion([FromBody] int id)
        {
            _logger.LogInformation($"DeleteQuestion method called.");
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the DeleteQuestion method.");
                return View("Error");
            }


        }

    }
}

