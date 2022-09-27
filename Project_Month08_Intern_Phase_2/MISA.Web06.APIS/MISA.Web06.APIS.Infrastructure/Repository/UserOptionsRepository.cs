using Microsoft.Extensions.Configuration;
using MISA.Web06.APIS.Core.Entities;
using MISA.Web06.APIS.Core.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MISA.Web06.APIS.Infrastructure.Repository
{
    public class UserOptionsRepository : BaseRepository<Column>, IUserOptionsRepository
    {
        #region Properties

        #endregion

        #region Constructor
        public UserOptionsRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion

        #region Methods
        /// <summary>
        /// Lấy ra bộ tùy chỉnh cột
        /// </summary>
        /// <returns>Bộ tùy chỉnh cột</returns>
        /// Author: TNDanh (19/9/2022)
        public override IEnumerable<Column> GetAll()
        {
            List<Column> columns = new List<Column>();
            //C:\Users\hue\Desktop\Project_APIS_MISA_08\Project_Web06_Intern_Phase_2\MISA.Web06.APIS\MISA.Web06.APIS.Infrastructure\ColumnOptionFolder\default.json
            using (StreamReader reader = new StreamReader("../MISA.Web06.APIS.Infrastructure/ColumnOptionFolder/customize.json"))
            {
                string json = reader.ReadToEnd();
                columns = JsonSerializer.Deserialize<List<Column>>(json);
            }
            return columns;
        }

        /// <summary>
        /// Xét các thuộc tính của tùy chỉnh cột
        /// </summary>
        /// <param name="columns">Danh sách thông tin tùy chỉnh cột</param>
        /// <returns>Số lượng cột</returns>
        /// Author: TNDanh (19/9/2022)
        public async Task<int> SetUserOptions(List<Column> columns)
        {
            List<Column> data = columns;
            string json = JsonSerializer.Serialize(data);
            await File.WriteAllTextAsync(@"../MISA.Web06.APIS.Infrastructure/ColumnOptionFolder/customize.json", json);
            return columns.Count();
        }
        #endregion
    }
}
