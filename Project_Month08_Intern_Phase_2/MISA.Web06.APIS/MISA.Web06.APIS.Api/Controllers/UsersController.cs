using Microsoft.AspNetCore.Mvc;
using MISA.Web06.APIS.Core.DTO;
using MISA.Web06.APIS.Core.Entities;
using MISA.Web06.APIS.Core.Interfaces.Infrastructure;
using MISA.Web06.APIS.Core.Interfaces.Services;
using MISA.Web06.APIS.Core.Resources;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.Drawing;

namespace MISA.Web06.APIS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : BaseEntitiesController<UserDTO, Guid>
    {
        #region Properties
        public IUsersRepository _usersRepository;
        public IJobTitlesRepository _jobTitlesRepository;
        public IUserGroupsRepository _userGroupsRepository;
        public IUserService _userSerivce;
        #endregion

        #region Constructor
        public UsersController(IUsersRepository usersRepository, IJobTitlesRepository jobTitlesRepository, IUserGroupsRepository userGroupsRepository, IUserService userService) : base(usersRepository)
        {
            _usersRepository = usersRepository;
            _jobTitlesRepository = jobTitlesRepository;
            _userGroupsRepository = userGroupsRepository;
            _userSerivce = userService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy thông tin tất cả người dùng
        /// </summary>
        /// <returns>Thông tin tất cả người dùng</returns>
        /// CreatedBy: TNDanh (4/9/2022)
        //[HttpGet]
        //public override IActionResult GetAll()
        //{
        //    try
        //    {
        //        var users = _usersRepository.GetAll();
        //        var res = new
        //        {
        //            TotalRecord = users.Count(),
        //            Data = users
        //        };
        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //        throw;
        //    }
        //}

        /// <summary>
        ///  Lấy thông tin người dùng qua userID
        /// </summary>
        /// <param name="userID">ID của người dùng</param>
        /// <returns>Thông tin người dùng qua userID</returns>
        /// CreatedBy: TNDanh (4/9/20222)
        //[HttpGet("{id}")]
        //public override IActionResult GetByID(Guid id)
        //{
        //    try
        //    {
        //        var entity = _respository.GetByID(id);
        //        return Ok(entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //        throw;
        //    }
        //}

        /// <summary>
        ///  Lấy thông tin người dùng qua userID
        /// </summary>
        /// <param name="userID">ID của người dùng</param>
        /// <returns>Thông tin người dùng qua userID</returns>
        /// CreatedBy: TNDanh (4/9/20222)
        //[HttpGet("{userID}")]
        //public async Task<ActionResult<UserDTO>> GetUserByID(Guid userID)
        //{
        //    try
        //    {
        //        var user = await _usersRepository.GetUserByID(userID);
        //        return Ok(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //        throw;
        //    }
        //}

        /// <summary>
        /// Xóa user qua ID
        /// </summary>
        /// <param name="userID">ID của user</param>
        /// <returns>Số lượng user bị xóa</returns>
        /// CreatedBY: TNDanh (6/9/2022)
        [HttpDelete("{userID}")]
        public async Task<ActionResult<int>> DeleteUserByID(Guid userID)
        {
            try
            {
                var user = await _usersRepository.DeleteUserByID(userID);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Tìm kiếm và phân trang user
        /// </summary>
        /// <param name="pageSize">Số lượng bản ghi</param>
        /// <param name="pageNumber">Chỉ số trang</param>
        /// <param name="searchWord">Từ khóa: Họ và tên, email, số điện thoại</param>
        /// <param name="userGroupName">Tên nhóm người dùng</param>
        /// <returns>User phù hợp với điều kiện</returns>
        /// CreatedBy: TNDanh (6/9/2022)
        [HttpGet("Filter")]
        public IActionResult GetFindAndPaging(int pageSize, int pageNumber, string? searchWord, string? userGroupName, string? jobTitle, string? organizationName)
        {
            try
            {
                var users = _usersRepository.GetFindAndPagingUser(pageSize, pageNumber, searchWord, userGroupName, jobTitle, organizationName);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Xuất file excel
        /// </summary>
        /// <returns>Xuất file excel</returns>
        /// CreatedBy: TNDanh (12/9/2022)
        [HttpGet("Export")]
        public async Task<IActionResult> ExportUserExcel()
        {
            List<UserDTO> users = _usersRepository.GetAll().ToList();
            var stream = await _usersRepository.ExportUserExcel(users);
            try
            {
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", CoreResource.GetResoureString("UsersExcelFile"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
            //finally
            //{
            //    stream.Close();
            //    stream.Dispose();
            //}
        }
        /// <summary>
        /// Thêm nhiều người dùng
        /// </summary>
        /// <param name="users">Thông tin của các người dùng</param>
        /// <returns>Số lượng người được thêm</returns>
        /// CreatedBy: TNDanh (19/9/2022)
        [HttpPost("AddUsers")]
        public async Task<IActionResult> AddUsers(List<UserDTO> users)
        {
            try
            {
                var res = await _usersRepository.AddUsers(users);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Xuất file excel về người dùng không hợp lệ
        /// </summary>
        /// <param name="invalidUsers">Danh sách thông tin người dùng không hợp lệ</param>
        /// <returns>File chứa thông tin người dùng không hợp lệ</returns>
        /// CreatedBy: TNDanh (21/9/2022)
        [HttpPost("ExportInvalidUsers")]
        public async Task<IActionResult> ExportInvalidUsers(List<InvalidUser> invalidUsers)
        {
            var stream = await _usersRepository.ExportInvalidUsers(invalidUsers);
            try
            {

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", CoreResource.GetResoureString("InvalidUsersExcelFile"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Nhập dữ liệu excel vào db
        /// </summary>
        /// <returns>Số lượng người dùng được thêm</returns>
        /// CreateBy: TNDanh (16/9/2022)
        [HttpPost("Import")]
        public async Task<IActionResult> ImportExcelToSql(IFormFile file)
        {
            try
            {
                var users = await _usersRepository.ImportExcelToSql(file);
                var res = _userSerivce.InsertAsync((List<UserDTO>)users);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Xuất file excel mẫu dùng để nhập khẩu
        /// </summary>
        /// <returns>File mẫu dùng để xuất khẩu</returns>
        /// CreatedBy: TNDanh (17/9/2022)
        [HttpGet("ExportDefault")]
        public async Task<IActionResult> ExportTemplateExcel()
        {
            var stream = await _usersRepository.ExportDefaultExcel();
            try
            {
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", CoreResource.GetResoureString("UsersExcelFileTemplate"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
            //finally
            //{
            //    stream.Close();
            //    stream.Dispose();
            //}
        }
        /// <summary>
        /// Thêm / sửa nhóm người dùng của người dùng
        /// </summary>
        /// <param name="userGroups">Danh sách nhóm người dùng</param>
        /// <param name="userID">ID của người dùng</param>
        /// <returns>Số lượng danh sách được thêm/sửa</returns>
        /// CreatedBy: TNDanh (9/9/2022)
        [HttpPost("EditUserGroupOfUser/{userID}")]
        public async Task<ActionResult<int>> EditUserGroupByUserID(List<UserGroup> userGroups, Guid userID)
        {
            try
            {
                var res = await _usersRepository.EditUserGroupByUserID(userGroups, userID);
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
