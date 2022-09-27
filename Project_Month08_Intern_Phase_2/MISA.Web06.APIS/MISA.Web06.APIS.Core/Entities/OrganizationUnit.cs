using System.ComponentModel.DataAnnotations;

namespace MISA.Web06.APIS.Core.Entities
{
    public class OrganizationUnit : BaseEntities
    {
        #region Properties
        /// <summary>
        /// ID của phòng ban
        /// </summary>
        [Key]
        public int OrganizationUnitID { get; set; }
        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string OrganizationUnitName { get; set; }
        /// <summary>
        /// ID của đơn vị 
        /// </summary>
        public int OrganizationID { get; set; }
        /// <summary>
        /// Tên của đơn vị
        /// </summary>
        public string OrganizationName { get; set; }
        #endregion

    }
}
