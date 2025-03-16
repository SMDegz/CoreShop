<template>
    <view>
        <u-navbar title="签到中心"></u-navbar>
        <view v-if="show">
            <u-toast ref="uToast" />
            <u-no-network></u-no-network>
            <view v-if="condition.status" class="coreshop-bg-green coreshop-solid-top u-padding-left-80 u-padding-right-80 u-padding-top-45 u-padding-bottom-45">
                <view class="u-text-center u-margin-bottom-20">
                    <text class="coreshop-text-white u-font-40">{{condition.msg}}</text>
                </view>
                <progress class="coreshop-progress-radius" :percent="condition.status ? 100 : 0" active stroke-width="10" activeColor="#fbbd08" />
                <view class="u-text-left u-margin-bottom-20  u-margin-top-40">
                    <text class="coreshop-text-white u-font-xs">'瞧你这记性，今天的签到你早都完成啦，别重复 “打卡” 啦～'</text>
                </view>
            </view>
			<view v-else class="coreshop-bg-red coreshop-solid-top u-padding-left-80 u-padding-right-80 u-padding-top-45 u-padding-bottom-45">
			    <view class="u-text-center u-margin-bottom-20">
			        <text class="coreshop-text-white u-font-40">{{condition.msg}}</text>
			    </view>
			    <progress class="coreshop-progress-radius" percent="100" active stroke-width="10" activeColor="#fbbd08" />
			    <view class="u-text-left u-margin-bottom-20  u-margin-top-40">
			        <text class="coreshop-text-white u-font-xs"> '瞧呀，今天的签到还在这儿等着你呢，别磨蹭啦，快来 “打卡留念” 吧～'</text>
			    </view>
			</view>
            <!--标题-->
            <view class="coreshop-text-black u-font-md  u-padding-20 coreshop-solid-bottom">签到须知</view>
            <!--内容-->
            <view class="coreshop-text-gray u-margin-20">
                <u-parse :html="distributionNotes"></u-parse>
            </view>
            <view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
                <view class="flex u-padding-20 flex-direction">
                    <u-button :custom-style="customStyle" size="medium" type="error" v-if="condition.status == false" @click="goApply()">签到</u-button>
                    <u-button :custom-style="customStyle" size="medium" type="primary" v-else>今日已完成签到</u-button>
                </view>
            </view>
        </view>
    </view>

</template>
<script>
    export default {
        data() {
            return {
                customStyle: {
                    width: '100%',
                },
                show: true,
                condition: {}
            }
        },
        methods: {
            goApply() {
                var _this = this;
                _this.$u.api.sign().then(res => {
                	console.log('hjx1')
                	console.log(res)
					if(res.msg=="今天已经签到，无需重复签到"){
						_this.$refs.uToast.show({ title: "今天已经签到，无需重复签到", type: 'error', back: false });
					}
					else if(res.status){
						var str = "签到成功，获得"+ res.data +"点积分"
						_this.$refs.uToast.show({ title: str, type: 'success', back: false });
						_this.condition.status = true;
						_this.condition.msg = str
					}else{
						_this.$refs.uToast.show({ title: "签到失败，请联系客服！", type: 'error', back: false });
					}
                });
            }
        },
        computed: {
            distributionNotes() {
                return '<body><li><strong>签到入口</strong><p style="margin-left:20px">打开小程序，找到 “每日签到” 图标，点击即可进入签到页面。</p></li><li><strong>签到时间</strong><p style="margin-left:20px">每天00:00 - 23:59为当日签到时段，错过将无法补签当日签到。</p></li><li><strong>签到奖励</strong><p  style="margin-left:20px">连续签到可获得积分、优惠券等奖励。若中断签到，连续签到天数将重新计算。</p></li><li><strong>异常处理</strong><p style="margin-left:20px">若签到时遇到卡顿、无反应等异常情况，请先检查网络连接，或尝试退出小程序重新进入。若问题仍未解决，请联系客服。</p></li><li class="alert"><strong>特别提醒</strong><p style="margin-left:20px">请勿使用第三方工具或作弊手段进行签到，一经发现，将取消奖励获取资格，并可能限制账号使用。</p></li></body></html>'
            }
        },
        onLoad: function () {
            var _this = this;
            _this.$u.api.isSign().then(res => {
				console.log('hjx')
				console.log(res)
				_this.condition = res
            });
        }
    }
</script>
<style lang="scss" scoped>
    @import "index.scss";
</style>
