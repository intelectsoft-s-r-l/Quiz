using ISQuiz.Filter;
using ISQuiz.Interface;
using ISQuiz.Models.API.Questionnaires;
using ISQuiz.Models.Enum;
using ISQuiz.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;

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
            try
            {
                string token = GetToken();

                var questionnaireData = await _quizRepository.GetQuestionnaires(token);

                if (questionnaireData.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await Index();
                }
                else if (questionnaireData.errorCode != EnErrorCode.NoError)
                {
                    throw new Exception(questionnaireData.errorMessage);
                }
                /*else if (questionnaireData.errorCode != 0)
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }*/
                return View("~/Views/Home/Index.cshtml", questionnaireData.questionnaires);

            }
            catch (Exception ex)
            {

                return PartialView("~/Views/_Shared/Error.cshtml");
            }
        }


        [HttpGet]
        public IActionResult Detail(int id) => View("~/Views/Home/Detail.cshtml", id);


        private async Task<DetailQuestionnaire> QuestionnaireDetail(int id)
        {

            try
            {
                string token = GetToken();
                var questionnaireData = await _quizRepository.GetQuestionnaire(token, id);

                if (questionnaireData.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await QuestionnaireDetail(id);
                }
                else if (questionnaireData.errorCode != EnErrorCode.NoError)
                {
                    throw new Exception(questionnaireData.errorMessage);
                }
                return questionnaireData;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private async Task<DetailQuestions> QuestionsDetail(int id)
        {
            try
            {
                string token = GetToken();
                var questionsData = await _quizRepository.GetQuestions(token, id);

                if (questionsData.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await QuestionsDetail(id);
                }
                else if (questionsData.errorCode != EnErrorCode.NoError)
                {
                    throw new Exception(questionsData.errorMessage);
                }
                //if err == 0
                return questionsData;
            }
            catch (Exception ex)
            {

                throw;
            }


        }


        [HttpGet]
        public async Task<IActionResult> GetInfoQuestionnaire(int id)
        {

            try
            {
                var questionnaireData = await QuestionnaireDetail(id);
                if (questionnaireData.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await GetInfoQuestionnaire(id);
                }
                else if (questionnaireData.errorCode != EnErrorCode.NoError)
                {
                    throw new Exception(questionnaireData.errorMessage);
                }

                var detailQuestionnaireVm = new QuestionnaireViewModel
                {
                    oid = id,
                    Title = questionnaireData.questionnaire.name
                };

                return PartialView("~/Views/Home/_Info.cshtml", detailQuestionnaireVm);


            }
            catch (Exception ex)
            {

                return PartialView("~/Views/_Shared/Error.cshtml");
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetQuestions(int id)
        {
            try
            {
                var questionsData = await QuestionsDetail(id);
                if (questionsData.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await GetQuestions(id);
                }
                else if (questionsData.errorCode != EnErrorCode.NoError)
                {
                    throw new Exception(questionsData.errorMessage);
                }

                return PartialView("~/Views/Home/_Questions.cshtml", questionsData);
            }
            catch (Exception ex)
            {
                return View("~/Views/_Shared/Error.cshtml");
            }
        }


        [HttpGet]
        public async Task<IActionResult> QuestionnaireStatistic(int id)
        {

            try
            {
                string token = GetToken();

                var statistic = await _quizRepository.GetQuestionnaireStatistic(token, id);

                if (statistic.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await QuestionnaireStatistic(id);
                }
                else if (statistic.errorCode != EnErrorCode.NoError)
                {
                    throw new Exception(statistic.errorMessage);
                }

                return PartialView("~/Views/Home/_Statistic.cshtml", statistic.questionnaireStatistic);
            }
            catch (Exception ex)
            {

                return PartialView("~/Views/_Shared/Error.cshtml");
            }

        }


        [HttpPost]
        public async Task<IActionResult> UpsertQuestionnaire([FromBody] UpsertQuestionnaire upsertQuestionnaireVM)
        {


            try
            {
                string token = GetToken();

                var userData = await _userRepository.getProfileInfo(token);

                if (userData.ErrorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await UpsertQuestionnaire(upsertQuestionnaireVM);
                }
                else if (userData.ErrorCode != EnErrorCode.NoError)
                {
                    throw new Exception(userData.ErrorMessage);
                }

                upsertQuestionnaireVM.companyOid = userData.User.CompanyID;
                upsertQuestionnaireVM.company = userData.User.Company;
                upsertQuestionnaireVM.token = token;

                var questionnaireBaseResponsed = await _quizRepository.UpsertQuestionnaire(upsertQuestionnaireVM);

                if (userData.ErrorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await UpsertQuestionnaire(upsertQuestionnaireVM);
                }
                else if (userData.ErrorCode != EnErrorCode.NoError)
                {
                    throw new Exception(userData.ErrorMessage);
                }

                return Json(new { StatusCode = 200 });


            }
            catch (Exception ex)
            {

                return PartialView("~/Views/_Shared/Error.cshtml");
            }



        }

        [HttpPost]
        public async Task<IActionResult> UpsertQuestion([FromBody] UpsertQuestionsViewModel upsertQuestionVM)
        {

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
                if (dataResponse.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await UpsertQuestion(upsertQuestionVM);
                }
                else if (dataResponse.errorCode != EnErrorCode.NoError)
                {
                    throw new Exception(dataResponse.errorMessage);
                }
                return Json(new { StatusCode = 200 });
            }
            catch (Exception ex)
            {

                return PartialView("~/Views/_Shared/Error.cshtml");
            }

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



            try
            {
                string token = GetToken();

                var userData = await _userRepository.getProfileInfo(token);

                if (userData.ErrorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await CreateQuestionnaire(upsertQuestionnaireVM);
                }
                else if (userData.ErrorCode != EnErrorCode.NoError)
                {
                    throw new Exception(userData.ErrorMessage);
                }

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
                else
                {
                    throw new Exception(questionnaireBaseResponsed.errorMessage + " ||| " + questionsBaseResponsed.errorMessage);
                }
                //return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                return PartialView("~/Views/_Shared/Error.cshtml");
            }

        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleteVm = new QuestionnaireViewModel();
                var questionnaireData = await QuestionnaireDetail(id);
                var questionsData = await QuestionsDetail(id);

                if (questionnaireData.errorCode == EnErrorCode.NoError && questionsData.errorCode == EnErrorCode.NoError)
                {
                    deleteVm.oid = id;
                    deleteVm.Title = questionnaireData.questionnaire.name;
                    deleteVm.Questions = questionsData.questions;
                    return PartialView("~/Views/Home/Delete.cshtml", deleteVm);
                }
                else
                    throw new Exception(questionnaireData.errorMessage + " ||| " + questionsData.errorMessage);
                
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/_Shared/Error.cshtml");
            }
        }

        //[HttpDelete, ActionName("Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteQuestionnaire([FromBody] int oid)
        {

            try
            {
                string token = GetToken();

                var baseResponse = await _quizRepository.DeleteQuestionnaire(token, oid);

                if (baseResponse.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await DeleteQuestionnaire(oid);
                }
                else if (baseResponse.errorCode != EnErrorCode.NoError)
                {
                    throw new Exception(baseResponse.errorMessage);
                }
                return Json(new { StatusCode = 200, Message = "Ok" });

            }
            catch (Exception ex)
            {
                return PartialView("~/Views/_Shared/Error.cshtml");
            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteQuestion([FromBody] int id)
        {
            try
            {
                string token = GetToken();
                var baseResponse = await _quizRepository.DeleteQuestion(token, id);

                if (baseResponse.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await DeleteQuestion(id);
                }
                else if (baseResponse.errorCode != EnErrorCode.NoError)
                {
                    throw new Exception(baseResponse.errorMessage);
                }
                return Json(new { StatusCode = 200, oid = id });
            }
            catch (Exception ex)
            {
                
                return PartialView("~/Views/_Shared/Error.cshtml");
            }

        }



        [Route("/404")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> NotFound() => PartialView("~/Views/_Shared/_NotFound.cshtml");


    }
}

