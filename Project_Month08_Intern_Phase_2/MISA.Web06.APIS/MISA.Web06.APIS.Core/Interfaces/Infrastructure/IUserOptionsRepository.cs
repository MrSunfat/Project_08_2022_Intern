using MISA.Web06.APIS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web06.APIS.Core.Interfaces.Infrastructure
{
    public interface IUserOptionsRepository : IBaseRespository<Column>
    {
        /// <summary>
        /// Xét các thuộc tính của tùy chỉnh cột
        /// </summary>
        /// <param name="columns">Danh sách thông tin tùy chỉnh cột</param>
        /// <returns>Số lượng cột</returns>
        /// Author: TNDanh (19/9/2022)
        public Task<int> SetUserOptions(List<Column> columns);
    }
}
