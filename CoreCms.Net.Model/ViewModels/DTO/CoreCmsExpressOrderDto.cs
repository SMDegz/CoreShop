using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCms.Net.Model.ViewModels.DTO
{
    public class CoreCmsExpressOrderDto:  FMPageByIntId
    {
        public System.Int32 userid { get; set; }


        public System.String expno { get; set; }


        public System.String expcom { get; set; }


        /// <summary>
        /// 门店编号
        /// </summary>

        public System.String storeid { get; set; }


        /// <summary>
        /// 门店名称
        /// </summary>

        public System.String storename { get; set; }


        /// <summary>
        /// 代拿列表
        /// </summary>

        public System.String explist { get; set; }


        /// <summary>
        /// 收件人
        /// </summary>

        public System.String recname { get; set; }


        /// <summary>
        /// 联系电话
        /// </summary>

        public System.String rectel { get; set; }


        /// <summary>
        /// 收货地址
        /// </summary>

        public System.String recaddr { get; set; }


        /// <summary>
        /// 派件时间



        public System.String sendtime { get; set; }


        /// <summary>
        /// 总金额
        /// </summary>



        public System.Int32 totalpay { get; set; }







        public System.Int32? discount { get; set; }







        public System.Int32? pointuse { get; set; }







        public System.Int32? serverid { get; set; }



        public System.String serverticker { get; set; }


        public System.Int32? orderstatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>

        public System.String note { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>

        public System.DateTime? createtime { get; set; }
    }
}
