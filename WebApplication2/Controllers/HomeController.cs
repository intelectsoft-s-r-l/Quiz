using ISAdminWeb.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using WebApplication2.Interface;
using WebApplication2.Models;
using WebApplication2.Repository;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    [Authorize]
    [Culture]
    public class HomeController : BaseController
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IQuizRepository quizRepository, IUserRepository userRepository, IStringLocalizer<HomeController> localizer)
        {
            _quizRepository = quizRepository;
            _userRepository = userRepository;
            _localizer = localizer;
        }

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


        [HttpGet]
        public async Task<IActionResult> GetInfoQuestionnaire(int id)
        {
            string token = GetToken();
            var questionnaireData = await _quizRepository.GetQuestionnaire(token, id);
            if (questionnaireData.errorCode == 143)
            {
                await RefreshToken();
                return await GetInfoQuestionnaire(id);
            }
            else if (questionnaireData.errorCode == 118)
            {
                return View("~/Views/Account/Login.cshtml");
            }
            else if (questionnaireData.errorCode == 0)
            {
                var detailQuestionnaireVm = new QuestionnaireViewModel();
                detailQuestionnaireVm.oid = id;
                detailQuestionnaireVm.Title = questionnaireData.questionnaire.name;
                detailQuestionnaireVm.Questions = questionnaireData.questionnaire.questions;
                //return View("~/Views/Home/Detail.cshtml", id);
                return PartialView("~/Views/Home/_Info.cshtml", detailQuestionnaireVm);

            }
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> QuestionnaireResponses(int id)
        {
            string token = GetToken();
            var questionnaireData = await _quizRepository.GetResponses(token, id);
            if (questionnaireData.errorCode == 143)
            {
                await RefreshToken();
                return await QuestionnaireResponses(id);
            }
            else if (questionnaireData.errorCode == 118)
            {
                return View("~/Views/Account/Login.cshtml");
            }
            else if (questionnaireData.errorCode == 0)
            {
                //return View("~/Views/Home/Detail.cshtml", id);
                return PartialView("~/Views/Home/_Responses.cshtml", questionnaireData.responses);

            }
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> QuestionnaireResponse(int id)
        {
            string token = GetToken();
            var questionnaireData = await _quizRepository.GetResponse(token, id);
            if (questionnaireData.errorCode == 143)
            {
                await RefreshToken();
                return await QuestionnaireResponse(id);
            }
            else if (questionnaireData.errorCode == 118)
            {
                return View("~/Views/Account/Login.cshtml");
            }
            else if (questionnaireData.errorCode == 0)
            {
                //return View("~/Views/Home/Detail.cshtml", id);
                return PartialView("~/Views/Home/_Response.cshtml", questionnaireData.responses);

            }
            return View("Error");
        }

        //[HttpGet]
        public async Task<IActionResult> Upsert(int id)
        {
            string token = GetToken();
            var upsertVm = new QuestionnaireViewModel();
            if (id != 0)
            {
                var questionnaireData = await _quizRepository.GetQuestionnaire(token, id);
                if (questionnaireData.errorCode == 143)
                {
                    await RefreshToken();
                    return await Upsert(id);
                }
                else if (questionnaireData.errorCode == 118)
                {
                    return View("~/Views/Account/Login.cshtml");
                }
                else if (questionnaireData.errorCode == 0)
                {
                    upsertVm.oid = id;
                    upsertVm.Title = questionnaireData.questionnaire.name;
                    upsertVm.Questions = questionnaireData.questionnaire.questions;
                    return View("~/Views/Home/Upsert.cshtml", upsertVm);

                }

            }
            upsertVm.oid = id;
            upsertVm.Questions = new List<QuestionViewModel>();
            return View("~/Views/Home/Upsert.cshtml", upsertVm);

        }

        [HttpPost]
        public async Task<IActionResult> UpsertQuestionnaire([FromBody] UpsertQuestionnareViewModel upsertQuestionnaireVM)
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
                var questionsVM = JsonConvert.DeserializeObject<QuestionsViewModel>(upsertQuestionnaireVM.Questions);

                //Correct model for post
                //Data from body(scritp post)
                UpsertQuestionnaire upsertQuestionnaire = new UpsertQuestionnaire()
                {
                    oid = upsertQuestionnaireVM.id,
                    name = upsertQuestionnaireVM.Title,
                    questions = questionsVM.questions,
                    companyOid = userData.User.CompanyID,
                    token = token,
                    company = userData.User.Company
                };

                var questionnaireBaseResponsed = await _quizRepository.Upsert(upsertQuestionnaire);
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



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            string token = GetToken();
            var questionnaireData = await _quizRepository.GetQuestionnaire(token, id);
            if (questionnaireData.errorCode == 143)
            {
                await RefreshToken();
                return await Delete(id);
            }
            else if (questionnaireData.errorCode == 118)
                return View("~/Views/Account/Login.cshtml");
            else if (questionnaireData.errorCode == 0)
                return PartialView("~/Views/Home/Delete.cshtml", questionnaireData);

            return View("Error");
        }
        //[HttpDelete, ActionName("Delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteQuestionnaire([FromBody] int oid)
        {
            string token = GetToken();
            var baseResponse = await _quizRepository.Delete(token, oid);
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


    }
}

