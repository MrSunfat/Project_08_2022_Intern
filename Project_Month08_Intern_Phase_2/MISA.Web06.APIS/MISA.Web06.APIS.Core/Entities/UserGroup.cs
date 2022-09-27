using MISA.Web06.APIS.Core.DTO;
using System.ComponentModel.DataAnnotations;

namespace MISA.Web06.APIS.Core.Entities
{
    public class UserGroup : BaseEntities
    {
        #region Properties
        /// <summary>
        /// ID của nhóm người dùng
        /// </summary>
        [Key]
        public int UserGroupID { get; set; }
        /// <summary>
        /// Tên nhóm người dùng
        /// </summary>
        public string UserGroupName { get; set; }
        /// <summary>
        /// Mô tả về nhóm người dùng
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Vị trí ưu tiên trong bảng
        /// </summary>
        public int SortOrder { get; set; }
        /// <summary>
        /// Số lượng thành viên
        /// </summary>
        public int MemberCount { get; set; }    
        /// <summary>
        /// Trạng thái sử dụng của nhóm người dùng
        /// </summary>
        public int Status { get; set; }
        #endregion
    }
}
