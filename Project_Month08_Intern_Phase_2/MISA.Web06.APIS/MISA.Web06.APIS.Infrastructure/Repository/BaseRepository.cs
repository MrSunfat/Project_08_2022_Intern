using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Web06.APIS.Core.Entities;
using MISA.Web06.APIS.Core.Interfaces.Infrastructure;
using MySqlConnector;

namespace MISA.Web06.APIS.Infrastructure.Repository
{
    public class BaseRepository<MISAEntity> : IDisposable , IBaseRespository<MISAEntity>
    {
        #region Properties
        protected readonly string connectString = "";
        protected MySqlConnection _mySqlConnection;
        protected string tableProcedure = "";
        #endregion

        #region Constructor
        public BaseRepository(IConfiguration configuration)
        {
            tableProcedure = typeof(MISAEntity).Name;
            connectString = configuration.GetConnectionString("MISAConnectString");
            _mySqlConnection = new MySqlConnection(connectString);
            _mySqlConnection.Open();
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            _mySqlConnection.Dispose();
        }
        //public void Dispose()
        //{
        //    _mySqlConnection.Close();
        //    _mySqlConnection.Dispose();
        //}

        /// <summary>
        /// Lấy tất cả các đối tượng
        /// </summary>
        /// <returns>Lấy tất cả các đối tượng</returns>
        /// CreatedBy: TNDanh (9/9/2022)
        public virtual IEnumerable<MISAEntity> GetAll()
        {
            var sqlCommand = $"Proc_GetAll{tableProcedure}";
            using (_mySqlConnection = new MySqlConnection(connectString))
            {
                var entities = _mySqlConnection.Query<MISAEntity>(sqlCommand, commandType: System.Data.CommandType.StoredProcedure);
                return entities.ToList();
            }
        }

        /// <summary>
        /// Lấy ra thông tin của đối tượng qua id
        /// </summary>
        /// <param name="entityID">ID của đối tượng</param>
        /// <returns>Thông tin của đối tượng qua id</returns>
        /// CreatedBy: TNDanh (24/9/2022)
        public virtual MISAEntity GetByID<EntityID>(EntityID entityID)
        {
            var sqlCommand = $"Proc_Get{tableProcedure}ByID";
            var parameters = new DynamicParameters();
            parameters.Add("@v_UserID", entityID);
            using (_mySqlConnection = new MySqlConnection(connectString))
            {
                MISAEntity entity = _mySqlConnection.QueryFirstOrDefault<MISAEntity>(sqlCommand, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
                return entity;
            }
        }

        /// <summary>
        /// Tìm kiếm và phân trang cho đối tượng
        /// </summary>
        /// <param name="pageSize">Số lượng đối tượng</param>
        /// <param name="pageNumber">Chỉ số trang</param>
        /// <param name="searchWord">Từ khóa tìm kiếm</param>
        /// <returns>CreatedBy: TNDanh(24/9/2022)</returns>
        public virtual object GetFindAndPaging(int pageSize, int pageNumber, string? searchWord)
        {
            var sqlCommand = $"Proc_GetFindAndPaging{tableProcedure}";
            var parameters = new DynamicParameters();
            parameters.Add("@v_PageSize", pageSize);
            parameters.Add("@v_PageNumber", pageNumber);
            parameters.Add("@v_SearchWord", searchWord);
            parameters.Add("@v_TotalRecord", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            parameters.Add("@v_TotalPage", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            parameters.Add($"@v_{tableProcedure}Start", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            parameters.Add($"@v_{tableProcedure}End", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            using (_mySqlConnection = new MySqlConnection(connectString))
            {
                var users = _mySqlConnection.Query<MISAEntity>(sqlCommand, param: parameters, commandType: System.Data.CommandType.StoredProcedure);

                var res = new
                {
                    TotalRecord = parameters.Get<int>("@v_TotalRecord"),
                    TotalPage = parameters.Get<int>("@v_TotalPage"),
                    UserStart = parameters.Get<int>($"@v_{tableProcedure}Start"),
                    UserEnd = parameters.Get<int>($"@v_{tableProcedure}End"),
                    CurrentPage = pageNumber,
                    CurrentPageRecords = pageSize,
                    Data = users
                };

                return res;
            }
        }
        #endregion
    }
}
