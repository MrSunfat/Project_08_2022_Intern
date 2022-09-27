using Microsoft.AspNetCore.Http;
using MISA.Web06.APIS.Core.DTO;
using MISA.Web06.APIS.Core.Entities;

namespace MISA.Web06.APIS.Core.Interfaces.Infrastructure
{
    public interface IUsersRepository : IBaseRespository<UserDTO>
    {
        #region Methods
        /// <summary>
        /// Lấy thông tin người dùng qua userID
        /// </summary>
        /// <param name="userID">ID của người dùng</param>
        /// <returns>Thông tin của người dùng qua userID</returns>
        /// CreatedBy: TNDanh (4/9/2022)
        //public Task<UserDTO> GetUserByID(Guid userID);
        /// <summary>
        /// Xóa user qua ID
        /// </summary>
        /// <param name="userID">ID của user</param>
        /// <returns>Số lượng user bị xóa</returns>
        /// CreatedBy: TNDanh (6/9/2022)
        public Task<int> DeleteUserByID(Guid userID);
        /// <summary>
        /// Thêm nhiều người dùng mới 
        /// </summary>
        /// <param name="user">Thông tin người dùng mới</param>
        /// <returns>Số lượng người thêm mới</returns>
        /// CreatedBy: TNDanh (4/9/2022)
        public Task<int> AddUsers(List<UserDTO> users);
        /// <summary>
        /// Sửa thông tin người dùng bằng userID
        /// </summary>
        /// <param name="user">Thông tin mới của người dùng</param>user
        /// <param name="userID">ID của người dùng</param>user
        /// <returns>Số lượng người dùng sửa đôi</returns>
        /// CreatedBy: TNDanh (4/9/2022)
        public Task<int> EditUserGroupByUserID(List<UserGroup> userGroups, Guid userID);
        /// <summary>
        /// Tìm kiếm và phân trang user
        /// </summary>
        /// <param name="pageSize">Số lượng bản ghi</param>
        /// <param name="pageNumber">Chỉ số trang</param>
        /// <param name="searchWord">Từ khóa: Họ và tên, email, số điện thoại</param>
        /// <param name="userGroupName">Tên nhóm người dùng</param>
        /// <returns>User phù hợp với điều kiện</returns>
        /// CreatedBy: TNDanh (6/9/2022)
        public object GetFindAndPagingUser(int pageSize, int pageNumber, string? searchWord, string? userGroupName, string? jobTitle, string? organizationName);
        /// <summary>
        /// Trả về stream, log ra file excel
        /// </summary>
        /// <param name="users">Thông tin người dùng</param>
        /// <returns>stream chứa thông tin người dùng</returns>
        /// Author: TNDanh (15/9/2022)
        public Task<MemoryStream> ExportUserExcel(List<UserDTO> users);
        /// <summary>
        /// Trả về file excel mẫu cho người dùng nhập
        /// </summary>
        /// <returns>stream chứa thông tin file mẫu</returns>
        /// CreatedBy: TNDanh (17/9/2022)
        public Task<MemoryStream> ExportDefaultExcel();
        /// <summary>
        /// Trả về stream, để xuất ra file người dùng không hợp lệ
        /// </summary>
        /// <param name="invalidUsers">Danh sách thông tin của người dùng không hợp lệ</param>
        /// <returns>Stream chứa thông tin người dùng không hợp lệ</returns>
        /// CreatedBy: TNDanh (21/9/2022)
        public Task<MemoryStream> ExportInvalidUsers(List<InvalidUser> invalidUsers);
        /// <summary>
        /// Nhận file excel rồi render ra danh sách thông tin người dùng
        /// </summary>
        /// <returns>Danh sách người dùng từ file excel</returns>
        /// CreatedBy: TNDanh (21/9/2022)
        public Task<object> ImportExcelToSql(IFormFile file);
        #endregion
    }
}
