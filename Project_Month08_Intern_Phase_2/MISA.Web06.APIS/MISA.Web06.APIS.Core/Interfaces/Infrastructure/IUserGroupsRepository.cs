using MISA.Web06.APIS.Core.DTO;
using MISA.Web06.APIS.Core.Entities;

namespace MISA.Web06.APIS.Core.Interfaces.Infrastructure
{
    public interface IUserGroupsRepository : IBaseRespository<UserGroup>
    {
        #region Properties
        /// <summary>
        /// Lấy ra thông tin của nhóm người dùng qua 
        /// </summary>
        /// <returns>Lấy ra thông tin của các nhóm người dùng</returns>
        /// <param name="userGroupID">ID của nhóm người dùng</param>
        /// CreatedBy: TNDanh (5/9/2022)
        /// </summary>
        public UserGroupDTO GetMemberInGroupByID(int userGroupID);
        /// <summary>
        /// Tìm kiếm và phân trang userGroup
        /// </summary>
        /// <param name="pageSize">Số lượng bản ghi</param>
        /// <param name="pageNumber">Chỉ số trang</param>
        /// <param name="searchWord">Từ khóa: Tên nhóm người dùng</param>
        /// <returns>UserGroup phù hợp với điều kiện</returns>
        /// CreatedBy: TNDanh (8/9/2022)
        //public object GetFindAndPagingUserGroup(int pageSize, int pageNumber, string? searchWord);
        /// <summary>
        /// Tìm kiếm thành viên bên trong nhóm người dùng
        /// </summary>
        /// <param name="userGroupID">ID của nhóm người dùng</param>
        /// <param name="searchWord">Từ khóa: Tên thành viên | Email của thành viên | Mô tả của thành viên</param>
        /// <returns>Thông tin của nhóm người dùng phù hợp</returns>
        /// CreatedBy: TNDanh (10/9/2022)
        public UserGroupDTO GetFindMemberInGroupByID(int userGroupID, string? searchWord);
        /// <summary>
        /// Thêm thành viên vào userGroup
        /// </summary>
        /// <param name="members">Thông tin các thành viên</param>
        /// <returns>Số lượng thành viên được thêm</returns>
        /// CreatedBy: TNDanh (10/9/2022)
        public Task<int> AddMemberInUserGroup(Guid[] memberIDs, int userGroupID);
        /// <summary>
        /// Xóa thành viên theo id
        /// </summary>
        /// <param name="MemberID">ID của thành viên</param>
        /// <param name="UserGroupID">ID của nhóm người dùng</param>
        /// <returns>Số lượng thành viên bị xóa</returns>
        /// CreatedBy: TNDanh (11/9/2022)
        public Task<int> DeleteMemberInGroup(Guid MemberID, int UserGroupID);
        /// <summary>
        /// Xóa nhiều member trong nhóm người dùng
        /// </summary>
        /// <param name="MemberIDs">ID của các thành viên</param>
        /// <param name="UserGroupID">ID của nhóm người dùng</param>
        /// <returns>Xóa nhiều member trong nhóm người dùng</returns>
        /// CreatedBy: TNDanh (6/9/2022)
        public Task<int> DeleteMembersInGroup(Guid[] MemberIDs, int UserGroupID);
        #endregion
    }
}
