/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2025/2/15 21:05:15
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Filter;
using CoreCms.Net.Loging;
using CoreCms.Net.IServices;
using CoreCms.Net.Utility.Helper;
using CoreCms.Net.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using SqlSugar;
using CoreCms.Net.Web.Admin.Infrastructure;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    /// 
    ///</summary>
    [Description("")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize]
    public class CoreCmsUserSignController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsUserSignServices _CoreCmsUserSignServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsUserSignController(IWebHostEnvironment webHostEnvironment
            ,ICoreCmsUserSignServices CoreCmsUserSignServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _CoreCmsUserSignServices = CoreCmsUserSignServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsUserSign/GetPageList
         /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<AdminUiCallBack> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsUserSign>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();

            Expression<Func<CoreCmsUserSign, object>> orderEx = orderField switch
            {
                "Id" => p => p.Id,"UserId" => p => p.UserId,"SignDate" => p => p.SignDate,"SignPoint" => p => p.SignPoint,"ContinuityDays" => p => p.ContinuityDays,"CreateTime" => p => p.CreateTime,
                _ => p => p.Id
            };

            //设置排序方式
            var orderDirection = Request.Form["orderDirection"].FirstOrDefault();
            var orderBy = orderDirection switch
            {
                "asc" => OrderByType.Asc,
                "desc" => OrderByType.Desc,
                _ => OrderByType.Desc
            };
            //查询筛选
			
			// int
			var Id = Request.Form["Id"].FirstOrDefault().ObjectToInt(0);
            if (Id > 0)
            {
                where = where.And(p => p.Id == Id);
            }
			// int
			var UserId = Request.Form["UserId"].FirstOrDefault().ObjectToInt(0);
            if (UserId > 0)
            {
                where = where.And(p => p.UserId == UserId);
            }
			// date
			var SignDate = Request.Form["SignDate"].FirstOrDefault();
            if (!string.IsNullOrEmpty(SignDate))
            {
                where = where.And(p => p.SignDate == (Convert.ToDateTime(SignDate)));
            }
			// int
			var SignPoint = Request.Form["SignPoint"].FirstOrDefault().ObjectToInt(0);
            if (SignPoint > 0)
            {
                where = where.And(p => p.SignPoint == SignPoint);
            }
			// int
			var ContinuityDays = Request.Form["ContinuityDays"].FirstOrDefault().ObjectToInt(0);
            if (ContinuityDays > 0)
            {
                where = where.And(p => p.ContinuityDays == ContinuityDays);
            }
			// datetime
			var CreateTime = Request.Form["CreateTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(CreateTime))
            {
                if (CreateTime.Contains("到"))
                {
                    var dts = CreateTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.CreateTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.CreateTime < dtEnd);
                }
                else
                {
                    var dt = CreateTime.ObjectToDate();
                    where = where.And(p => p.CreateTime > dt);
                }
            }
            //获取数据
            var list = await _CoreCmsUserSignServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize, true);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsUserSign/GetIndex
        /// <summary>
        /// 首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public AdminUiCallBack GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return jm;
        }
        #endregion

        #region 创建数据============================================================
        // POST: Api/CoreCmsUserSign/GetCreate
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public AdminUiCallBack GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return jm;
        }
        #endregion

        #region 创建提交============================================================
        // POST: Api/CoreCmsUserSign/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody]CoreCmsUserSign entity)
        {
            var jm = await _CoreCmsUserSignServices.InsertAsync(entity);
            return jm;
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsUserSign/GetEdit
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<AdminUiCallBack> GetEdit([FromBody]FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _CoreCmsUserSignServices.QueryByIdAsync(entity.id, false);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;
            jm.data = model;

            return jm;
        }
        #endregion

        #region 编辑提交============================================================
        // POST: Api/CoreCmsUserSign/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody]CoreCmsUserSign entity)
        {
            var jm = await _CoreCmsUserSignServices.UpdateAsync(entity);
            return jm;
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/CoreCmsUserSign/DoDelete/10
        /// <summary>
        /// 单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody]FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _CoreCmsUserSignServices.ExistsAsync(p => p.Id == entity.id, true);
            if (!model)
            {
                jm.msg = GlobalConstVars.DataisNo;
				return jm;
            }
            jm = await _CoreCmsUserSignServices.DeleteByIdAsync(entity.id);

            return jm;
        }
        #endregion

        #region 批量删除============================================================
        // POST: Api/CoreCmsUserSign/DoBatchDelete/10,11,20
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除")]
        public async Task<AdminUiCallBack> DoBatchDelete([FromBody]FMArrayIntIds entity)
        {
            var jm = await _CoreCmsUserSignServices.DeleteByIdsAsync(entity.id);
            return jm;
        }

        #endregion

        #region 预览数据============================================================
        // POST: Api/CoreCmsUserSign/GetDetails/10
        /// <summary>
        /// 预览数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("预览数据")]
        public async Task<AdminUiCallBack> GetDetails([FromBody]FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _CoreCmsUserSignServices.QueryByIdAsync(entity.id, false);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;
            jm.data = model;

            return jm;
        }
        #endregion

        #region 选择导出============================================================
        // POST: Api/CoreCmsUserSign/SelectExportExcel/10
        /// <summary>
        /// 选择导出
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("选择导出")]
        public async Task<AdminUiCallBack> SelectExportExcel([FromBody]FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var mySheet = book.CreateSheet("Sheet1");
            //获取list数据
            var listModel = await _CoreCmsUserSignServices.QueryListByClauseAsync(p => entity.id.Contains(p.Id), p => p.Id, OrderByType.Asc, true);
            //给sheet1添加第一行的头部标题
            var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);

            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);

            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);

            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);

            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);

            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);

            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);

            headerRow.Height = 30 * 20;
            var commonCellStyle = ExcelHelper.GetCommonStyle(book);

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listModel.Count(); i++)
            {
                var rowTemp = mySheet.CreateRow(i + 1);

                    var rowTemp0 = rowTemp.CreateCell(0);
                        rowTemp0.SetCellValue(listModel[i].Id.ToString());
                        rowTemp0.CellStyle = commonCellStyle;

                    var rowTemp1 = rowTemp.CreateCell(1);
                        rowTemp1.SetCellValue(listModel[i].UserId.ToString());
                        rowTemp1.CellStyle = commonCellStyle;

                    var rowTemp2 = rowTemp.CreateCell(2);
                        rowTemp2.SetCellValue(listModel[i].SignDate.ToString());
                        rowTemp2.CellStyle = commonCellStyle;

                    var rowTemp3 = rowTemp.CreateCell(3);
                        rowTemp3.SetCellValue(listModel[i].SignPoint.ToString());
                        rowTemp3.CellStyle = commonCellStyle;

                    var rowTemp4 = rowTemp.CreateCell(4);
                        rowTemp4.SetCellValue(listModel[i].ContinuityDays.ToString());
                        rowTemp4.CellStyle = commonCellStyle;

                    var rowTemp5 = rowTemp.CreateCell(5);
                        rowTemp5.SetCellValue(listModel[i].CreateTime.ToString());
                        rowTemp5.CellStyle = commonCellStyle;

            }
            // 导出excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsUserSign导出(选择结果).xls";
            string filePath = webRootPath + tpath;
            DirectoryInfo di = new DirectoryInfo(filePath);
            if (!di.Exists)
            {
                di.Create();
            }
            FileStream fileHssf = new FileStream(filePath + fileName, FileMode.Create);
            book.Write(fileHssf);
            fileHssf.Close();

            jm.code = 0;
            jm.msg = GlobalConstVars.ExcelExportSuccess;
            jm.data = tpath + fileName;

            return jm;
        }
        #endregion

        #region 查询导出============================================================
        // POST: Api/CoreCmsUserSign/QueryExportExcel/10
        /// <summary>
        /// 查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsUserSign>();
                //查询筛选
			
			// int
			var Id = Request.Form["Id"].FirstOrDefault().ObjectToInt(0);
            if (Id > 0)
            {
                where = where.And(p => p.Id == Id);
            }
			// int
			var UserId = Request.Form["UserId"].FirstOrDefault().ObjectToInt(0);
            if (UserId > 0)
            {
                where = where.And(p => p.UserId == UserId);
            }
			// date
			var SignDate = Request.Form["SignDate"].FirstOrDefault();
            if (!string.IsNullOrEmpty(SignDate))
            {
                where = where.And(p => p.SignDate == (Convert.ToDateTime(SignDate)));
            }
			// int
			var SignPoint = Request.Form["SignPoint"].FirstOrDefault().ObjectToInt(0);
            if (SignPoint > 0)
            {
                where = where.And(p => p.SignPoint == SignPoint);
            }
			// int
			var ContinuityDays = Request.Form["ContinuityDays"].FirstOrDefault().ObjectToInt(0);
            if (ContinuityDays > 0)
            {
                where = where.And(p => p.ContinuityDays == ContinuityDays);
            }
			// datetime
			var CreateTime = Request.Form["CreateTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(CreateTime))
            {
                var dt = CreateTime.ObjectToDate();
                where = where.And(p => p.CreateTime > dt);
            }
            //获取数据
            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var mySheet = book.CreateSheet("Sheet1");
            //获取list数据
            var listModel = await _CoreCmsUserSignServices.QueryListByClauseAsync(where, p => p.Id, OrderByType.Asc, true);
            //给sheet1添加第一行的头部标题
                var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);
            
            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);
			
            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);
			
            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);
			
            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);
			
            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);
			
            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);
			

            headerRow.Height = 30 * 20;
            var commonCellStyle = ExcelHelper.GetCommonStyle(book);

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listModel.Count; i++)
            {
                var rowTemp = mySheet.CreateRow(i + 1);


            var rowTemp0 = rowTemp.CreateCell(0);
            rowTemp0.SetCellValue(listModel[i].Id.ToString());
            rowTemp0.CellStyle = commonCellStyle;



            var rowTemp1 = rowTemp.CreateCell(1);
            rowTemp1.SetCellValue(listModel[i].UserId.ToString());
            rowTemp1.CellStyle = commonCellStyle;



            var rowTemp2 = rowTemp.CreateCell(2);
            rowTemp2.SetCellValue(listModel[i].SignDate.ToString());
            rowTemp2.CellStyle = commonCellStyle;



            var rowTemp3 = rowTemp.CreateCell(3);
            rowTemp3.SetCellValue(listModel[i].SignPoint.ToString());
            rowTemp3.CellStyle = commonCellStyle;



            var rowTemp4 = rowTemp.CreateCell(4);
            rowTemp4.SetCellValue(listModel[i].ContinuityDays.ToString());
            rowTemp4.CellStyle = commonCellStyle;



            var rowTemp5 = rowTemp.CreateCell(5);
            rowTemp5.SetCellValue(listModel[i].CreateTime.ToString());
            rowTemp5.CellStyle = commonCellStyle;


            }
            // 写入到excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsUserSign导出(查询结果).xls";
            string filePath = webRootPath + tpath;
            DirectoryInfo di = new DirectoryInfo(filePath);
            if (!di.Exists)
            {
                di.Create();
            }
            FileStream fileHssf = new FileStream(filePath + fileName, FileMode.Create);
            book.Write(fileHssf);
            fileHssf.Close();

            jm.code = 0;
            jm.msg = GlobalConstVars.ExcelExportSuccess;
            jm.data = tpath + fileName;

            return jm;
        }
        #endregion

        

    }
}
