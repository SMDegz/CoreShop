using CoreCms.Net.Model.FromBody;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCms.Net.Model.ViewModels.DTO
{
    public class CoreCmsParcelStorageDto: FMPageByIntId
    {
        /// <summary>
        /// 收件人手机号
        /// </summary>
        public System.String phone_number { get; set; }


        /// <summary>
        /// 所属门店/网点编号
        /// </summary>
        public System.String store_id { get; set; }
    }
}
