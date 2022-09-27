using Microsoft.AspNetCore.Mvc;
using MISA.Web06.APIS.Core.Entities;
using MISA.Web06.APIS.Core.Interfaces.Infrastructure;

namespace MISA.Web06.APIS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserOptionsController : ControllerBase
    {
        #region Properties
        private readonly IConfiguration _configuration;
        private readonly List<Column> _columnOption;
        private readonly IUserOptionsRepository _userOptionsRepository;
        #endregion

        #region Constructor
        public UserOptionsController(IConfiguration config, IUserOptionsRepository userOptionsRepository)
        {
            _configuration = config;
            _columnOption = _configuration.GetSection("ColumnOption").Get<List<Column>>().ToList();
            _userOptionsRepository = userOptionsRepository; 

        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy thông tin của tùy chọn cột
        /// </summary>
        /// <returns>Thông tin của tùy chọn cột</returns>
        /// CreateBy: TNDanh (13/9/2022)
        [HttpGet()]
        public IActionResult GetColumnOptions()
        {
            try
            {
            var res = _userOptionsRepository.GetAll();
            return Ok(res);

            }
            catch (Exception ex)
            {   
                return BadRequest(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Xét bộ tùy chỉnh cột mới
        /// </summary>
        /// <param name="columns">Thông tin bộ tùy chỉnh cột</param>
        /// <returns>Số lượng tùy chỉnh cột</returns>
        /// Author: TNDanh (19/9/2022)
        [HttpPost("New")]
        public async Task<IActionResult> SetColumnOptions(List<Column> columns)
        {
            try
            {
                var res = await _userOptionsRepository.SetUserOptions(columns);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Xét bộ tùy chỉnh cột mặc định
        /// </summary>
        /// <returns>Số lượng tùy chỉnh cột</returns>
        /// Author: TNDanh (19/9/2022)
        [HttpPost("Default")]
        public async Task<IActionResult> SetDefaultColumnOptions()
        {
            try
            {
                var res = await _userOptionsRepository.SetUserOptions(_columnOption);
                return Ok(res);
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
