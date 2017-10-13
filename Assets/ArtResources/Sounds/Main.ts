//////////////////////////////////////////////////////////////////////////////////////
//
//  Copyright (c) 2014-present, Egret Technology.
//  All rights reserved.
//  Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions are met:
//
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the Egret nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
//
//  THIS SOFTWARE IS PROVIDED BY EGRET AND CONTRIBUTORS "AS IS" AND ANY EXPRESS
//  OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
//  OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
//  IN NO EVENT SHALL EGRET AND CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
//  INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
//  LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;LOSS OF USE, DATA,
//  OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
//  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
//  EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//////////////////////////////////////////////////////////////////////////////////////

class Main extends egret.DisplayObjectContainer {

    /**
     * 加载进度界面
     * Process interface loading
     */
    private loadingView: LoadingUI;
    private WheelViewScene: WheelViewScene;

    public constructor() {
        super();

        DataGloble.openId = Functions.getQueryString('uid', 'i', window.location.href);

        // 从url || cookie里获取token
        var token = Functions.getQueryString('token', 'i', window.location.href);
        if (token === null) {
            token = getCookie('token');
        }
        DataGloble.token = token;
        egret.TextField.default_fontFamily = "Arial";
        egret.registerImplementation("eui.IAssetAdapter", new AssetAdapter());
        egret.registerImplementation("eui.IThemeAdapter", new ThemeAdapter());

        this.addEventListener(egret.Event.ADDED_TO_STAGE, this.onAddToStage, this);
    }

    private onAddToStage(event: egret.Event) {
        MethodCore.loadData('/game/basic/login', function(data, that){
            if (data) {
                DataGloble.userData = data;
                DataGloble.UserId = data['data']['uid'];

                console.log('openid:', DataGloble.openId - 0);
                console.log('uid:', DataGloble.UserId);
                console.log('userData:', data);

                // 设置攻击数据
                if (data['data']['attackTarget']) {
                    DataGloble.shootData = data['data']['attackTarget'];
                    DataGloble.wheelShootData = data['data']['attackTarget'];
                }

                // 设置偷取数据
                if (data['data']['stealIslands']) {
                    DataGloble.stealData = data['data']['stealIslands'];
                }

                // 简单统计
                window['TDGA'] && window['TDGA'].Account && window['TDGA'].Account({
                    accountId : DataGloble.userData['data']['uid'],
                    level : DataGloble.userData['data']['islandId'],
                    accountName : DataGloble.userData['data']['name'],
                    gender: DataGloble.userData['data']['gender']
                });

                // 向顶层页面传递friendshipCode、headImg
                PostMessage.refreshShareData();

                /*if (
                    data['data']['headImg'] && 
                    data['data']['friendshipCode'] && 
                    window && 
                    window.parent && 
                    typeof window.parent.postMessage === 'function'
                ) {
                    window.parent.postMessage({
                        info_type: 'msg_share_refresh_data',
                        info_data: {
                            friend_code: data['data']['friendshipCode'],
                            img_url: 'http://www.jianguoyouxi.com/pic/p.png?v=' + DataGloble.version,
                            share_title: '敢打劳资！把劳资的意大利炮拉出来！',
                            share_desc: '没有什么是一炮解决不了的，如果有，那就两炮！'
                        }
                    }, '*');
                }*/

                //设置加载进度界面
                //Config to load process interface
                this.loadingView = new LoadingUI();
                this.stage.addChild(this.loadingView);

                // 初始化Resource资源加载库
                RES.addEventListener(RES.ResourceEvent.CONFIG_COMPLETE, that.onConfigComplete, this);
                RES.loadConfig("resource/default.res.json?v=" + DataGloble.version, "resource/");
            } else {
                alert('获取登录信息错误 请重试!');
            }
        }, '', this);
        
    }

    private isThemeLoadEnd = false;
    private isResourceLoadEnd = false;

    /**
     * 配置文件加载完成,开始预加载preload资源组。
     * configuration file loading is completed, start to pre-load the preload resource group
     */
    private onConfigComplete(event: RES.ResourceEvent): void {
        var theme = new eui.Theme("resource/default.thm.json?v="+ DataGloble.version, this.stage);
        theme.addEventListener(eui.UIEvent.COMPLETE, this.onThemeLoadComplete, this);

        var islandId = DataGloble.userData['data']['islandId'];
        var guideStep = DataGloble.getGuideStep();
        var groups = ['preload', 'shop', 'CityIsland' + islandId];

        // 新手指导
        if (guideStep) {
            groups.push('newUserGuide', 'followPackage');
        }

        // 新手指导攻击场景
        if (~['6_1', '13_1'].indexOf(DataGloble.getGuideStepString())) {
            groups.push('shootview');
            if (islandId !== DataGloble.shootData['islandId']) {
                groups.push('CityIsland' + DataGloble.shootData['islandId']);
            }
        }

        // 新手指导偷取场景
        if (~['16_1'].indexOf(DataGloble.getGuideStepString())) {
            groups.push('steal');

            DataGloble.userData['data']['stealIslands'].forEach(function (data) {
                groups.push('CityIsland' + data.islandId);
            });
        }

        RES.removeEventListener(RES.ResourceEvent.CONFIG_COMPLETE, this.onConfigComplete, this);

        new LoadResGroup({
            groupName: groups,
            complete: this.onResourceLoadComplete,
            error: this.onResourceLoadError,
            progress: this.onResourceProgress,
            itemError: this.onItemLoadError
        }, this);
    }

    /**
     * preload资源组加载完成
     * Preload resource group is loaded
     */
    private onResourceLoadComplete(event: RES.ResourceEvent) {
        this.stage.removeChild(this.loadingView);

        // 清除loading
        var bg = document.getElementById('bg');
        if (bg && bg.parentNode) {
            bg.parentNode.removeChild(bg);
        }

        var bg_script = document.getElementById('bg_script');
        if (bg_script && bg_script.parentNode) {
            bg_script.parentNode.removeChild(bg_script);
        }

        this.isResourceLoadEnd = true;
        this.createScene();

        // 监听postMessage
        PostMessage.monitorPostMessage();
    }

    /**
     * 资源组加载出错
     *  The resource group loading failed
     */
    private onItemLoadError (event: RES.ResourceEvent) {
        console.warn("Url:" + event.resItem.url + " has failed to load");
    }

    /**
     * 资源组加载出错
     *  The resource group loading failed
     */
    private onResourceLoadError (event: RES.ResourceEvent) {
        //TODO
        console.warn("Group:" + event.groupName + " has failed to load");
        //忽略加载失败的项目
        //Ignore the loading failed projects
        this.onResourceLoadComplete(event);
    }

    /**
     * preload资源组加载进度
     * Loading process of preload resource group
     */
    private onResourceProgress (event: RES.ResourceEvent) {
        this.loadingView.setProgress(event.itemsLoaded, event.itemsTotal);
    }

    private onThemeLoadComplete(): void {
        this.isThemeLoadEnd = true;
        this.createScene();
    }

    private createScene () {
        if (this.isThemeLoadEnd && this.isResourceLoadEnd) {
            this.createGameScene();
        }
    }

    /**
     * 创建游戏场景
     * Create a game scene
     */
    private createGameScene() {

        // 主界面
        this.addChild(MainViewScene.getInstance());

        //商城界面
        this.addChild(MainShopViewScene.getInstance());
        // this.addChild(ShopViewScene.getInstance());
        // ShopViewScene.getInstance().initView();

        // 高于 主界面 场景集合（拼图场景）
        this.addChild(MainPlusViewScene.getInstance());

        // 未读信息，公告展示场景
        this.addChild(MainXViewScene.getInstance());

        // WebSocket提示框
        this.addChild(WebSocketScene.getInstance());

        // 新手指导
        if (DataGloble.getGuideStep()) {
            var newUserguideviewscene = NewUserGuideViewScene.getInstance();
            this.addChild(newUserguideviewscene);
            
            // 运行新手指导
            newUserguideviewscene.runStep(true);
        }

        // 接口loading
        this.addChild(DataInterfaceLoading.getInstance());

        // 公用信息提示场景
        this.addChild(PublicDialog.getInstance());

        // 最顶层场景
        this.addChild(TopScene.getInstance());

        // 如果不是新手指导，而且是游客身份，提示用户去关注公众号
        if (
            DataGloble.baseUrl.indexOf('caishenlaile.com') === -1 && 
            !DataGloble.getGuideStep() && 
            !DataGloble.userData["data"]["isAuthed"] &&
            !DataGloble.isWXBrowser
        ) {
            TopScene.getInstance().touristGoFollow();
        }


        /*var flag = 0;

        new LoadResGroup({
            groupName: 'stealAnimate',
            complete: () => {
                var data = RES.getRes("stealAnimate_json");
                var txtr = RES.getRes("stealAnimate_png");
                var mcFactory:egret.MovieClipDataFactory = new egret.MovieClipDataFactory(data, txtr);
                // mcFactory.frameRate
                console.log(mcFactory);

                var mc = new egret.MovieClip(mcFactory.generateMovieClipData("stealAnimate"));
                mc.frameRate = 20;
                
                mc.addEventListener(egret.MovieClipEvent.FRAME_LABEL, (e:egret.MovieClipEvent) => {
                    if (e.type === 'frame_label' && e.frameLabel === 'loop') {
                        if (flag === 0 || flag === 1) {
                            mc.gotoAndPlay(7, 1);
                            if (flag === 1) {
                                flag = 2;
                            }
                        }

                    }
                }, this);

                mc.addEventListener(egret.Event.COMPLETE, (e:egret.Event)=>{
                    console.log('COMPLETE', e.type);
                }, this);
                console.log(mc);
                this.addChild(mc);
                // mc.gotoAndPlay(-1, 1);
                mc.x = 150;
                mc.y = 320;

                var button = new eui.Image('icons_json.zhaomu');
                this.addChild(button);
                button.x = 320;
                button.y = 700;
                button.addEventListener(egret.TouchEvent.TOUCH_TAP, () => {
                    mc.nextFrame();
                    console.log('nextFrame', mc.currentFrame)
                    // flag = 1;
                    // mc.gotoAndPlay(7, 1);
                }, this);
            }
        }, this);*/
        

        /*var a = new egret.Bitmap(RES.getRes('icons_json.close'));
        a.touchEnabled = true;
        a.addEventListener(egret.TouchEvent.TOUCH_TAP, function() {
            MainLeftContainer.getInstance().dispatchMessageIconAction(0, 'email');
        }, this)
        this.addChild(a);
        a.x = 320;
        a.y = 100;*/

        /*egret.Tween.get(a, {
            loop: true,
            onChange: function () {
                // console.log(this.y)
            },
            onChangeObj: a
        }).to({
            y: 500
        }, 2000).to({
            y: 100
        }, 2000)*/

        /*var dataArr = [];
        dataArr.push({
            source: 'vip',
            title: 'X30天',
            callback: function(self){
                // EnergyScene.getInstance().changeEnergyText(DataGloble.userData['data']['energy']);
                MainLeftContainer.getInstance().dispatchMessageIconAction(0, 'email');
                self.parent.removeChild(self);
            }
        });
        new DataHandle(dataArr, this);*/
        
        // 加载声音
        SoundsCore.loadSounds();

        var sources = [
            'fly_animate',
            'wheel_animate',
            'ranking', // 排行榜
            'public_light',
            'followNotice', //关注提示
            'jigsaw_piece_'+ DataGloble['userData']['data']['jigsawInfo']['series'],
            'jigsawPublic',
            'newJigsawPublic'
        ];

        if (DataGloble.userData['data']['one_yuan_buying']['countdown']) {
            sources.push('giftPacks');
        }
        

        // 静默加载
        new LoadResGroup({groupName: sources}); 
        
    }

}


