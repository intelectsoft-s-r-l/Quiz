using ISQuiz.Filter;
using ISQuiz.Interface;
using ISQuiz.Models.API.Questionnaires;
using ISQuiz.Models.Enum;
using ISQuiz.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

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
            //Log.Information("Into Home.Index");
            try
            {
                string token = GetToken();

                var questionnairesData = await _quizRepository.GetQuestionnaires(token);

                if (questionnairesData.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken())
                    {
                        return await Index();
                    }
                }
                else if (questionnairesData.errorCode == EnErrorCode.Invalid_token)
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }
                else if (questionnairesData.errorCode != EnErrorCode.NoError)
                {
                    Log.Information("Response => {@questionnairesData}", questionnairesData);
                    throw new Exception(questionnairesData.errorName + "|||" + questionnairesData.errorMessage);
                }

                return View("~/Views/Home/Index.cshtml", questionnairesData.questionnaires);

                /*else if (questionnaireData.errorCode != 0)
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }*/


            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return PartialView("~/Views/_Shared/Error.cshtml", ex);
            }
        }


        [HttpGet]
        public IActionResult Detail(int id) => View("~/Views/Home/Detail.cshtml", id);


        private async Task<DetailQuestionnaire> QuestionnaireDetail(int id)
        {
            //Log.Information("Into Home.QuestionnaireDetail");
            try
            {
                string token = GetToken();
                var questionnaireData = await _quizRepository.GetQuestionnaire(token, id);

                if (questionnaireData.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken())
                    {
                        return await QuestionnaireDetail(id);
                    }
                }
                else if (questionnaireData.errorCode == EnErrorCode.Invalid_token)
                {
                    return new DetailQuestionnaire { errorCode = EnErrorCode.Invalid_token, errorMessage = questionnaireData.errorMessage};
                }
                else if (questionnaireData.errorCode != EnErrorCode.NoError)
                {
                    Log.Information("Response => {@questionnaireData}", questionnaireData);
                    throw new Exception(questionnaireData.errorName + "|||" + questionnaireData.errorMessage);
                }
                return questionnaireData;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw;
            }

        }

        private async Task<DetailQuestions> QuestionsDetail(int id)
        {
            //Log.Information("Into Home.QuestionsDetail");
            try
            {
                string token = GetToken();
                var questionsData = await _quizRepository.GetQuestions(token, id);

                if (questionsData.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken())
                    {
                        return await QuestionsDetail(id);
                    }
                }
                else if (questionsData.errorCode == EnErrorCode.Invalid_token)
                {
                    return new DetailQuestions { errorCode = EnErrorCode.Invalid_token, errorMessage = questionsData.errorMessage };
                }
                else if (questionsData.errorCode != EnErrorCode.NoError)
                {
                    Log.Information("Response => {@questionsData}", questionsData);
                    throw new Exception(questionsData.errorName + "|||" + questionsData.errorMessage);
                }
                //if err == 0
                return questionsData;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw;
            }


        }


        [HttpGet]
        public async Task<IActionResult> GetInfoQuestionnaire(int id)
        {
            //Log.Information("Into Home.GetInfoQuestionnaire");
            try
            {
                var questionnaireData = await QuestionnaireDetail(id);
                if (questionnaireData.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken())
                    {
                        return await GetInfoQuestionnaire(id);
                    }
                }
                else if (questionnaireData.errorCode == EnErrorCode.Invalid_token)
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }
                else if (questionnaireData.errorCode != EnErrorCode.NoError)
                {
                    Log.Information("Response => {@questionnaireData}", questionnaireData);
                    throw new Exception(questionnaireData.errorName + "|||" + questionnaireData.errorMessage);
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
                Log.Error(ex, ex.Message);
                return PartialView("~/Views/_Shared/Error.cshtml", ex);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetQuestions(int id)
        {
            //Log.Information("Into Home.GetQuestions");
            try
            {
                var questionsData = await QuestionsDetail(id);
                if (questionsData.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken())
                    {
                        return await GetQuestions(id);
                    }
                }
                else if (questionsData.errorCode == EnErrorCode.Invalid_token)
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }
                else if (questionsData.errorCode != EnErrorCode.NoError)
                {
                    Log.Information("Response => {@questionsData}", questionsData);
                    throw new Exception(questionsData.errorName + "|||" + questionsData.errorMessage);
                }

                return PartialView("~/Views/Home/_Questions.cshtml", questionsData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return View("~/Views/_Shared/Error.cshtml", ex);
            }
        }


        [HttpGet]
        public async Task<IActionResult> QuestionnaireStatistic(int id)
        {
            //Log.Information("Into Home.QuestionnaireStatistic");
            try
            {
                string token = GetToken();

                var statistic = await _quizRepository.GetQuestionnaireStatistic(token, id);

                if (statistic.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken())
                    {
                        return await QuestionnaireStatistic(id);
                    }
                }
                else if (statistic.errorCode == EnErrorCode.Invalid_token)
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }
                else if (statistic.errorCode != EnErrorCode.NoError)
                {
                    Log.Information("Response => {@statistic}", statistic);
                    throw new Exception(statistic.errorName + "|||" + statistic.errorMessage);
                }

                return PartialView("~/Views/Home/_Statistic.cshtml", statistic.questionnaireStatistic);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return PartialView("~/Views/_Shared/Error.cshtml", ex);
            }

        }


        [HttpPost]
        public async Task<IActionResult> UpsertQuestionnaire([FromBody] UpsertQuestionnaire upsertQuestionnaireVM)
        {

            //Log.Information("Into Home.UpsertQuestionnaire");
            try
            {
                string token = GetToken();

                var userData = await _userRepository.getProfileInfo(token);

                if (userData.ErrorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken())
                    {
                        return await UpsertQuestionnaire(upsertQuestionnaireVM);
                    }
                }
                else if (userData.ErrorCode == EnErrorCode.Invalid_token)
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }
                else if (userData.ErrorCode != EnErrorCode.NoError)
                {
                    Log.Information("Response => {@userData}", userData);
                    throw new Exception(userData.ErrorMessage);
                }

                upsertQuestionnaireVM.companyOid = userData.User.CompanyID;
                upsertQuestionnaireVM.company = userData.User.Company;
                upsertQuestionnaireVM.token = token;

                var questionnaireBaseResponsed = await _quizRepository.UpsertQuestionnaire(upsertQuestionnaireVM);

                if (userData.ErrorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken())
                    {
                        return await UpsertQuestionnaire(upsertQuestionnaireVM);
                    }
                }
                else if (userData.ErrorCode == EnErrorCode.Invalid_token)
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }
                else if (userData.ErrorCode != EnErrorCode.NoError)
                {
                    throw new Exception(userData.ErrorMessage);
                }

                return Json(new { StatusCode = 200 });


            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return PartialView("~/Views/_Shared/Error.cshtml", ex);
            }



        }

        [HttpPost]
        public async Task<IActionResult> UpsertQuestion([FromBody] UpsertQuestionsViewModel upsertQuestionVM)
        {
            //Log.Information("Into Home.UpsertQuestion");
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
                    if (await RefreshToken())
                    {
                        return await UpsertQuestion(upsertQuestionVM);
                    }
                }
                else if (dataResponse.errorCode == EnErrorCode.Invalid_token)
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }
                else if (dataResponse.errorCode != EnErrorCode.NoError)
                {
                    Log.Information("Response => {@dataResponse}", dataResponse);
                    throw new Exception(dataResponse.errorName + "|||" + dataResponse.errorMessage);
                }
                return Json(new { StatusCode = 200 });
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return PartialView("~/Views/_Shared/Error.cshtml", ex);
            }

        }

        //[HttpGet]
        public IActionResult CreateQuestionnaire()
        {
            //Log.Information("Into Home.CreateQuestionnaire");
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

            //Log.Information("Into Home.CreateQuestionnaire |Post");

            try
            {
                string token = GetToken();

                var userData = await _userRepository.getProfileInfo(token);

                if (userData.ErrorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken())
                    {
                        return await CreateQuestionnaire(upsertQuestionnaireVM);
                    }
                }
                else if (userData.ErrorCode == EnErrorCode.Invalid_token)
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }
                else if (userData.ErrorCode != EnErrorCode.NoError)
                {
                    Log.Information("Response => {@userData}", userData);
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
                else if (questionnaireBaseResponsed.errorCode == EnErrorCode.Invalid_token || questionsBaseResponsed.errorCode == EnErrorCode.Invalid_token)
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }
                else
                {
                    Log.Information("Response => " +
                        "questionsBaseResponsed: {@questionsBaseResponsed} ||| " +
                        "questionsBaseResponsed: {@questionsBaseResponsed}", questionsBaseResponsed, questionsBaseResponsed);
                    throw new Exception(questionnaireBaseResponsed.errorMessage + " ||| " + questionsBaseResponsed.errorMessage);
                }
                //return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return PartialView("~/Views/_Shared/Error.cshtml", ex);
            }

        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //Log.Information("Into Home.Delete");

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
                else if (questionnaireData.errorCode == EnErrorCode.Invalid_token || questionnaireData.errorCode == EnErrorCode.Invalid_token)
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }
                else
                {
                    Log.Information("Response => questionnaireData: {@questionnaireData}, questionsData: {@questionsData}", questionnaireData, questionsData);
                    throw new Exception(questionnaireData.errorMessage + " ||| " + questionsData.errorMessage);
                }


            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return PartialView("~/Views/_Shared/Error.cshtml", ex);
            }
        }

        //[HttpDelete, ActionName("Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteQuestionnaire([FromBody] int oid)
        {
            //Log.Information("Into Home.DeleteQuestionnaire");
            try
            {
                string token = GetToken();

                var baseResponse = await _quizRepository.DeleteQuestionnaire(token, oid);

                if (baseResponse.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken())
                    {
                        return await DeleteQuestionnaire(oid);
                    }
                }
                else if (baseResponse.errorCode == EnErrorCode.Invalid_token)
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }
                else if (baseResponse.errorCode != EnErrorCode.NoError)
                {
                    Log.Information("Response => {@baseResponse}", baseResponse);
                    throw new Exception(baseResponse.errorName + "|||" + baseResponse.errorMessage);
                }
                return Json(new { StatusCode = 200, Message = "Ok" });

            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return PartialView("~/Views/_Shared/Error.cshtml", ex);
            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteQuestion([FromBody] int id)
        {
            //Log.Information("Into Home.DeleteQuestion");
            try
            {
                string token = GetToken();
                var baseResponse = await _quizRepository.DeleteQuestion(token, id);

                if (baseResponse.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken())
                    {
                        return await DeleteQuestion(id);
                    }
                }
                else if (baseResponse.errorCode == EnErrorCode.Invalid_token)
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }
                else if (baseResponse.errorCode != EnErrorCode.NoError)
                {
                    Log.Information("Response => {@baseResponse}", baseResponse);
                    throw new Exception(baseResponse.errorName + "|||" + baseResponse.errorMessage);
                }
                return Json(new { StatusCode = 200, oid = id });
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return PartialView("~/Views/_Shared/Error.cshtml", ex);
            }

        }



        [Route("/404")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> NotFound() => PartialView("~/Views/_Shared/_NotFound.cshtml");


    }
}

