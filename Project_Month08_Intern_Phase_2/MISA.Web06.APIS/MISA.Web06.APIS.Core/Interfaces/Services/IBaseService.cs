using MISA.Web06.APIS.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web06.APIS.Core.Interfaces.Services
{
    public interface IBaseService<MISAEntity>
    {

        #region Methods
        /// <summary>
        /// Validate các trường bắt buộc và các trường mặc định
        /// </summary>
        /// <param name="entity">Nội dung đối tượng</param>
        /// <returns>Trả về thông tin của trường</returns>
        /// CreatdBy: TNDanh (26/9/2022)
        public object InsertAsync(List<UserDTO> entities);
        #endregion
    }
}
