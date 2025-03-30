<template>
    <view class="pageBox">
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="快递信息管理"></u-navbar>

        <!-- 搜索框 -->
        <view class="u-padding-15 u-margin-bottom-15 coreshop-bg-white u-border-bottom">
            <u-search placeholder="请输入提货单号、收件人姓名等" v-model="searchKey" :show-action="true" action-text="搜索" :animation="false" @search="search" @custom="search"></u-search>
        </view>

        <!-- 快递信息列表 -->
        <view v-if="expressList.length">
            <view class="expressList" v-for="(item, index) in expressList" :key="index" @click="this.goToDetail(item)">
                <view class="top">
                    <view class="left">
                        <u-icon name="tags" :size="30" color="rgb(94,94,94)"></u-icon>
                        <view class="u-margin-left-20 u-margin-right-20 u-font-md ">提货单号：{{item.expno}}</view>
                    </view>
                    <view class="right">{{item.orderStatusName}}</view>
                </view>
                <view class="item">
                    <view class="content">
                        <view class="title u-line-2">收件人：{{item.recname}}</view>
                        <view class="type">{{item.recaddr}}</view>
                        <view class="delivery-time">配送时间：{{ item.sendtime }}</view>
                    </view>
                    <view class="right">
                        <view class="price">￥{{ item.totalpay }}</view>
                    </view>
                </view>
                <view class="bottom u-margin-top-20">
                    <view class="more u-font-xs">
                        备注：{{item.note}}
                    </view>
                </view>
            </view>
        </view>
		
		
        <!-- 无数据时默认显示 -->
        <view class="coreshop-emptybox" v-else>
            <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/data.png'" icon-size="300" text="未查询到数据" mode="list"></u-empty>
        </view>

        <!-- 加载更多 -->
        <u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
    </view>
</template>

<script>
export default {
    data() {
        return {
            searchKey: '', // 搜索关键字
            expressList: [],
            page: 1,
            limit: 10,
            status: 'loadmore',
            iconType: 'flower',
            loadText: {
                loadmore: '轻轻上拉',
                loading: '努力加载中',
                nomore: '实在没有了'
            }
        }
    },
    onLoad() {
        this.getExpressList();
    },
    // 页面滚动到底部触发事件
    onReachBottom() {
        let _this = this
        if (_this.status === 'loadmore') {
            _this.getExpressList()
        }
    },
    methods: {
		 goToDetail(item) {
			 console.log(item)
			 uni.navigateTo({
			     url: '/pages/kuaidi/detail/detail?orderDetail=' + JSON.stringify(item)
			 });
		},
        // 获取快递信息列表
        getExpressList() {
            let _this = this
            let data = {
                page: _this.page,
                limit: _this.limit,
                expno: _this.searchKey
            }
            _this.status = 'loading';
            this.$u.api.userKuaidiOrderList(data).then(res => {
				console.log('hjx1')
				console.log(res)
                if (res.status) {
                    _this.expressList = [..._this.expressList, ...res.data]
                    // 判断数据是否加载完毕
                    if (_this.page < res.otherData.totalPages) {
                        _this.page++
                        _this.status = 'loadmore'
                    } else {
                        _this.status = 'nomore'
                    }
                } else {
                    // 接口请求出错
                    _this.$u.toast(res.msg)
                    _this.status = 'loadmore'
                }
            });
        },
        // 搜索
        search() {
            this.page = 1;
            this.expressList = [];
            this.getExpressList();
        }
    }
}
</script>

<style lang="scss" scoped>
  @import "manage.scss";
</style>