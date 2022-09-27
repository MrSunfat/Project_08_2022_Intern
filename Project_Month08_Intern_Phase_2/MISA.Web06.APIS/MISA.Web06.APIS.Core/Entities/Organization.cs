using System.ComponentModel.DataAnnotations;

namespace MISA.Web06.APIS.Core.Entities
{
    public class Organization : BaseEntities
    {
        #region Properties
        /// <summary>
        /// ID của đơn vị
        /// </summary>
        [Key]
        public int OrganizationID { get; set; }
        /// <summary>
        /// Tên đơn vị
        /// </summary>
        public string OrganizationName { get; set; }
        #endregion
    }
}
