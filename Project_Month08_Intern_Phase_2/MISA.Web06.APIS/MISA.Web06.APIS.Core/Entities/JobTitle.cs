using System.ComponentModel.DataAnnotations;

namespace MISA.Web06.APIS.Core.Entities
{
    public class JobTitle : BaseEntities
    {
        #region Properties
        /// <summary>
        /// ID của chức vụ
        /// </summary>
        [Key]
        public int JobTitleID { get; set; }
        /// <summary>
        /// Tên chức vụ
        /// </summary>
        public string JobTitleName { get; set; }
        #endregion
    }
}
