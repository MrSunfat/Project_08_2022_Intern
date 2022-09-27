using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web06.APIS.Core.Interfaces.Infrastructure;

namespace MISA.Web06.APIS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseEntitiesController<MISAEntity, EntityID> : ControllerBase
    {
        #region Properties
        protected IBaseRespository<MISAEntity> _respository;
        protected string typeEntity = "";
        #endregion

        #region Constructor
        public BaseEntitiesController(IBaseRespository<MISAEntity> respository)
        {
            _respository = respository;
            typeEntity = typeof(MISAEntity).Name;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy thông tin tất cả đối tượng
        /// </summary>
        /// <returns>Thông tin tất cả đối tượng</returns>
        /// CreatedBy: TNDanh (25/9/2022)
        [HttpGet]
        public virtual IActionResult GetAll()
        {
            try
            {
                var entities = _respository.GetAll();
                var res = new
                {
                    TotalRecord = entities.Count(),
                    Data = entities
                };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Lấy ra thông tin của đối tượng qua id
        /// </summary>
        /// <param name="id">ID của đối tượng</param>
        /// <returns>Thông tin của đối tượng qua id</returns>
        /// CreatedBy: TNDanh (25/9/2022)
        [HttpGet("{id}")]
        public virtual IActionResult GetByID(EntityID id)
        {
            try
            {
                var entity = _respository.GetByID(id);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Tìm kiếm và phân trang cho đối tượng
        /// </summary>
        /// <param name="pageSize">Số lượng đối tượng</param>
        /// <param name="pageNumber">Chỉ số trang</param>
        /// <param name="searchWord">Từ khóa tìm kiếm</param>
        /// <returns>CreatedBy: TNDanh(24/9/2022)</returns>
        [HttpGet("FilterEntity")]
        public virtual IActionResult GetFindAndPaging(int pageSize, int pageNumber, string? searchWord)
        {
            try
            {
                var res = _respository.GetFindAndPaging(pageSize, pageNumber, searchWord);
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
