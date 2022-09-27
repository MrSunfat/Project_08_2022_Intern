using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web06.APIS.Core.Entities
{
    public class Column
    {
        #region Properties
        /// <summary>
        /// Tên cột - tiếng việt
        /// </summary>
        [Key]
        public string Name { get; set; }
        /// <summary>
        /// Tên cột - Tiếng Anh
        /// </summary>
        public string Field { get; set; }   
        /// <summary>
        /// Lưu những cột được chọn để hiện thị
        /// </summary>
        public bool Selected { get; set; }
        /// <summary>
        /// Chiều ngang của trường đó
        /// </summary>
        public int Width { get; set; }
        #endregion
    }
}
