using Dapper;
using MISA.Web06.APIS.Core.DTO;
using MISA.Web06.APIS.Core.Entities;
using MISA.Web06.APIS.Core.Interfaces.Infrastructure;
using MySqlConnector;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Drawing;
using OfficeOpenXml.Style;
using MISA.Web06.APIS.Core.Resources;
using MISA.Web06.APIS.Core.Interfaces.Enum;
using Microsoft.AspNetCore.Http;
using MISA.Web06.APIS.Core.Interfaces.Services;
using Newtonsoft.Json;

namespace MISA.Web06.APIS.Infrastructure.Repository
{
    public class UsersRepository : BaseRepository<UserDTO>, IUsersRepository
    {
        #region Properties
        private readonly IUserGroupsRepository _userGroupsRepository;
        private readonly IJobTitlesRepository _jobTitlesRepository;
        private readonly IUserService _userService;
        #endregion

        #region Constructor
        public UsersRepository(IConfiguration configuration, IUserGroupsRepository userGroupsRepository, IJobTitlesRepository jobTitlesRepository, IUserService userService) : base(configuration)
        {
            _userGroupsRepository = userGroupsRepository;
            _jobTitlesRepository = jobTitlesRepository;
            _userService = userService;
            tableProcedure = typeof(User).Name;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy thông tin của người dùng
        /// </summary>
        /// <returns>Danh sách thông tin người dùng</returns>
        /// CrearedBy: TNDanh (4/9/2022)
        //public override IEnumerable<UserDTO> GetAll()
        //{
        //    var sqlCommand = $"Proc_GetAllUser";
        //    using (_mySqlConnection = new MySqlConnection(connectString))
        //    {
        //        var listUser = _mySqlConnection.Query<UserDTO>(sqlCommand, commandType: System.Data.CommandType.StoredProcedure);
        //        return listUser;
        //    }
        //}

        /// <summary>
        ///  Lấy thông tin người dùng qua userID
        /// </summary>
        /// <param name="userID">ID của người dùng</param>
        /// <returns>Thông tin người dùng qua userID</returns>
        /// CreatedBy: TNDanh (4/9/20222)
        //public Task<UserDTO> GetUserByID(Guid userID)
        //{
        //    var sqlCommand = "Proc_GetUserByID";
        //    var pramameters = new DynamicParameters();
        //    pramameters.Add("@v_UserID", userID);
        //    using (_mySqlConnection = new MySqlConnection(connectString))
        //    {
        //        var userDictionary = new Dictionary<Guid, User>();

        //        var user = _mySqlConnection.QueryFirst<UserDTO>(sqlCommand, param: pramameters, commandType: System.Data.CommandType.StoredProcedure);

        //        return Task.FromResult(user);
        //    }
        //}
        /// <summary>
        /// Thêm người dùng mới
        /// </summary>
        /// <param name="user">Thông tin người dùng mới</param>
        /// <returns>Số lượng người thêm mới</returns>
        /// CreatedBy: TNDanh (4/9/2022)
        public async Task<int> AddUsers(List<UserDTO> users)
        {
            var sqlAddUsers = "Proc_AddNewUser";
            using (_mySqlConnection = new MySqlConnection(connectString))
            {
                _mySqlConnection.Open();
                var jsonUsers = JsonConvert.SerializeObject(users);
                using (MySqlTransaction transaction = _mySqlConnection.BeginTransaction())
                {
                    try
                    {
                        var sqlAddListUser = "Proc_AddListUser";
                        var paramJson = new DynamicParameters();
                        paramJson.Add("@v_ListUser", jsonUsers);
                        
                        await _mySqlConnection.ExecuteAsync(sqlAddListUser, param: paramJson, transaction, commandType: System.Data.CommandType.StoredProcedure);
                        foreach (var user in users)
                        {
                            string[] userGroupIDs = user.UserGroupIDs.Split(",");
                            if (userGroupIDs.Count() > 0)
                            {
                                var sqlAddUserGroupDetail = "Proc_AddNewUserGroupDetail";
                                foreach (var userGroup in userGroupIDs)
                                {
                                    var param = new DynamicParameters();
                                    param.Add("@v_UserID", user.UserID);
                                    param.Add("@v_UserGroupID", Convert.ToInt32(userGroup));
                                    await _mySqlConnection.ExecuteAsync(sqlAddUserGroupDetail, param, transaction, commandType: System.Data.CommandType.StoredProcedure);
                                }
                            }
                        }
                        int res = 0;

                        if (users.Count() > 0)
                        {
                            res = users.Count;
                        }
                        transaction.Commit();
                        return res;
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
        /// Sửa thông tin nhóm người dùng của người dùng bằng userID
        /// </summary>
        /// <param name="userGroups">Thông tin nhóm người dùng của người dùng</param>user
        /// <param name="userID">ID của người dùng</param>user
        /// <returns>Số lượng nhóm người dùng sửa đôi</returns>
        /// CreatedBy: TNDanh (4/9/2022)
        public async Task<int> EditUserGroupByUserID(List<UserGroup> userGroups, Guid userID)
        {
            var sqlDeleteUserGroupInUser = "Proc_DeleteAllUserGroupInUser";
            var sqlInsertUserGroupOfUser = "Proc_EditUserGroupInUser";
            var parameters = new DynamicParameters();
            parameters.Add("@v_UserID", userID);

            using (_mySqlConnection = new MySqlConnection(connectString))
            {
                _mySqlConnection.Open();
                using (MySqlTransaction transaction = _mySqlConnection.BeginTransaction())
                {
                    try
                    {
                        // 1. Xóa hết các nhóm người dùng trước
                        var res = await _mySqlConnection.ExecuteAsync(sqlDeleteUserGroupInUser, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);
                        // 2. Thêm các nhóm người dùng
                        var sqlCommandAddUserGroupInUser = "Proc_AddListUserGroupDetail";
                        List<object> detail = new List<object>();
                        foreach (var userGroup in userGroups)
                        {
                            if (userGroup != null)
                            {
                                detail.Add(new
                                {
                                    UserID = userID,
                                    UserGroupID = userGroup.UserGroupID
                                });
                                parameters.Add("@v_UserGroupID", userGroup.UserGroupID);
                                //await _mySqlConnection.ExecuteAsync(sqlInsertUserGroupOfUser, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);
                            }
                        }
                        parameters.Add("@v_ListUserGroupDetail", JsonConvert.SerializeObject(detail));
                        await _mySqlConnection.ExecuteAsync(sqlCommandAddUserGroupInUser, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);

                        transaction.Commit();
                        if (res > 0)
                        {
                            return userGroups.Count();
                        }
                        return res;
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
        /// Xóa user qua ID
        /// </summary>
        /// <param name="userID">ID của user</param>
        /// <returns>Số lượng user bị xóa</returns>
        /// CreatedBY: TNDanh (6/9/2022)
        public async Task<int> DeleteUserByID(Guid userID)
        {
            var sqlCommand = "Proc_DeleteUserByID";
            var parameters = new DynamicParameters();
            parameters.Add("@v_UserID", userID);
            using (_mySqlConnection = new MySqlConnection(connectString))
            {
                var res = await _mySqlConnection.ExecuteAsync(sqlCommand, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
                return res;
            }
        }
        /// <summary>
        /// Tìm kiếm và phân trang user
        /// </summary>
        /// <param name="pageSize">Số lượng bản ghi</param>
        /// <param name="pageNumber">Chỉ số trang</param>
        /// <param name="searchWord">Từ khóa: Họ và tên, email, số điện thoại</param>
        /// <param name="userGroupName">Tên nhóm người dùng</param>
        /// <returns>User phù hợp với điều kiện</returns>
        /// CreatedBy: TNDanh (6/9/2022)
        public object GetFindAndPagingUser(int pageSize, int pageNumber, string? searchWord, string? userGroupName, string? jobTitle, string? organizationName)
        {
            var sqlCommand = $"Proc_GetFindAndPagingUser";
            var param = new DynamicParameters();
            param.Add("@v_PageSize", pageSize);
            param.Add("@v_PageNumber", pageNumber);
            param.Add("@v_SearchWord", searchWord);
            param.Add("@v_TagUserGroup", userGroupName);
            param.Add("@v_TotalRecord", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            param.Add("@v_TotalPage", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            param.Add("@v_UserStart", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            param.Add("@v_UserEnd", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            param.Add("@v_TagJobTitle", jobTitle);
            param.Add("@v_TagOrganization", organizationName);

            using (_mySqlConnection = new MySqlConnection(connectString))
            {
                var users = _mySqlConnection.Query<UserDTO>(sqlCommand, param: param, commandType: System.Data.CommandType.StoredProcedure);

                var res = new
                {
                    TotalRecord = param.Get<int>("@v_TotalRecord"),
                    TotalPage = param.Get<int>("@v_TotalPage"),
                    UserStart = param.Get<int>("@v_UserStart"),
                    UserEnd = param.Get<int>("@v_UserEnd"),
                    CurrentPage = pageNumber,
                    CurrentPageRecords = pageSize,
                    Data = users
                };

                return res;
            }
        }
        /// <summary>
        /// Trả về stream, log ra file excel
        /// </summary>
        /// <param name="users">Thông tin người dùng</param>
        /// <returns>stream chứa thông tin người dùng</returns>
        /// Author: TNDanh (15/9/2022)
        public async Task<MemoryStream> ExportUserExcel(List<UserDTO> users)
        {
            await Task.Yield();
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add(CoreResource.GetResoureString("UsersExcelWorkSheet"));
                var rowStart = 1;
                var rowEnd = users.Count + rowStart;

                var tableIndex = 1;
                workSheet.Cells[rowStart, 1].Value = CoreResource.GetResoureString("Index");
                workSheet.Cells[rowStart, 2].Value = CoreResource.GetResoureString("FullName");
                workSheet.Cells[rowStart, 3].Value = CoreResource.GetResoureString("JobTitle");
                workSheet.Cells[rowStart, 4].Value = CoreResource.GetResoureString("OrganizationUnit");
                workSheet.Cells[rowStart, 5].Value = CoreResource.GetResoureString("Organization");
                workSheet.Cells[rowStart, 6].Value = CoreResource.GetResoureString("Email");
                workSheet.Cells[rowStart, 7].Value = CoreResource.GetResoureString("UserGroup");
                workSheet.Cells[rowStart, 8].Value = CoreResource.GetResoureString("Mobile");
                workSheet.Cells[rowStart, 9].Value = CoreResource.GetResoureString("WorkPhone");
                workSheet.Cells[rowStart, 10].Value = CoreResource.GetResoureString("Status");

                var forLoopIndex = rowStart + 1;

                for (int i = 1; i <= 10; i++)
                {
                    workSheet.Column(i).Style.Font.Size = 11;
                    workSheet.Column(i).Style.Font.Name = "Times New Roman";
                    workSheet.Column(i).Style.Font.Color.SetColor(Color.Black);

                    workSheet.Cells[1, i].Style.Fill.PatternType = ExcelFillStyle.DarkGrid;
                    workSheet.Cells[1, i].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                }

                foreach (var user in users)
                {
                    workSheet.Cells[forLoopIndex, 1].Value = tableIndex;
                    workSheet.Cells[forLoopIndex, 2].Value = user.FullName;
                    workSheet.Cells[forLoopIndex, 3].Value = user.JobTitleName;
                    workSheet.Cells[forLoopIndex, 4].Value = user.OrganizationUnitName;
                    workSheet.Cells[forLoopIndex, 5].Value = user.OrganizationName;
                    workSheet.Cells[forLoopIndex, 6].Value = user.Email;
                    workSheet.Cells[forLoopIndex, 7].Value = user.UserGroupName;
                    workSheet.Cells[forLoopIndex, 8].Value = user.Mobile;
                    workSheet.Cells[forLoopIndex, 9].Value = user.WorkPhone;
                    if (user.Status == (int)StatusUser.Active)
                    {
                        workSheet.Cells[forLoopIndex, 10].Value = CoreResource.GetResoureString("Status_Active");
                        workSheet.Cells[forLoopIndex, 10].Style.Font.Color.SetColor(Color.Green);
                        workSheet.Cells[forLoopIndex, 10].Style.Font.Bold = true;
                    }
                    else
                    {
                        workSheet.Cells[forLoopIndex, 10].Value = CoreResource.GetResoureString("Status_Inactive");
                        workSheet.Cells[forLoopIndex, 10].Style.Font.Color.SetColor(Color.Red);
                    }
                    forLoopIndex++;
                    tableIndex++;
                }

                ExcelTable excelTable = workSheet.Tables.Add(workSheet.Cells[rowStart, 1, rowEnd, 10], "Users");

                excelTable.ShowHeader = true;

                workSheet.Row(1).Style.Font.Size = 10;
                workSheet.Row(1).Style.Font.Name = "Arial";
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Row(1).Style.Font.Color.SetColor(Color.Black);

                excelTable.TableStyle = TableStyles.Light8;

                workSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Columns[10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[rowStart, 1, rowEnd, 10].AutoFitColumns();

                excelTable.ShowFilter = false;

                package.Save();
            }

            stream.Position = 0;
            return stream;
        }
        /// <summary>
        /// Trả về file excel mẫu cho người dùng nhập
        /// </summary>
        /// <returns>stream chứa thông tin file mẫu</returns>
        /// CreatedBy: TNDanh (17/9/2022)
        public async Task<MemoryStream> ExportDefaultExcel()
        {
            await Task.Yield();
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add(CoreResource.GetResoureString("UsersExcelWorkSheet"));
                var rowStart = 1;

                for (int i = 1; i <= 9; i++)
                {
                    workSheet.Column(i).Style.Font.Size = 11;
                    workSheet.Column(i).Style.Font.Name = "Times New Roman";
                    workSheet.Column(i).Style.Font.Color.SetColor(Color.Black);

                    workSheet.Cells[1, i].Style.Fill.PatternType = ExcelFillStyle.DarkGrid;
                    workSheet.Cells[1, i].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                }

                workSheet.Row(1).Style.Font.Size = 10;
                workSheet.Row(1).Style.Font.Name = "Arial";
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Row(1).Style.Font.Color.SetColor(Color.Black);

                workSheet.Cells[rowStart, 1].Value = CoreResource.GetResoureString("FullName") + " *";
                workSheet.Cells[rowStart, 2].Value = CoreResource.GetResoureString("JobTitle") + " *";
                workSheet.Cells[rowStart, 3].Value = CoreResource.GetResoureString("OrganizationUnit");
                workSheet.Cells[rowStart, 4].Value = CoreResource.GetResoureString("Organization") + " *";
                workSheet.Cells[rowStart, 5].Value = CoreResource.GetResoureString("Email");
                workSheet.Cells[rowStart, 6].Value = CoreResource.GetResoureString("UserGroup");
                workSheet.Cells[rowStart, 7].Value = CoreResource.GetResoureString("Mobile");
                workSheet.Cells[rowStart, 8].Value = CoreResource.GetResoureString("WorkPhone");
                workSheet.Cells[rowStart, 9].Value = CoreResource.GetResoureString("Status");

                workSheet.Cells[1, 1, 1, 9].AutoFitColumns();

                package.Save();
            }
            stream.Position = 0;
            return stream;
        }
        /// <summary>
        /// Trả về stream, để xuất ra file người dùng không hợp lệ
        /// </summary>
        /// <param name="invalidUsers">Danh sách thông tin của người dùng không hợp lệ</param>
        /// <returns>Stream chứa thông tin người dùng không hợp lệ</returns>
        /// CreatedBy: TNDanh (21/9/2022)
        public async Task<MemoryStream> ExportInvalidUsers(List<InvalidUser> invalidUsers)
        {
            await Task.Yield();
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add(CoreResource.GetResoureString("InvalidUsersExcelWorkSheet"));
                var rowStart = 1;
                var rowEnd = invalidUsers.Count + rowStart;

                var tableIndex = 1;
                workSheet.Cells[rowStart, 1].Value = CoreResource.GetResoureString("Index");
                workSheet.Cells[rowStart, 2].Value = CoreResource.GetResoureString("FullName");
                workSheet.Cells[rowStart, 3].Value = CoreResource.GetResoureString("JobTitle");
                workSheet.Cells[rowStart, 4].Value = CoreResource.GetResoureString("OrganizationUnit");
                workSheet.Cells[rowStart, 5].Value = CoreResource.GetResoureString("Organization");
                workSheet.Cells[rowStart, 6].Value = CoreResource.GetResoureString("Email");
                workSheet.Cells[rowStart, 7].Value = CoreResource.GetResoureString("UserGroup");
                workSheet.Cells[rowStart, 8].Value = CoreResource.GetResoureString("Mobile");
                workSheet.Cells[rowStart, 9].Value = CoreResource.GetResoureString("WorkPhone");
                workSheet.Cells[rowStart, 10].Value = CoreResource.GetResoureString("Status");
                workSheet.Cells[rowStart, 11].Value = CoreResource.GetResoureString("ErrorOfInvalidUser");

                for (int i = 1; i <= 10; i++)
                {
                    workSheet.Column(i).Style.Font.Size = 11;
                    workSheet.Column(i).Style.Font.Name = "Times New Roman";
                    workSheet.Column(i).Style.Font.Color.SetColor(Color.Black);

                    workSheet.Cells[1, i].Style.Fill.PatternType = ExcelFillStyle.DarkGrid;
                    workSheet.Cells[1, i].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                }

                var forLoopIndex = rowStart + 1;
                foreach (var user in invalidUsers)
                {
                    workSheet.Cells[forLoopIndex, 1].Value = tableIndex;
                    workSheet.Cells[forLoopIndex, 2].Value = user.FullName;
                    workSheet.Cells[forLoopIndex, 3].Value = user.JobTitleName;
                    workSheet.Cells[forLoopIndex, 4].Value = user.OrganizationUnitName;
                    workSheet.Cells[forLoopIndex, 5].Value = user.OrganizationName;
                    workSheet.Cells[forLoopIndex, 6].Value = user.Email;
                    workSheet.Cells[forLoopIndex, 7].Value = user.UserGroupName;
                    workSheet.Cells[forLoopIndex, 8].Value = user.Mobile;
                    workSheet.Cells[forLoopIndex, 9].Value = user.WorkPhone;
                    if (user.Status == 3)
                    {
                        workSheet.Cells[forLoopIndex, 10].Value = CoreResource.GetResoureString("Status_Active");
                        workSheet.Cells[forLoopIndex, 10].Style.Font.Color.SetColor(Color.Green);
                        workSheet.Cells[forLoopIndex, 10].Style.Font.Bold = true;

                    }
                    else
                    {
                        workSheet.Cells[forLoopIndex, 10].Value = CoreResource.GetResoureString("Status_Inactive");
                        //workSheet.Cells[forLoopIndex, 10].Style.Font.Color.SetColor(Color.Red);
                    }
                    workSheet.Cells[forLoopIndex, 11].Value = user.ErrorUser;
                    workSheet.Cells[forLoopIndex, 11].Style.Font.Color.SetColor(Color.Red);
                    forLoopIndex++;
                    tableIndex++;
                }

                ExcelTable excelTable = workSheet.Tables.Add(workSheet.Cells[rowStart, 1, rowEnd, 10], "Users");

                excelTable.ShowHeader = true;

                workSheet.Row(1).Style.Font.Size = 10;
                workSheet.Row(1).Style.Font.Name = "Arial";
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Row(1).Style.Font.Color.SetColor(Color.Black);


                excelTable.TableStyle = TableStyles.Light8;

                workSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Columns[10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[rowStart, 1, rowEnd, 11].AutoFitColumns();

                excelTable.ShowFilter = false;

                package.Save();
            }

            stream.Position = 0;
            return stream;
        }
        /// <summary>
        /// Nhận file excel rồi render ra danh sách thông tin người dùng
        /// </summary>
        /// <returns>Danh sách người dùng từ file excel</returns>
        /// CreatedBy: TNDanh (21/9/2022)
        public async Task<object> ImportExcelToSql(IFormFile file)
            {
            var validUsers = new List<object>();
            var invalidUsers = new List<object>();
            int rowCount = 0;
            int rowIllegal = 0;

            List<UserDTO> users = new List<UserDTO>();

            var jobTitles = _jobTitlesRepository.GetAll();
            var userGroups = _userGroupsRepository.GetAll();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        UserDTO user = new UserDTO();
                        // Họ và tên
                        string fullName = "";
                        if (worksheet.Cells[row, 1].Value != null)
                        {
                            fullName = worksheet.Cells[row, 1].Value.ToString().Trim();
                        }
                        string[] arrFullName = fullName.Split(" ");
                        var lastName = arrFullName.Last();
                        var firstName = String.Join(" ", arrFullName.SkipLast(1).ToArray());

                        int userStatus = 0;
                        if (worksheet.Cells[row, 9].Value != null)
                        {
                            userStatus = worksheet.Cells[row, 9].Value.ToString().Trim().ToLower() == "Đang hoạt động".ToLower() ? (int)StatusUser.Active : (int)StatusUser.Inactive;
                        }
                        else
                        {
                            userStatus = 1;
                        }
                        string email = worksheet.Cells[row, 5].Value == null ? "" : worksheet.Cells[row, 5].Value.ToString().Trim();
                        // Chức vụ
                        string jobTitleName = "";
                        int jobTitleID = 1;

                        if (worksheet.Cells[row, 2].Value != null)
                        {
                            jobTitleName = worksheet.Cells[row, 2].Value.ToString().Trim();
                            jobTitleID = jobTitles.FirstOrDefault(j => j.JobTitleName.ToLower() == jobTitleName.ToLower()) == null ? 1 : jobTitles.FirstOrDefault(j => j.JobTitleName.ToLower() == jobTitleName.ToLower()).JobTitleID;
                        }
                        // Đơn vị
                        string organization = "";
                        if (worksheet.Cells[row, 4].Value != null)
                        {
                            organization = worksheet.Cells[row, 4].Value.ToString().Trim();
                        }

                        string userGroupNameArray;
                        if (worksheet.Cells[row, 6].Value != null)
                        {
                            userGroupNameArray = worksheet.Cells[row, 6].Value.ToString().Trim();
                        }

                        user.UserID = Guid.NewGuid();
                        user.FullName = fullName;
                        user.FirstName = firstName;
                        user.LastName = lastName;
                        user.Email = email;
                        user.Mobile = (string?)worksheet.Cells[row, 7].Value;
                        user.WorkPhone = (string?)worksheet.Cells[row, 8].Value;
                        user.JobTitleID = jobTitleID;
                        user.JobTitleName = jobTitleName;
                        user.OrganizationUnitID = 1;
                        user.OrganizationUnitName = (string?)worksheet.Cells[row, 3].Value;
                        user.OrganizationID = 1;
                        user.OrganizationName = organization;
                        user.UserGroupIDs = "1";
                        user.UserGroupName = (string?)worksheet.Cells[row, 6].Value;
                        user.Status = userStatus;

                        users.Add(user);

                        //if (_userService.Insert(user).ErrorUser.Count() > 0)
                        //{
                        //    rowIllegal += 1;
                        //    invalidUsers.Add(_userService.Insert(user));
                        //} else
                        //{
                        //    validUsers.Add(_userService.Insert(user));
                        //}

                        //if (error.Count > 0)
                        //{
                        //    rowIllegal += 1;
                        //    invalidUsers.Add(new
                        //    {
                        //        UserID = Guid.NewGuid(),
                        //        FullName = fullName,
                        //        FirstName = firstName,
                        //        LastName = lastName,
                        //        Email = email,
                        //        Mobile = worksheet.Cells[row, 7].Value,
                        //        WorkPhone = worksheet.Cells[row, 8].Value,
                        //        JobTitleID = jobTitleID,
                        //        JobTitleName = jobTitleName,
                        //        OrganizationUnitID = 1,
                        //        OrganizationUnitName = worksheet.Cells[row, 3].Value,
                        //        OrganizationID = 1,
                        //        OrganizationName = organization,
                        //        UserGroupIDs = "1",
                        //        UserGroupName = worksheet.Cells[row, 6].Value,
                        //        Status = userStatus,
                        //        ErrorUser = String.Join(", ", error.ToArray())
                        //    }); ;
                        //}
                        //else
                        //{
                        //    validUsers.Add(new
                        //    {
                        //        UserID = Guid.NewGuid(),
                        //        FullName = fullName,
                        //        FirstName = firstName,
                        //        LastName = lastName,
                        //        Email = email,
                        //        Mobile = worksheet.Cells[row, 7].Value,
                        //        WorkPhone = worksheet.Cells[row, 8].Value,
                        //        JobTitleID = jobTitleID,
                        //        JobTitleName = jobTitleName,
                        //        OrganizationUnitID = 1,
                        //        OrganizationUnitName = worksheet.Cells[row, 3].Value,
                        //        OrganizationID = 1,
                        //        OrganizationName = organization,
                        //        UserGroupIDs = "1",
                        //        UserGroupName = worksheet.Cells[row, 6].Value,
                        //        Status = userStatus,
                        //        Erorr = error
                        //    });

                        //}

                    }
                }
            }
            //var res = new
            //{
            //    Rows = rowCount - 1,
            //    ValidRow = rowCount - 1 - rowIllegal,
            //    IllegalRow = rowIllegal,
            //    ValidUsers = validUsers,
            //    InvalidUsers = invalidUsers
            //};
            return users;
        }
        #endregion

    }
}
