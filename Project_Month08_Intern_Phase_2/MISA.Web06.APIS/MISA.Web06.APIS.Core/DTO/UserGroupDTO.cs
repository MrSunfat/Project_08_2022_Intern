using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web06.APIS.Core.DTO
{
    public class UserGroupDTO
    {
        #region Properties
        /// <summary>
        /// ID của nhóm người dùng
        /// </summary>
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
        /// Danh sách chứa thông tin của các thành viên
        /// </summary>
        public List<MemberDTO> Members { get; set; }
        /// <summary>
        /// Số lượng thành viên
        /// </summary>
        public int MemberCount { get; set; }
        /// <summary>
        /// Trạng thái sử dụng của nhóm người dùng
        /// </summary>
        public int Status { get; set; } = 1;
        #endregion
    }
}
