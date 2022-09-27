using Microsoft.Extensions.Configuration;
using MISA.Web06.APIS.Core.DTO;
using MISA.Web06.APIS.Core.Entities;
using MISA.Web06.APIS.Core.Interfaces.Infrastructure;
using MISA.Web06.APIS.Core.Interfaces.Services;
using MISA.Web06.APIS.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySqlConnector;
using Dapper;

namespace MISA.Web06.APIS.Core.Services
{
    public class UserService : BaseService<InvalidUser>, IUserService
    {
        #region Properties
        //protected static List<string> error = new List<string>();
        protected MySqlConnection _mySqlConnection;
        protected readonly string connectString = "";
        #endregion

        #region Constructor
        public UserService(IConfiguration configuration)
        {
            connectString = configuration.GetConnectionString("MISAConnectString");
        }

        #endregion

        #region Methods
        /// <summary>
        /// Validate các trường bắt buộc và các trường mặc định
        /// </summary>
        /// <param name="users">Nội dung người dùng</param>
        /// <returns>Trả về thông tin các trường lỗi của người dùng</returns>
        /// CreatdBy: TNDanh (26/9/2022)
        public override async Task<object> InsertAsync(List<UserDTO> users)
        {
            var validUsers = new List<object>();
            var invalidUsers = new List<object>();
            int rowCount = users.Count;
            int rowIllegal = 0;
            foreach (var user in users)
            {
                List<string> error = new List<string>();
                InvalidUser invalidUser = new InvalidUser();

                // -- Validate các trường bắt buộc -- //
                // 1. Họ và tên
                string fullName = "";
                if (String.IsNullOrEmpty(user.FullName) == true)
                {
                    error.Add(CoreResource.GetResoureString("FullNameEmpty"));
                }
                else
                {
                    invalidUser.FullName = user.FullName.Trim();
                    string[] arrFullName = fullName.Split(" ");
                    invalidUser.LastName = arrFullName.Last();
                    invalidUser.FirstName = String.Join(" ", arrFullName.SkipLast(1).ToArray());

                }
                // 2. Chức vụ
                string jobTitleName = "";
                int jobTitleID = 1;

                if (String.IsNullOrEmpty(user.JobTitleName) == true)
                {
                    error.Add(CoreResource.GetResoureString("JobTitleEmpty"));
                }
                else
                {
                    invalidUser.JobTitleID = user.JobTitleID;
                    invalidUser.JobTitleName = user.JobTitleName.Trim();
                }
                // 3. Đơn vị
                string organization = "";
                if (String.IsNullOrEmpty(user.OrganizationName) == true)
                {
                    error.Add(CoreResource.GetResoureString("OrganizationEmpty"));
                }
                else
                {
                    invalidUser.OrganizationName = user.OrganizationName.Trim();
                }
           
                // -- Validate nâng cao -- //
                // 4. Email
                if (String.IsNullOrEmpty(user.Email) != true)
                {
                    if (validateEmailRegex.IsMatch(user.Email) != true)
                    {
                        error.Add(CoreResource.GetResoureString("EmailInvalid"));
                    }
                }
                // 5. Điện thoại di động
                if (String.IsNullOrEmpty(user.Mobile) != true)
                {
                    if (user.Mobile.Length > 9 && user.Mobile.Length < 12)
                    {
                        error.Add(CoreResource.GetResoureString("PhoneInvalid"));
                    }
                }
                // 6. Điện thoại cơ quan
                if (!String.IsNullOrEmpty(user.Email) == true)
                {
                    if (user.Mobile.Length > 9 && user.Mobile.Length < 12)
                    {
                        error.Add(CoreResource.GetResoureString("PhoneInvalid"));

                    }
                }

                var sqlComand = $"SELECT u.Mobile FROM user u WHERE u.Mobile = {user.Mobile}";
                var dupliacateMobile = "";
                using (_mySqlConnection = new MySqlConnection(connectString))
                {
                    dupliacateMobile = await _mySqlConnection.QueryFirstOrDefaultAsync(sqlComand, commandType: System.Data.CommandType.StoredProcedure);
                }

                if (dupliacateMobile != "")
                {
                    error.Add("Số điện thoại bị trùng !");
                }

                invalidUser.ErrorUser = String.Join(", ", error);
                invalidUser.UserID = user.UserID;
                invalidUser.Email = user.Email;
                invalidUser.WorkPhone = user.WorkPhone;
                invalidUser.Mobile = user.Mobile;
                invalidUser.OrganizationID = user.OrganizationID;
                invalidUser.Status = user.Status;
                invalidUser.OrganizationUnitID = user.OrganizationUnitID;
                invalidUser.OrganizationUnitName = user.OrganizationUnitName;

                if (invalidUser.ErrorUser.Count() > 0)
                {
                    rowIllegal += 1;
                    invalidUsers.Add(invalidUser);
                } else
                {
                    validUsers.Add(user);
                }
            }
            var res = new
            {
                Rows = rowCount,
                ValidRow = rowCount - rowIllegal,
                IllegalRow = rowIllegal,
                ValidUsers = validUsers,
                InvalidUsers = invalidUsers
            };
            return res;
        }

        public static object ValidateRequired(string property, string nameResource)
        {
            if (String.IsNullOrEmpty(property) == true)
            {
                return new {
                    Status = 404,
                    Message = CoreResource.GetResoureString(nameResource)
                };   
            }
            return new
            {
                Status = 200,
                Message = property
            };
            
        }
        public int? GetOrganizationUnitID(List<OrganizationUnit> organizationUnits, string? organizationUnitName, string? organizationName)
        {
            if (string.IsNullOrEmpty(organizationUnitName) && !string.IsNullOrEmpty(organizationName))
            {
                foreach (var organizationUnit in organizationUnits)
                {
                    if (organizationUnit.OrganizationUnitName == organizationName) return organizationUnit.OrganizationUnitID;
                }
            }
            else if (!string.IsNullOrEmpty(organizationUnitName) && !string.IsNullOrEmpty(organizationName))
            {
                foreach (var organizationUnit in organizationUnits)
                {
                    if (organizationUnit.OrganizationUnitName == organizationUnitName) return organizationUnit.OrganizationUnitID;
                }
            }
            return null;
        }
        #endregion
    }
}
