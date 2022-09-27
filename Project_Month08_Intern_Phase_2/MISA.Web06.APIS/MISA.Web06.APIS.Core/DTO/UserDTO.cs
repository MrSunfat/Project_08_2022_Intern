using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web06.APIS.Core.DTO
{
    public class UserDTO
    {
        #region Properties
        /// <summary>
        /// ID của người dùng
        /// </summary>
        [Key]
        public Guid UserID { get; set; }
        /// <summary>
        /// Tên đầy đủ của người dùng
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Họ và tên đệm của người dùng
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Tên chính của người dùng
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Email người dùng
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Số điện thoại của người dùng
        /// </summary>
        public string? Mobile { get; set; }
        /// <summary>
        /// Số điện thoại của đơn vị
        /// </summary>
        public string? WorkPhone { get; set; }
        /// <summary>
        /// ID chức vụ
        /// </summary>
        public int JobTitleID { get; set; }
        /// <summary>
        /// Tên chức vụ
        /// </summary>
        public string? JobTitleName { get; set; }
        /// <summary>
        /// ID của phòng ban
        /// </summary>
        public int? OrganizationUnitID { get; set; }
        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string? OrganizationUnitName { get; set; }
        /// <summary>
        /// ID của đơn vị
        /// </summary>
        public int OrganizationID { get; set; }
        /// <summary>
        /// Tên của đơn vị
        /// </summary>
        public string? OrganizationName { get; set; }
        /// <summary>
        /// Danh sách ID của nhóm người dùng
        /// </summary>
        public string? UserGroupIDs { get; set; }
        /// <summary>
        /// Danh sách các tên nhóm người dùng
        /// </summary>
        public string? UserGroupName { get; set; }
        /// <summary>
        /// Trạng thái tài khoản người dùng
        /// </summary>
        public int Status { get; set; }
        #endregion
    }
}
