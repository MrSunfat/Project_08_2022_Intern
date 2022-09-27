using Dapper;
using MISA.Web06.APIS.Core.DTO;
using MISA.Web06.APIS.Core.Entities;
using MISA.Web06.APIS.Core.Interfaces.Infrastructure;
using MySqlConnector;
using Microsoft.Extensions.Configuration;


namespace MISA.Web06.APIS.Infrastructure.Repository
{
    public class UserGroupsRepository : BaseRepository<UserGroup>, IUserGroupsRepository
    {
        #region Properties

        #endregion

        #region Constructor
        public UserGroupsRepository(IConfiguration configuration) : base(configuration)
        {
            tableProcedure = typeof(UserGroup).Name;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy ra thông tin của các nhóm người dùng
        /// </summary>
        /// <returns>Lấy ra thông tin của các nhóm người dùng</returns>
        /// CreatedBy: TNDanh (5/9/2022)
        public UserGroupDTO GetMemberInGroupByID(int userGroupID)
        {
            var sqlCommand = $"Proc_GetMembersInGroupBy{tableProcedure}ID";
            var parameters = new DynamicParameters();
            parameters.Add("v_UserGroupID", userGroupID);
            using (_mySqlConnection = new MySqlConnection(connectString))
            {
                var userGroupDictionary = new Dictionary<int, UserGroupDTO>();

                var userGroup = _mySqlConnection.Query<UserGroupDTO, MemberDTO, UserGroupDTO>(sqlCommand,
                    (userGroup, memberDTO) =>
                    {
                        UserGroupDTO userGroupEntry;

                        if (!userGroupDictionary.TryGetValue(userGroup.UserGroupID, out userGroupEntry))
                        {
                            userGroupEntry = userGroup;
                            userGroupEntry.Members = new List<MemberDTO>();
                            userGroupDictionary.Add(userGroupEntry.UserGroupID, userGroupEntry);
                        }

                        // kiểm tra môn học khác null thì thêm vào ListSubject và kiểm tra subject đã có chưa
                        if (userGroup != null && !userGroupEntry.Members.Any(u => u.MemberID == memberDTO.MemberID))
                        {
                            userGroupEntry.Members.Add(memberDTO);
                        }
                        return userGroupEntry;
                    },
                    splitOn: "MemberID",
                    param: parameters,
                    commandType: System.Data.CommandType.StoredProcedure)
                    .Distinct()
                    .ToList();

                userGroup[0].MemberCount = userGroup[0].Members.Count();

                userGroupDictionary.Clear();

                return userGroup[0];
            }
        }
        /// <summary>
        /// Lấy tất cả thông tin của nhóm người dùng
        /// </summary>
        /// <returns>Lấy tất cả thông tin của nhóm người dùng</returns>
        /// CreatedBY: TNDanh (8/9/2022)
        //public override IEnumerable<UserGroup> GetAll()
        //{
        //    var sqlCommand = $"Proc_GetAll{tableProcedure}";
        //    using (_mySqlConnection = new MySqlConnection(connectString))
        //    {
        //        var userGroups = _mySqlConnection.Query<UserGroup>(sqlCommand, commandType: System.Data.CommandType.StoredProcedure);
        //        return userGroups;
        //    }
        //}
        /// <summary>
        /// Tìm kiếm và phân trang userGroup
        /// </summary>
        /// <param name="pageSize">Số lượng bản ghi</param>
        /// <param name="pageNumber">Chỉ số trang</param>
        /// <param name="searchWord">Từ khóa: Tên nhóm người dùng</param>
        /// <returns>UserGroup phù hợp với điều kiện</returns>
        /// CreatedBy: TNDanh (8/9/2022)
        //public object GetFindAndPagingUserGroup(int pageSize, int pageNumber, string? searchWord)
        //{
        //    var sqlCommand = $"Proc_GetFindAndPaging{tableProcedure}";
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@v_PageNumber", pageNumber);
        //    parameters.Add("@v_PageSize", pageSize);
        //    parameters.Add("@v_SearchWord", searchWord);
        //    parameters.Add("@v_TotalPage", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
        //    parameters.Add("@v_TotalRecord", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
        //    parameters.Add("@v_UserGroupStart", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
        //    parameters.Add("@v_UserGroupEnd", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

        //    using (_mySqlConnection = new MySqlConnection(connectString))
        //    {
        //        var userGroups = _mySqlConnection.Query<UserGroup>(sqlCommand, param: parameters, commandType: System.Data.CommandType.StoredProcedure);

        //        var res = new
        //        {
        //            TotalRecord = parameters.Get<int>("@v_TotalRecord"),
        //            TotalPage = parameters.Get<int>("@v_TotalPage"),
        //            UserGroupStart = parameters.Get<int>("@v_UserGroupStart"),
        //            UserGroupEnd = parameters.Get<int>("@v_UserGroupEnd"),
        //            CurrentPage = pageNumber,
        //            CurrentPageRecords = pageSize,
        //            Data = userGroups
        //        };

        //        return res;
        //    }
        //}
        /// <summary>
        /// Thêm thành viên vào userGroup
        /// </summary>
        /// <param name="members">Thông tin các thành viên</param>
        /// <returns>Số lượng thành viên được thêm</returns>
        /// CreatedBy: TNDanh (10/9/2022)
        public async Task<int> AddMemberInUserGroup(Guid[] memberIDs, int userGroupID)
        {
            var sqlCommandAddMember = "Proc_AddMemberInUserGroup";
            var sqlCommandDeleteAllMember = "Proc_DeleteAllMemberInUserGroup";  
            using (_mySqlConnection = new MySqlConnection(connectString))
            {
                _mySqlConnection.Open();
                using(MySqlTransaction transaction = _mySqlConnection.BeginTransaction())
                {
                    try
                    {
                        int totalMember = 0;    
                        var parameters = new DynamicParameters();
                        parameters.Add("@v_UserGroupID", userGroupID);
                        // 1. Xóa toàn bộ thành viên đi
                        await _mySqlConnection.ExecuteAsync(sqlCommandDeleteAllMember, param: parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);
                        // 2.Thực hiện thêm nhiều thành viên
                        foreach (var member in memberIDs)
                        {
                                parameters.Add("@v_UserID", member);
                                var res = await _mySqlConnection.ExecuteAsync(sqlCommandAddMember, param: parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);
                                if (res > 0)
                                {
                                    totalMember++;
                                }
                        }
                        
                        transaction.Commit();
                        return totalMember;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>
        /// Xóa nhiều member trong nhóm người dùng
        /// </summary>
        /// <param name="MemberIDs">ID của các thành viên</param>
        /// <param name="UserGroupID">ID của nhóm người dùng</param>
        /// <returns>Xóa nhiều member trong nhóm người dùng</returns>
        /// CreatedBy: TNDanh (6/9/2022)
        public async Task<int> DeleteMembersInGroup(Guid[] MemberIDs,int UserGroupID)
        {
            var sqlCommand = "Proc_DeleteMemberInUserGroup";
            using (_mySqlConnection = new MySqlConnection(connectString)) 
            {
                _mySqlConnection.Open();
                using(MySqlTransaction trans = _mySqlConnection.BeginTransaction())
                {
                    try
                    {
                        int totalMember = 0;
                        var parameters = new DynamicParameters();
                        parameters.Add("@v_UserGroupID", UserGroupID);
                        foreach (var memberID in MemberIDs)
                        {
                            parameters.Add("@v_UserID", memberID);
                            var members = await _mySqlConnection.ExecuteAsync(sqlCommand, parameters, trans, commandType: System.Data.CommandType.StoredProcedure);
                            if (members > 0)
                            {
                                totalMember++;
                            }
                        }
                        trans.Commit();
                        return totalMember;
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>
        /// Tìm kiếm thành viên bên trong nhóm người dùng
        /// </summary>
        /// <param name="userGroupID">ID của nhóm người dùng</param>
        /// <param name="searchWord">Từ khóa: Tên thành viên | Email của thành viên | Mô tả của thành viên</param>
        /// <returns>Thông tin của nhóm người dùng phù hợp</returns>
        /// CreatedBy: TNDanh (10/9/2022)
        public UserGroupDTO GetFindMemberInGroupByID(int userGroupID, string? searchWord)
        {
            var sqlCommand = "Proc_GetFindMemberInUserGroup";
            using(_mySqlConnection = new MySqlConnection(connectString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@v_SearchWord", searchWord);
                parameters.Add("@v_UserGroupID", userGroupID);
                using (_mySqlConnection = new MySqlConnection(connectString))
                {
                    var userGroupDictionary = new Dictionary<int, UserGroupDTO>();

                    var userGroup = _mySqlConnection.Query<UserGroupDTO, MemberDTO, UserGroupDTO>(sqlCommand,
                        (userGroup, memberDTO) =>
                        {
                            UserGroupDTO userGroupEntry;

                            if (!userGroupDictionary.TryGetValue(userGroup.UserGroupID, out userGroupEntry))
                            {
                                userGroupEntry = userGroup;
                                userGroupEntry.Members = new List<MemberDTO>();
                                userGroupDictionary.Add(userGroupEntry.UserGroupID, userGroupEntry);
                            }

                        // kiểm tra môn học khác null thì thêm vào ListSubject và kiểm tra subject đã có chưa
                            if (userGroup != null && !userGroupEntry.Members.Any(u => u.MemberID == memberDTO.MemberID))
                            {
                                userGroupEntry.Members.Add(memberDTO);
                            }
                            return userGroupEntry;
                        },
                        splitOn: "UserGroupID, MemberID",
                        param: parameters,
                        commandType: System.Data.CommandType.StoredProcedure)
                        .Distinct()
                        .ToList();

                    if (userGroup != null && userGroup.Count() > 0) { 
                        userGroup[0].MemberCount = userGroup[0].Members.Count();
                        return userGroup[0];
                    }

                    userGroupDictionary.Clear();
                    return null;
                }

            }
        }
        /// <summary>
        /// Xóa thành viên theo id
        /// </summary>
        /// <param name="MemberID">ID của thành viên</param>
        /// <param name="UserGroupID">ID của nhóm người dùng</param>
        /// <returns>Số lượng thành viên bị xóa</returns>
        /// CreatedBy: TNDanh (11/9/2022)
        public async Task<int> DeleteMemberInGroup(Guid MemberID, int UserGroupID)
        {
            var sqlCommand = "Proc_DeleteMemberInUserGroup";
            using (_mySqlConnection = new MySqlConnection(connectString))
            {
                _mySqlConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@v_UserGroupID", UserGroupID);
                parameters.Add("@v_UserID", MemberID);
                var member = await _mySqlConnection.ExecuteAsync(sqlCommand, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return member;
            }
        }
        #endregion
    }
}
