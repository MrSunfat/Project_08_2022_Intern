using MISA.Web06.APIS.Core.DTO;

namespace MISA.Web06.APIS.Core.Entities
{
    public class InvalidUser : UserDTO
    {
        /// <summary>
        /// Các lỗi của người dùng
        /// </summary>
        public string? ErrorUser { get; set; }   
    }
}
