/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2025/3/9 22:53:35
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Caching.Manual;
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 快递配送表 接口实现
    /// </summary>
    public class CoreCmsExpressOrderRepository : BaseRepository<CoreCmsExpressOrder>, ICoreCmsExpressOrderRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsExpressOrderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsExpressOrder entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync() > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsExpressOrder entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await DbClient.Queryable<CoreCmsExpressOrder>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
            jm.msg = "不存在此信息";
            return jm;
            }
            //事物处理过程开始
        	oldModel.id = entity.id;
            oldModel.expno = entity.expno;
            oldModel.expcom = entity.expcom;
            oldModel.storeid = entity.storeid;
            oldModel.storename = entity.storename;
            oldModel.explist = entity.explist;
            oldModel.recname = entity.recname;
            oldModel.rectel = entity.rectel;
            oldModel.recaddr = entity.recaddr;
            oldModel.sendtime = entity.sendtime;
            oldModel.totalpay = entity.totalpay;
            oldModel.discount = entity.discount;
            oldModel.pointuse = entity.pointuse;
            oldModel.serverid = entity.serverid;
            oldModel.serverticker = entity.serverticker;
            oldModel.note = entity.note;
            oldModel.createtime = entity.createtime;
            
            //事物处理过程结束
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsExpressOrder> entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Updateable(entity).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdAsync(object id)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Deleteable<CoreCmsExpressOrder>(id).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        /// <summary>
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Deleteable<CoreCmsExpressOrder>().In(ids).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        #endregion

       #region 获取缓存的所有数据==========================================================

        /// <summary>
        /// 获取缓存的所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<CoreCmsExpressOrder>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<CoreCmsExpressOrder>>(GlobalConstVars.CacheCoreCmsExpressOrder);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<CoreCmsExpressOrder>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<CoreCmsExpressOrder>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.CacheCoreCmsExpressOrder, list);
            return list;
        }

        #endregion


        #region 重写根据条件查询分页数据
        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public new async Task<IPageList<CoreCmsExpressOrder>> QueryPageAsync(Expression<Func<CoreCmsExpressOrder, bool>> predicate,
            Expression<Func<CoreCmsExpressOrder, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsExpressOrder> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsExpressOrder>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsExpressOrder
                {
                      id = p.id,
                expno = p.expno,
                expcom = p.expcom,
                storeid = p.storeid,
                storename = p.storename,
                explist = p.explist,
                recname = p.recname,
                rectel = p.rectel,
                recaddr = p.recaddr,
                sendtime = p.sendtime,
                totalpay = p.totalpay,
                discount = p.discount,
                pointuse = p.pointuse,
                serverid = p.serverid,
                serverticker = p.serverticker,
                note = p.note,
                createtime = p.createtime,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsExpressOrder>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsExpressOrder
                {
                      id = p.id,
                expno = p.expno,
                expcom = p.expcom,
                storeid = p.storeid,
                storename = p.storename,
                explist = p.explist,
                recname = p.recname,
                rectel = p.rectel,
                recaddr = p.recaddr,
                sendtime = p.sendtime,
                totalpay = p.totalpay,
                discount = p.discount,
                pointuse = p.pointuse,
                serverid = p.serverid,
                serverticker = p.serverticker,
                note = p.note,
                createtime = p.createtime,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<CoreCmsExpressOrder>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
