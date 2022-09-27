using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web06.APIS.Core.Entities;
using MISA.Web06.APIS.Core.Interfaces.Infrastructure;

namespace MISA.Web06.APIS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class JobTitlesController : BaseEntitiesController<JobTitle, int>
    {
        #region Properties
        private readonly IJobTitlesRepository _jobTitlesRepository;
        #endregion

        #region Constructor
        public JobTitlesController(IJobTitlesRepository jobTitlesRepository) : base(jobTitlesRepository)
        {
            _jobTitlesRepository = jobTitlesRepository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy tất cả thông tin của chức vụ
        /// </summary>
        /// <returns>Lấy tất cả thông tin của chức vụ</returns>
        /// Author: TNDanh (13/9/2022)
        [HttpGet()]
        public override IActionResult GetAll()
        {
            try
            {
                var jobTitles = _jobTitlesRepository.GetAll();
                return Ok(jobTitles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        #endregion
    }
}
