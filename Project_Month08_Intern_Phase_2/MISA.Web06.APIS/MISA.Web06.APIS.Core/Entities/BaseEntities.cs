namespace MISA.Web06.APIS.Core.Entities
{
    public class BaseEntities
    {
        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người thay đổi
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Ngày thay đổi
        /// </summary>

        public DateTime? ModifiedDate { get; set; }
    }
}