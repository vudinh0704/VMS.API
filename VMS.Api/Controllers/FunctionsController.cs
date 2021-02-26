using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VMS.Api.Exceptions;
using VMS.Api.Helpers;
using VMS.Api.Interfaces;
using VMS.Api.Models.Functions;
using VMS.Core.Entities;
using VMS.Core.Interfaces.Repositories;

namespace VMS.Api.Controllers
{
    [Route("functions")]
    [ApiController]
    public class FunctionsController : BaseApiController
    {
        private readonly IFunctionRepository _functionRepository;

        public FunctionsController(IFunctionRepository repository, IBaseApiService service) : base(service)
        {
            _functionRepository = repository;
        }

        /// <summary>
        /// POST /functions
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("")]
        [VmsAuthorize(FunctionCodes = "Function_Full, Function_Create")]
        public async Task<IActionResult> CreateFunctionAsync([FromBody] FunctionCreateModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Code))
            {
                throw new IsRequiredException("code");
            }

            if (model.Code.Length > 20)
            {
                throw new CodeIsInvalidException();
            }

            if (await _functionRepository.AnyByCodeAsync(model.Code))
            {
                throw new AlreadyExistsException("code");
            }

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                throw new IsRequiredException("description");
            }

            if (model.Description.Length < 20)
            {
                throw new DescriptionIsInvalidException();
            }

            if (await _functionRepository.AnyByDescriptionAsync(model.Description))
            {
                throw new AlreadyExistsException("description");
            }            

            DateTime now = DateTime.Now;

            var function = new Function
            {
                Code = model.Code,
                Description = model.Description,
                CreatedDate = now,
                CreatedBy = CurrentAccountId,
                UpdatedDate = now,
                UpdatedBy = CurrentAccountId
            };

            await _functionRepository.CreateFunctionAsync(function);

            return Ok(FunctionDTO.GetFrom(function));
        }

        /// <summary>
        /// DELETE /functions/{functionId}
        /// </summary>
        /// <param name="functionId"></param>
        /// <returns></returns>
        [HttpDelete, Route("{functionId:int}")]
        [VmsAuthorize(FunctionCodes = "Function_Full, Function_Delete")]
        public async Task<IActionResult> DeleteFunctionAsync([FromRoute] int functionId)
        {
            var function = await _functionRepository.GetFunctionByIdAsync(functionId);

            if (function == null)
            {
                throw new NotFound404Exception("function");
            }

            if (function.Code.Contains("Full"))
            {
                throw new ForbiddenException(); // can not delete 'Full' functions
            }

            function.IsDeleted = true;
            function.UpdatedDate = DateTime.Now;
            function.UpdatedBy = CurrentAccountId;

            await _functionRepository.UpdateFunctionAsync(function);

            return Ok(FunctionDTO.GetFrom(function));
        }

        /// <summary>
        /// GET /functions/{functionId}
        /// </summary>
        /// <param name="functionId"></param>
        /// <returns></returns>
        [HttpGet, Route("{functionId:int}")]
        [VmsAuthorize(FunctionCodes = "Function_Full, Function_Read")]
        public async Task<IActionResult> GetFunctionByIdAsync([FromRoute] int functionId)
        {
            var function = await _functionRepository.GetFunctionByIdAsync(functionId);

            if (function == null)
            {
                throw new NotFound404Exception("function");
            }

            return Ok(FunctionDTO.GetFrom(function));
        }

        /// <summary>
        /// GET /functions
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet, Route("")]
        [VmsAuthorize(FunctionCodes = "Function_Full, Function_Read")]
        public async Task<IActionResult> GetFunctionsAsync([FromQuery] FunctionGetModel model)
        {
            model.Validate();

            var list = await _functionRepository.GetFunctionsAsync(model.Keyword, model.IsActive, model.Page, model.PageSize);

            if (list.Items.Count == 0)
            {
                throw new NotFound404Exception("page");
            }

            return Ok(FunctionList.GetFrom(list));
        }

        /// <summary>
        /// PUT /functions/{functionId}
        /// </summary>
        /// <param name="functionId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route("{functionId:int}")]
        [VmsAuthorize(FunctionCodes = "Function_Full, Function_Modify")]
        public async Task<IActionResult> UpdateFunctionAsync([FromRoute] int functionId, [FromBody] FunctionUpdateModel model)
        {
            var function = await _functionRepository.GetFunctionByIdAsync(functionId);

            if (function == null)
            {
                throw new NotFound404Exception("function");
            }

            if (function.Code.Contains("Full"))
            {
                throw new ForbiddenException();
            }

            if (string.IsNullOrWhiteSpace(model.Code))
            {
                throw new IsRequiredException("code");
            }

            if (model.Code.Length > 20)
            {
                throw new CodeIsInvalidException();
            }

            if (function.Code != model.Code && await _functionRepository.AnyByCodeAsync(model.Code))
            {
                throw new AlreadyExistsException("code");
            }            

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                throw new IsRequiredException("description");
            }

            if (model.Description.Length < 20)
            {
                throw new DescriptionIsInvalidException();
            }

            if (function.Description != model.Description && await _functionRepository.AnyByDescriptionAsync(model.Description))
            {
                throw new AlreadyExistsException("description");
            }            

            // bind data
            function.Code = model.Code;
            function.Description = model.Description;
            function.IsActive = model.IsActive;
            function.UpdatedDate = DateTime.Now;

            await _functionRepository.UpdateFunctionAsync(function);

            return Ok(FunctionDTO.GetFrom(function));
        }
    }
}