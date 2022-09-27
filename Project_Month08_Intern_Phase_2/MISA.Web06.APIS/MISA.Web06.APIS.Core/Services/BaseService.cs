using MISA.Web06.APIS.Core.DTO;
using MISA.Web06.APIS.Core.Interfaces.Infrastructure;
using MISA.Web06.APIS.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.Web06.APIS.Core.Services
{
    public class BaseService<MISAEntity> : IBaseService<MISAEntity>
    {
        #region Properties
        protected Regex validatePhoneNumberRegex = new Regex("^\\+?[1-9][0-9]{7,14}$");
        protected Regex validateEmailRegex = new Regex("(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])");
        #endregion

        #region Constructor
        public BaseService()
        {
        }

        #endregion

        #region Methods
        /// <summary>
        /// Validate các trường bắt buộc và các trường mặc định
        /// </summary>
        /// <param name="entity">Nội dung đối tượng</param>
        /// <returns>Trả về thông tin các trường lỗi của đối tượng</returns>
        /// CreatdBy: TNDanh (26/9/2022)
        public virtual object InsertAsync(List<UserDTO> entities)
        {
            
            throw new NotImplementedException();
        }
        #endregion
    }
}
