using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web06.APIS.Core.DTO
{
    public class MemberDTO 
    {
        #region Properties
        /// <summary>
        /// ID của thành viên
        /// </summary>
        public Guid MemberID { get; set; }
        /// <summary>
        /// Tên thành viên
        /// </summary>
        public string MemberName { get; set; }
        /// <summary>
        /// Email thành viên
        /// </summary>
        public string Email { get; set; }   
        /// <summary>
        /// Mô tả về chức vụ, cơ quan của thành viên
        /// </summary>
        public string MemberDescription { get; set; }
        #endregion
    }
}
