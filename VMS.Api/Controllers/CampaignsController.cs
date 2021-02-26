using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VMS.Api.Exceptions;
using VMS.Api.Helpers;
using VMS.Api.Interfaces;
using VMS.Api.Models.Campaigns;
using VMS.Core.Entities;
using VMS.Core.Enums;
using VMS.Core.Interfaces.Repositories;

namespace VMS.Api.Controllers
{
    [Route("campaigns")]
    [ApiController]
    public class CampaignsController : BaseApiController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICampaignRepository _campaignRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWardRepository _wardRepository;

        public CampaignsController(
            IAccountRepository accountRepository, 
            ICampaignRepository campaignRepository, 
            ICategoryRepository categoryRepository, 
            IWardRepository wardRepository,
            IBaseApiService service
            ) : base(service)
        {
            _accountRepository = accountRepository;
            _campaignRepository = campaignRepository;
            _categoryRepository = categoryRepository;
            _wardRepository = wardRepository;
        }

        /// <summary>
        /// POST /campaigns
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("")]
        [VmsAuthorize(FunctionCodes = "Campaign_Full, Campaign_Create")]
        public async Task<IActionResult> CreateCampaignAsync([FromBody] CampaignCreateModel model)
        {
            if (model.WardId == 0)
            {
                throw new IsRequiredException("ward");
            }

            if (!await _wardRepository.AnyByIdAsync(model.WardId))
            {
                throw new NotFound400Exception("ward");
            }

            if (model.CategoryId == 0)
            {
                throw new IsRequiredException("category");                
            }

            if (!await _categoryRepository.AnyByIdAsync(model.CategoryId))
            {
                throw new NotFound400Exception("category");
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new IsRequiredException("name");
            }

            if (model.Name.Length > 50)
            {
                throw new NameIsInvalidException();
            }

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                throw new IsRequiredException("description");
            }

            if (model.Description.Length < 20)
            {
                throw new DescriptionIsInvalidException();
            }

            if (string.IsNullOrWhiteSpace(model.Location))
            {
                throw new IsRequiredException("location");
            }

            if (model.Location.Length > 100)
            {
                throw new LocationIsInvalidException();
            }

            if ((int)model.Type < 0 || (int)model.Type > Enum.GetNames(typeof(CampaignType)).Length)
            {
                throw new TypeIsInvalidException();
            }

            if (model.RegistrationStartDate == DateTime.MinValue)
            {
                throw new IsRequiredException("startDate");
            }

            if (model.RegistrationEndDate == DateTime.MinValue)
            {
                throw new IsRequiredException("endDate");
            }

            if (model.RegistrationStartDate >= model.RegistrationEndDate)
            {
                throw new StartDateMustBeBeforeEndDateException();
            }

            if (model.ExecutionStartDate == DateTime.MinValue)
            {
                throw new IsRequiredException("startDate");
            }

            if (model.ExecutionEndDate == DateTime.MinValue)
            {
                throw new IsRequiredException("endDate");
            }

            if (model.ExecutionStartDate >= model.ExecutionEndDate)
            {
                throw new StartDateMustBeBeforeEndDateException();
            }

            if (model.RegistrationEndDate.AddDays(2) >= model.ExecutionStartDate)
            {
                throw new ExecutionDateRangeMustBeAfterRegistrationDateRangeException();
            }

            var now = DateTime.Now;

            var campaign = new Campaign
            {
                AccountId = CurrentAccountId,
                CategoryId = model.CategoryId,
                WardId = model.WardId,
                Name = model.Name,
                Image = model.Image,
                Description = model.Description,
                Location = model.Location,
                Type = model.Type,
                RegistrationStartDate = model.RegistrationStartDate,
                RegistrationEndDate = model.RegistrationEndDate,
                ExecutionStartDate = model.ExecutionStartDate,
                ExecutionEndDate = model.ExecutionEndDate,
                CreatedDate = now,
                UpdatedDate = now
            };

            await _campaignRepository.CreateCampaignAsync(campaign);

            return Ok(CampaignDTO.GetFrom(campaign));
        }

        /// <summary>
        /// DELETE /campaigns/{campaignId}
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [HttpDelete, Route("{campaignId:int}")]
        [VmsAuthorize(FunctionCodes = "Campaign_Full, Campaign_Delete_All, Campaign_Delete")]
        public async Task<IActionResult> DeleteCampaignAsync([FromRoute] int campaignId)
        {
            var currentFunctionCodes = GetCurrentAccountFunctionCodes();
            var campaign = await _campaignRepository.GetCampaignByIdAsync(campaignId);

            if (campaign == null)
            {
                throw new NotFound404Exception("campaign");
            }

            if (!currentFunctionCodes.Contains("Campaign_Full"))
            {
                if (!currentFunctionCodes.Contains("Campaign_Delete_All"))
                {
                    if (campaign.AccountId != CurrentAccountId)
                    {
                        throw new ForbiddenException();
                    }
                }

                var currentAccount = await _accountRepository.GetAccountByIdAsync(CurrentAccountId);
                var account = await _accountRepository.GetAccountByIdAsync(campaign.AccountId);

                if (currentAccount.GroupId > account.GroupId)
                {
                    throw new ForbiddenException(); // the lower the group id, the higher the authority; can only delete the group with authority lower than the current group
                }
            }

            campaign.IsDeleted = true;
            campaign.UpdatedDate = DateTime.Now;

            await _campaignRepository.UpdateCampaignAsync(campaign);

            return Ok(CampaignDTO.GetFrom(campaign));
        }

        /// <summary>
        /// GET /campaigns/{campaignId}
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [HttpGet, Route("{campaignId:int}")]
        public async Task<IActionResult> GetCampaignByIdAsync([FromRoute] int campaignId)
        {
            var campaign = await _campaignRepository.GetCampaignByIdAsync(campaignId);

            if (campaign == null)
            {
                throw new NotFound404Exception("campaign");
            }

            return Ok(CampaignDTO.GetFrom(campaign));
        }

        /// <summary>
        /// GET /campaigns
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet, Route("")]
        public async Task<IActionResult> GetCampaignsAsync([FromQuery] CampaignGetModel model)
        {
            model.Validate();

            var list = await _campaignRepository.GetCampaignsAsync(model.Keyword, model.Category, model.Type, model.DateType, model.Period, model.Page, model.PageSize);

            if (list == null)
            {
                throw new NotFound400Exception("page");
            }

            return Ok(CampaignList.GetFrom(list));
        }

        /// <summary>
        /// PUT /campaigns/{campaignId}
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route("{campaignId:int}")]
        [VmsAuthorize(FunctionCodes = "Campaign_Full, Campaign_Modify")]
        public async Task<IActionResult> UpdateCampaignAsync([FromRoute] int campaignId, [FromBody] CampaignUpdateModel model)
        {
            var currentFunctionCodes = GetCurrentAccountFunctionCodes();
            var campaign = await _campaignRepository.GetCampaignByIdAsync(campaignId);

            if (campaign == null)
            {
                throw new NotFound404Exception("campaign");
            }

            if (!currentFunctionCodes.Contains("Campaign_Full"))
            {
                if (campaign.AccountId != CurrentAccountId)
                {
                    throw new ForbiddenException();
                }
            }            

            if (model.WardId == 0)
            {
                throw new IsRequiredException("ward");
            }

            if (!await _wardRepository.AnyByIdAsync(model.WardId))
            {
                throw new NotFound400Exception("ward");
            }

            if (model.CategoryId == 0)
            {
                throw new IsRequiredException("category");
            }

            if (!await _categoryRepository.AnyByIdAsync(model.CategoryId))
            {
                throw new NotFound400Exception("category");
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new IsRequiredException("name");
            }

            if (model.Name.Length > 50)
            {
                throw new NameIsInvalidException();
            }

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                throw new IsRequiredException("description");
            }

            if (model.Description.Length < 20)
            {
                throw new DescriptionIsInvalidException();
            }

            if (string.IsNullOrWhiteSpace(model.Location))
            {
                throw new IsRequiredException("location");
            }

            if (model.Location.Length > 100)
            {
                throw new LocationIsInvalidException();
            }

            if ((int)model.Type < 0 || (int)model.Type > Enum.GetNames(typeof(CampaignType)).Length)
            {
                throw new TypeIsInvalidException();
            }

            if (model.RegistrationStartDate == DateTime.MinValue)
            {
                throw new IsRequiredException("startDate");
            }

            if (model.RegistrationEndDate == DateTime.MinValue)
            {
                throw new IsRequiredException("endDate");
            }

            if (model.RegistrationStartDate >= model.RegistrationEndDate)
            {
                throw new StartDateMustBeBeforeEndDateException();
            }

            if (model.ExecutionStartDate == DateTime.MinValue)
            {
                throw new IsRequiredException("startDate");
            }

            if (model.ExecutionEndDate == DateTime.MinValue)
            {
                throw new IsRequiredException("endDate");
            }

            if (model.ExecutionStartDate >= model.ExecutionEndDate)
            {
                throw new StartDateMustBeBeforeEndDateException();
            }

            if (model.RegistrationEndDate.AddDays(2) >= model.ExecutionStartDate)
            {
                throw new ExecutionDateRangeMustBeAfterRegistrationDateRangeException();
            }

            // bind data
            campaign.CategoryId = model.CategoryId;
            campaign.WardId = model.WardId;
            campaign.Name = model.Name;
            campaign.Image = model.Image;
            campaign.Description = model.Description;
            campaign.Location = model.Location;
            campaign.Type = model.Type;
            campaign.RegistrationStartDate = model.RegistrationStartDate;
            campaign.RegistrationEndDate = model.RegistrationEndDate;
            campaign.ExecutionStartDate = model.ExecutionStartDate;
            campaign.ExecutionEndDate = model.ExecutionEndDate;
            campaign.UpdatedDate = DateTime.Now;

            await _campaignRepository.UpdateCampaignAsync(campaign);

            return Ok(CampaignDTO.GetFrom(campaign));
        }
    }
}