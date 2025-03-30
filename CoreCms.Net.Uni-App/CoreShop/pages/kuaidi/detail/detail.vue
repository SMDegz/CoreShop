<template>
    <view class="pageBox">
        <u-navbar title="快递信息详情"></u-navbar>

        <!-- 快递基本信息 -->
        <view class="coreshop-bg-white coreshop-card-box">
            <view class="coreshop-card-view">
                <view class="u-font-lg coreshop-text-bold coreshop-text-black">快递基本信息</view>
                <u-line color="#eee" border-style="dashed" margin="20rpx 0" />

                <view class="coreshop-text-black title-view">
                    <view class="title">快递单号</view>
                    <view class="u-text-right">
                        <text>{{ expressInfo.expno }}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view">
                    <view class="title">快递公司</view>
                    <view class="u-text-right">
                        <text>{{ expressInfo.expcom }}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view">
                    <view class="title">门店名称</view>
                    <view class="u-text-right">
                        <text>{{ expressInfo.storename }}</text>
                    </view>
                </view>
            </view>
        </view>

        <!-- 收件人信息 -->
        <view class="coreshop-bg-white coreshop-card-box">
            <view class="coreshop-card-view">
                <view class="u-font-lg coreshop-text-bold coreshop-text-black">收件人信息</view>
                <u-line color="#eee" border-style="dashed" margin="20rpx 0" />

                <view class="coreshop-text-black title-view">
                    <view class="title">收件人</view>
                    <view class="u-text-right">
                        <text>{{ expressInfo.recname }}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view">
                    <view class="title">联系电话</view>
                    <view class="u-text-right">
                        <text>{{ expressInfo.rectel }}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view">
                    <view class="title">地址</view>
                    <view class="u-text-right">
                        <text>{{ expressInfo.recaddr }}</text>
                    </view>
                </view>
            </view>
        </view>

        <!-- 订单信息 -->
        <view class="coreshop-bg-white coreshop-card-box">
            <view class="coreshop-card-view">
                <view class="u-font-lg coreshop-text-bold coreshop-text-black">订单信息</view>
                <u-line color="#eee" border-style="dashed" margin="20rpx 0" />

				<view v-if="expressInfo.coreCmsParcelStorageList && expressInfo.coreCmsParcelStorageList.length > 0">
					<view v-for="(item, index) in expressInfo.coreCmsParcelStorageList" :key="index" class="coreshop-text-black title-view">
						<!-- <view class="title">{{index + 1}}</view> -->
						<view class="u-text-right">
							<text>订单号：{{ item.tracking_number }}</text>
						</view>
						<!-- <view class="title">取件码</view> -->
						<view class="u-text-right">
							<text>取件码：{{ item.pickupcode }}</text>
						</view>
					</view>
				</view>
				<view v-else>
					<view class="coreshop-text-black title-view">
						<view class="title">暂无代拿快递信息</view>
					</view>
				</view>
				<u-line color="#eee" border-style="dashed" margin="20rpx 0" />
                <view class="coreshop-text-black title-view">
                    <view class="title">配送时间</view>
                    <view class="u-text-right">
                        <text>{{ expressInfo.sendtime }}</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view">
                    <view class="title">总金额</view>
                    <view class="u-text-right">
                        <text>{{ expressInfo.totalpay }} 元</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view">
                    <view class="title">优惠金额</view>
                    <view class="u-text-right">
                        <text>{{ expressInfo.discount || 0 }} 元</text>
                    </view>
                </view>
                <view class="coreshop-text-black title-view">
                    <view class="title">订单状态</view>
                    <view class="u-text-right">
                        <text>{{ expressInfo.orderStatusName }}</text>
                    </view>
                </view>
            </view>
        </view>
    </view>
</template>

<script>
export default {
    data() {
		return {
			expressInfo: {}
		};
	},
	onLoad(options) {
		// 接收列表页传递过来的详情信息
		this.expressInfo = JSON.parse(options.orderDetail);
	},
    methods: {
        getOrderStatusText(status) {
            switch (status) {
                case 1:
                    return '待处理';
                case 2:
                    return '已处理';
                case 3:
                    return '已完成';
                default:
                    return '未知状态';
            }
        }
    }
};
</script>

<style lang="scss" scoped>
@import "detail.scss";
</style>