using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web06.APIS.Core.Interfaces.Infrastructure
{
    public interface IBaseRespository<MISAEntity>
    {
        #region Methods
        /// <summary>
        /// Lấy thông tin tất cả đối tượng
        /// </summary>
        /// <returns>Thông tin tất cả đối tượng</returns>
        /// CreatedBy: TNDanh (4/9/2022)
        public IEnumerable<MISAEntity> GetAll();

        /// <summary>
        /// Lấy ra thông tin của đối tượng qua id
        /// </summary>
        /// <param name="entityID">ID của đối tượng</param>
        /// <returns>Thông tin của đối tượng qua id</returns>
        /// CreatedBy: TNDanh (24/9/2022)
        public MISAEntity GetByID<EntityID>(EntityID entityID);

        /// <summary>
        /// Tìm kiếm và phân trang cho đối tượng
        /// </summary>
        /// <param name="pageSize">Số lượng đối tượng</param>
        /// <param name="pageNumber">Chỉ số trang</param>
        /// <param name="searchWord">Từ khóa tìm kiếm</param>
        /// <returns>CreatedBy: TNDanh(24/9/2022)</returns>
        public object GetFindAndPaging(int pageSize, int pageNumber, string? searchWord);
        #endregion
    }
}
