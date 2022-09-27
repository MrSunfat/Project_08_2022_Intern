using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Web06.APIS.Core.DTO;
using MISA.Web06.APIS.Core.Entities;
using MISA.Web06.APIS.Core.Interfaces.Infrastructure;

namespace MISA.Web06.APIS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserGroupsController : BaseEntitiesController<UserGroup, int>
    {
        #region Properties
        private readonly IUserGroupsRepository _userGroupsRepository;
        #endregion

        #region Constructor
        public UserGroupsController(IUserGroupsRepository userGroupsRepository) : base(userGroupsRepository)
        {
            _userGroupsRepository = userGroupsRepository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy tất cả thông tin của nhóm người dùng
        /// </summary>
        /// <returns>Lấy tất cả thông tin của nhóm người dùng</returns>
        /// CreatedBY: TNDanh (8/9/2022)
        [HttpGet]
        public override IActionResult GetAll()
        {
            try
            {
                var userGroups = _userGroupsRepository.GetAll();
                return Ok(userGroups);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Lấy thông tin thành viên của userGroup
        /// </summary>
        /// <param name="UserGroupID">ID của userGroup</param>
        /// <returns>Lấy thông tin thành viên của userGroup</returns>
        /// CreatedBy: TNDanh (5/8/2022)
        [HttpGet("{UserGroupID}")]
        public override IActionResult GetByID(int UserGroupID)
        {
            try
            {
                var members = _userGroupsRepository.GetMemberInGroupByID(UserGroupID);
                return Ok(members);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Phân trang và tìm kiếm trong nhóm người dùng
        /// </summary>
        /// <param name="pageSize">Số bản ghi / trang</param>
        /// <param name="pageNumber">Chỉ số trang</param>
        /// <param name="searchWord">Từ tìm kiếm</param>
        /// <returns>Nhóm người dùng phù hợp</returns>
        /// CreatedBy: TNDanh (10/9/2022)
        [HttpGet("Filter")]
        public override IActionResult GetFindAndPaging(int pageSize, int pageNumber, string? searchWord)
        {
            try
            {
                var userGroups = _userGroupsRepository.GetFindAndPaging(pageSize, pageNumber, searchWord);
                return Ok(userGroups);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Tìm kiếm thành viên bên trong nhóm người dùng
        /// </summary>
        /// <param name="UserGroupID">ID của nhóm người dùng</param>
        /// <param name="searchWord">Từ khóa: Tên thành viên | Email của thành viên | Mô tả của thành viên</param>
        /// <returns>Thông tin của nhóm người dùng phù hợp</returns>
        /// CreatedBy: TNDanh (10/9/2022)
        [HttpGet("Filter/{UserGroupID}/Members")]
        public IActionResult GetFindMemberInGroupByID(int UserGroupID, string? searchWord)
        {
            try
            {
                var res = _userGroupsRepository.GetFindMemberInGroupByID(UserGroupID, searchWord);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Thêm thành viên vào trong nhóm
        /// </summary>
        /// <param name="Members">Danh sách thông tin thành viên</param>
        /// <param name="UserGroupID">ID của nhóm người dùng</param>
        /// <returns>Số lượng thành viên được thêm</returns>
        /// CreatedBy: TNDanh (10/9/2022)
        [HttpPost("AddUserGroup/{UserGroupID}/Members")]
        public async Task<ActionResult<int>> AddMemberInUserGroup(Guid[] MemberIDs, int UserGroupID)
        {
            try
            {
        
                var res = await _userGroupsRepository.AddMemberInUserGroup(MemberIDs, UserGroupID);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Xóa nhiều thành viên qua id
        /// </summary>
        /// <param name="MemberIDs">Danh sách id của các thành viên</param>
        /// <param name="UserGroupID">ID của nhóm người dùng</param>
        /// <returns>Số lượng thành viên bị xóa</returns>
        /// CreatedBy: TNDanh (10/9/2022)
        [HttpDelete("RemoveUserGroup/{UserGroupID}/Members")]
        public async Task<ActionResult<int>> DeleteMembersInGroup(Guid[] MemberIDs, int UserGroupID)
        {
            try
            {
                var res = await _userGroupsRepository.DeleteMembersInGroup(MemberIDs, UserGroupID);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Xóa thành viên theo id
        /// </summary>
        /// <param name="MemberID">ID của thành viên</param>
        /// <param name="UserGroupID">ID của nhóm người dùng</param>
        /// <returns>Số lượng thành viên bị xóa</returns>
        /// CreatedBy: TNDanh (11/9/2022)
        [HttpDelete("RemoveUserGroup/{UserGroupID}/Members/{MemberID}")]
        public async Task<ActionResult<int>> DeleteMemberInGroup(Guid MemberID, int UserGroupID)
        {
            try
            {
                var res = await _userGroupsRepository.DeleteMemberInGroup(MemberID, UserGroupID);
                return res;
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
