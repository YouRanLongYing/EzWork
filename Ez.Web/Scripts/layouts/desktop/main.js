/*!
 * WebDesk For Product JavascriptLibrary
 * DeskTop v1.0
 * Jquery v1.7.2
 * User: kongjing/endfalse@163.com
 * Date: 2014.08.08
 * Include Jquery (http://www.jquery.com/)
 */
(function ($, EzDialog) {
    /*在桌面头部生成“窗口”菜单时需要将当前窗口配置信息存放到按钮指定键的data下*/
    var WINDIW_ARGS = "win-args";
    /**
    * 检测拖动对象是否发生越界现象
    * @param {domElement} dragbtn 目标对象
    * */
    var _checkApp_btn_OverFlow = function (dragbtn) {
        var that = this; /*当前名空间下的指针对象*/
        var targetpos = dragbtn.position(), isoveron = false, ico_name_h = that.appBtnInfo.nameHeight, dif = that.appBtnInfo.space, desksize = that.size;
        /*检测是否重叠*/
        var _checkCd = function (id, newpos) {
            var iscd = false;
            that.appBtns.each(function () {
                var other = $(this);
                if (other.attr("id") == id) return true;
                var otherpos = other.position(),
                       minleft = otherpos.left - other.width() - dif, //最小左边界
                       maxleft = otherpos.left + other.width() + dif; //最大左边界
                mintop = otherpos.top - other.height() - dif, //最小上边界
                       maxtop = otherpos.top + other.height() + dif; //最大上边界
                iscd_lr = newpos.left > minleft && newpos.left < maxleft, //可能发生左右重叠
                       iscd_tb = newpos.top > mintop && newpos.top < maxtop; //可能发生上下重叠
                iscd = iscd_lr && iscd_tb;
                if (iscd)
                    return !iscd;
            });
            return iscd;
        }
        /*布局桌面按钮*/
        that.appBtns.each(function () {
            var other = $(this);
            if (other.attr("id") == dragbtn.attr("id")) return true;
            var otherpos = other.position(),
                   baksourcepos = { left: dragbtn.sourcePos.left, top: dragbtn.sourcePos.top },
                   minleft = otherpos.left - other.width() - dif, //最小左边界
                   maxleft = otherpos.left + other.width() + dif; //最大左边界
            mintop = otherpos.top - other.height() - dif, //最小上边界
                   maxtop = otherpos.top + other.height() + dif, //最大上边界
                   iscd_lr = targetpos.left > minleft && targetpos.left < maxleft, //可能发生左右重叠
                   iscd_tb = targetpos.top > mintop && targetpos.top < maxtop, //可能发生上下重叠
                   min_cross_left = that.appBtnInfo.startPos.left, //最小左边距
                   max_cross_left = desksize.w - that.appBtnInfo.outerWidth, //最大左边距
                   min_cross_top = that.appBtnInfo.startPos.top, //做小上边界
                   max_cross_top = desksize.h - that.appBtnInfo.outerHeight, //最大上边距
                   iscross_l = targetpos.left < min_cross_left;
            iscross_r = targetpos.left > max_cross_left;
            iscross_lr = iscross_l || iscross_r, //越过左右边界
                   iscross_t = targetpos.top < min_cross_top;
            iscross_b = targetpos.top > max_cross_top;
            iscross_tb = iscross_t || iscross_b, //越过上下边界
                   iscd = iscd_lr && iscd_tb, //重叠
                   iscross = iscross_lr || iscross_tb, //越界
                   isoveron = iscd || iscross,
                   center_pos_x = otherpos.left + (that.appBtnInfo.outerWidth - dif) / 2, //中线点x
                   center_pos_y = otherpos.top + (that.appBtnInfo.outerHeight - ico_name_h - dif) / 2; //中线点y
            if (iscd) {//重叠
                var target_left = targetpos.left + 12, target_top = targetpos.top;
                if (target_left > center_pos_x && target_top > center_pos_y)//下方，第四象限
                {
                    dragbtn.sourcePos.left = otherpos.left;
                    dragbtn.sourcePos.top = otherpos.top + that.appBtnInfo.outerHeight;
                }
                else if (target_left < center_pos_x && target_top < center_pos_y)//上方，第二象限
                {
                    dragbtn.sourcePos.left = otherpos.left;
                    dragbtn.sourcePos.top = otherpos.top - that.appBtnInfo.outerHeight;
                }
                else if (target_left < center_pos_x && target_top > center_pos_y)//左方，第三象限
                {
                    dragbtn.sourcePos.left = otherpos.left - that.appBtnInfo.outerWidth;
                    dragbtn.sourcePos.top = otherpos.top;
                }
                else if (target_left > center_pos_x && target_top < center_pos_y)//右方，第一象限
                {
                    dragbtn.sourcePos.left = otherpos.left + that.appBtnInfo.outerWidth;
                    dragbtn.sourcePos.top = otherpos.top;
                }
            }
            if (iscross) {//越界
                if (iscross_lr) {
                    if (iscross_l) { dragbtn.sourcePos.left = min_cross_left; }
                    else if (iscross_r) { dragbtn.sourcePos.left = max_cross_left; }
                    dragbtn.sourcePos.top = targetpos.top;
                }
                else if (iscross_tb) {
                    if (iscross_t) { dragbtn.sourcePos.top = min_cross_top; }
                    else if (iscross_b) { dragbtn.sourcePos.top = max_cross_top; }
                    dragbtn.sourcePos.left = targetpos.left;
                }
            }
            if (isoveron) {
                //处理后判断执行后是否会发生越界现象
                if (dragbtn.sourcePos.left < min_cross_left || dragbtn.sourcePos.left > max_cross_left ||
                     dragbtn.sourcePos.top < min_cross_top || dragbtn.sourcePos.top > max_cross_top || _checkCd(dragbtn.attr("id"), dragbtn.sourcePos)) { dragbtn.sourcePos = baksourcepos; }
                return !isoveron;
            }
        })
        return isoveron;
    },
    /**
    * 桌面对象
    * @method _DeskTop
    */
    _DeskTop = function (langlist) {
        var that = this;
        that.lang = langlist || { WEEK_LIST: "星期日,星期一,星期二,星期三,星期四,星期五,星期六" };
        that.domElement = that.ele("desktop");
        if (!that.domElement) return;
        that.desk = $("#main"),
        that.appBtns = that.desk.find(".app-btn"),
        that.topBar = that.ele("desktop_bar", true),
        that.wrapper = this.ele("wrapper"),
        that.selectRegion = this.ele("select_region", true),
        that.topBarHeight = 24; //任务栏实际高度
        var _firstBtn = that.appBtns.eq(0);
        that.appBtnInfo = {
            nameHeight: 23, //appbtn name 实际高度
            width: 80, //appbtn 实际宽度
            space: 10, //appbtn 间隔距离
            startPos: _firstBtn.position()
        };
        that.appBtnInfo.outerWidth = _firstBtn.outerWidth() + that.appBtnInfo.space,
        that.appBtnInfo.outerHeight = _firstBtn.outerHeight() + that.appBtnInfo.nameHeight + that.appBtnInfo.space;
        that.deskLayout();
        $(window).resize(function () {
            that.deskLayout();
        });
        that._drag_boxs = [],
        that._drag_oldPos = {},
        that._region_selecting = false;
        var opened_wins_ul, win_btns_intopbar_box = $(".desktop-bar-right-activbox"); //桌面头部 可以代替当前操作窗口的操作按钮容器
        win_btns_intopbar_box.find(".win-close,.win-size-max,.win-size-min").on("click", function () {
            $(this).data("btnClickEvent")();
        })
        /**
        * 检测是否在最大化时设置或清除桌面标题部分
        * @method checkToSetOrEmptyDeskTitleBar
        */
        that.checkToSetOrEmptyDeskTitleBar = function () {
            var barhtml = $(".desktop-bar-mid"), zindex = 0, tmp = {};
            for (var key in that.dialog.dialogCache) {
                var dialog = that.dialog.dialogCache[key];
                dialog.info.title.html(dialog.args.title);
                dialog.info.maxBtn.show();
                dialog.info.minBtn.show();
                dialog.info.closeBtn.show();
                if (dialog.currentMode != 'max' || dialog.window.position().top > 0) continue;
                var curzindex = dialog.window.css("z-index");
                tmp[curzindex] = dialog;
                zindex = Math.max(curzindex, zindex)
            }
            if (zindex) {
                var topdialog = tmp[zindex];
                if (topdialog) {
                    var title = topdialog.args.title;
                    barhtml.text(title);
                    topdialog.info.title.empty();
                    topdialog.info.maxBtn.hide();
                    topdialog.info.minBtn.hide();
                    topdialog.info.closeBtn.hide();
                    win_btns_intopbar_box.show();

                    win_btns_intopbar_box.find(".win-close").data("btnClickEvent", function () {
                        topdialog.info.closeBtn.trigger("click");
                        $(this).removeData("btnClickEvent");
                    })
                    win_btns_intopbar_box.find(".win-size-max").data("btnClickEvent", function () {
                        topdialog.activeTo("normal");
                        $(this).removeData("btnClickEvent");
                    })
                    win_btns_intopbar_box.find(".win-size-min").data("btnClickEvent", function () {
                        topdialog.info.minBtn.trigger("click");
                        $(this).removeData("btnClickEvent");
                    })
                }
            }
            else {
                win_btns_intopbar_box.hide();
                barhtml.empty();
            }
        }
        /*为左面提供的对话框插件*/
        that.dialog = new EzDialog(
        /*configs*/{targetTag: "", topZindex: 12 }, //窗口基本配置信息
        /*perOpenEventHandler*/"", //窗口打开时调用
        /*openedEventHandler*/function (dialog, args) {//打开后调用，关于“窗口”菜单的配置
            var id = "openedWins_list";
            var li_id = id + "_" + args.winid;
            opened_wins_ul = $("#" + id);
            var opened_win_li = $("#" + li_id);
            if (!opened_wins_ul.is("ul")) {
                opened_wins_ul = $("<ul id='" + id + "' class='desktop-bar-child' style='width:280px; overflow:hidden;'></ul>");
                $("#openedWins").parent().append(opened_wins_ul);
            }
            if (!opened_win_li.is("li")) {
                var title = args.src; //args.title||"打开窗口";
                var opened_win_li = $(((opened_wins_ul.find("li").size() >= 1) ? "<li id='" + li_id + "_underline' class='desktop-bar-menu-split'></li>" : "")
                    + "<li id='" + li_id + "' class='desktop-bar-child-menu' title='" + title + "' style='overflow:hidden;' >" +
                    "<a href='javascript:void(0)'>" + title + "</a></li>");
                opened_win_li.data(WINDIW_ARGS, args);
                opened_wins_ul.append(opened_win_li);
                opened_win_li.on("click", function () { dialog.activeTo("keep-mode"); });
            }
        },
        /*closedEventHanlder*/function (dialog, args) {//窗口关闭后调用
            if (opened_wins_ul.find("li").size() == 1) opened_wins_ul.remove();
            else { $("#openedWins_list_" + args.winid).remove(); $("#openedWins_list_" + args.winid + "_underline").remove(); }
            that.checkToSetOrEmptyDeskTitleBar();
        },
        /*minEventHandler*/function () {//窗口最小化时调用
            that.checkToSetOrEmptyDeskTitleBar();
        },
        /*resizeChangeEventHandler*/function (dialog, args) {//窗口尺寸变化时调用
            var barhtml = $(".desktop-bar-mid");
            if (dialog.currentMode == "max") {
                barhtml.text(args.title);
                dialog.info.title.empty();
                //var mtop = args.hassearch ? 43 : 24;
                //dialog.info.menuSwitch.css("margin-top", mtop);
            }
            else {
                dialog.info.title.html(args.title);
                //dialog.info.menuSwitch.css("margin-top", 0);
            }
            that.checkToSetOrEmptyDeskTitleBar();
        },
        /*windowMovingEventHandler*/function () {//窗口移动中调用
            that.checkToSetOrEmptyDeskTitleBar();
        },
        /*structWindowData*/function (key) {//构造窗口菜单数据时调用
            var target_a = $("#desk_top_menu_bar li").eq(0).find("a[data-src='" + key + "']");
            var li = target_a.parent();
            var olis = li.nextAll();
            if (olis.size() == 0) return;
            var title_a = li.parent().next();
            var menus = [];
            var menu_group = { title: title_a.text(), children: [{ text: target_a.text(), src: key}] };
            olis.each(function (i) {
                var _li = $(this);
                if (_li.find("ul.desktop-bar-child2").size() == 0) {
                    var txt = $.trim(_li.text());
                    if(txt!="") {
                        menu_group.children[i + 1] = {text: _li.text(), src: _li.find("a").eq(0).data("src")};
                    }
                }
            })
            menus.push(menu_group);
            return menus;
        }
        );

        /*任务栏系统时间*/
        (function (that) {
            var _a_systemtime = that.ele("systemtime"); /*任务栏系统时间的DomElement*/
            setInterval(function () {
                var _date = new Date();
                var _y = _date.getFullYear();
                var _m = _date.getMonth() + 1;
                var _d = _date.getDate();
                var _dd = _date.getDay();
                var _hh = _date.getHours();
                var _mm = _date.getMinutes();
                var _ss = _date.getSeconds();
                var _y_m_d = _y + "/" + _m + "/" + _d;
                var _h_f_m = _hh + ":" + (_mm < 10 ? "0" + _mm : _mm) + ":" + (_ss < 10 ? "0" + _ss : _ss);
                var weekday = that.lang.WEEK_LIST.split(",")
                _a_systemtime.setAttribute("title", _y_m_d + " " + weekday[_dd]);
                _a_systemtime.innerText = _h_f_m;
            }, 1000);
        })(that);
        /*桌面图标拖动与选取事件*/
        (function (that) {
            var select_region_state = {},
                tmp_arr_item_exits = {}, /*检测按钮是否已经添加过的容器*/
                _mouseupHandler = function (event, args) {
                    var boxs = that._drag_boxs;
                    if (typeof (boxs) != 'undefined' && (boxs.length && boxs.length > 0)) {
                        for (var index in boxs) {
                            var box = boxs[index]
                            if (!box.hasClass("window"))/*按钮时进行处理*/
                            {
                                if (_checkApp_btn_OverFlow.call(that, box)) {
                                    box.stop().animate({ "left": box.sourcePos.left, "top": box.sourcePos.top });
                                }
                                else {
                                    box.sourcePos = box.position();
                                }
                            }
                            else if (box.position().top < 0) {
                                box.stop().animate({ "top": 0 });
                            }
                        }
                    }

                    that.selectRegion.css({ "left": 0, "top": 0, "height": "0px", width: "0px" }).hide();
                    select_region_state.startPoint = null;
                    that._region_selecting = false;
                },
                _checkRegionedAppbtn = function () {
                    var regionPos = that.selectRegion.position(),
                      region_min_left = regionPos.left,
                      region_max_left = regionPos.left + that.selectRegion.width(),
                      region_min_top = regionPos.top,
                      region_max_top = regionPos.top + that.selectRegion.height();
                    that.appBtns.each(function () {
                        var current = $(this),
                           btnPos = current.position(),
                           right = that.appBtnInfo.outerWidth + btnPos.left,
                           bottom = that.appBtnInfo.outerHeight + btnPos.top;

                        var isregionIn = (btnPos.left > region_min_left && btnPos.left < region_max_left || right > region_min_left && right < region_max_left) &&
                           (btnPos.top > region_min_top && btnPos.top < region_max_top || btnPos.top < region_min_top && region_min_top < bottom)

                        if (tmp_arr_item_exits[current.attr("id")] != "exits" && isregionIn) {/*未被记录且已被选中*/
                            current.removeClass("selected").addClass("selected").css("z-index", "2");
                            current.__proto__.sourcePos = current.position();
                            that._drag_boxs.push(current);
                            tmp_arr_item_exits[current.attr("id")] = "exits";
                        }
                        else if (tmp_arr_item_exits[current.attr("id")] == "exits" && !isregionIn) {/*未被选中且已被记录*/
                            current.removeClass("selected").css("z-index", "1");
                            tmp_arr_item_exits[current.attr("id")] = null;
                            delete tmp_arr_item_exits[current.attr("id")];
                            for (var i = 0; i < that._drag_boxs.length; i++) {
                                if (that._drag_boxs[i] && that._drag_boxs[i].attr("id") == current.attr("id")) {
                                    that._drag_boxs[i] = null;
                                    delete that._drag_boxs[i];
                                    break;
                                }
                            }
                        }
                    })
                };
            that.topBar.on("mouseup", _mouseupHandler);

            that.appBtns.on("dblclick", function (event) {

                var _iswin = $(this).data("iswin"), _src = $(this).data("src");
                if (_iswin) {
                    var appbtn = $(this);
                    var args = appbtn.data(WINDIW_ARGS);
                    if (!args) {
                        var _winsize = appbtn.data("win-size"),
                                _hasmenu = appbtn.data("win-hasmenu"),
                                _hassearch = appbtn.data("win-hassearch"), _title = appbtn.data("win-title");
                        if (typeof (_hasmenu) == 'string') _hasmenu = _hasmenu.toLowerCase() == 'true';
                        if (typeof (_hassearch) == 'string') _hassearch = _hassearch.toLowerCase() == 'true';
                        var _width = appbtn.data("win-width");
                        var _height = appbtn.data("win-height");
                        args = {
                            src: _src,
                            dropEnable: true,
                            mode: _winsize,
                            hasmenu: _hasmenu,
                            hassearch: _hassearch,
                            title: _title,
                            fromBtn:appbtn
                        };
                        if (_width && typeof (_width) == "number" && _width > 0) {
                            args.width = _width;
                        }
                        if (_height && typeof (_height) == "number" && _height > 0) {
                            args.height = _height;
                        }
                        var handler = that.dialog.openWin(args,appbtn);
                        appbtn.data(WINDIW_ARGS, handler.args)
                    }
                    else {
                        that.dialog.openWin(args,appbtn)
                    }

                } else {
                    top.location.href = _src;
                }

            });
            $("body").onDragListening(
               function (event, args) {
                   that._region_selecting = !event.dragEnable;
                   if (that._region_selecting) {
                       that._drag_boxs = [];
                       tmp_arr_item_exits = {};
                       that.selectRegion.css({ "left": event.clientX, "top": event.clientY, "height": "1px", width: "1px" }).show();
                       that.appBtns.removeClass("selected").css("z-index", "1");
                       select_region_state.startPoint = { x: event.clientX, y: event.clientY };

                   }
                   else {
                       var _iswin = args && args.target && args.target.hasClass("window");
                       that._drag_boxs = that._drag_boxs.length > 1 ? that._drag_boxs : [];
                       if (!_iswin) {
                           that.appBtns.removeClass("selected").css("z-index", "1");
                       }
                       else {
                           $("body .window").css("z-index", "10");
                       }
                       that._drag_boxs = that._drag_boxs && that._drag_boxs.length > 0 ? that._drag_boxs : [args.target];
                       for (var i in that._drag_boxs) {
                           var _movingbox = that._drag_boxs[i];
                           _movingbox.addClass("selected").css("z-index", _iswin ? "12" : "2");
                           _movingbox.__proto__.sourcePos = _movingbox.position();
                       }
                   }
               },
               function (event, args) {
                   if (that._region_selecting) {//正在执行选取操作
                       var x = event.clientX, y = event.clientY
                       var _regionpos = that.selectRegion.position();
                       var _h = 0, _w = 0;
                       if (x > select_region_state.startPoint.x && y < select_region_state.startPoint.y)//右上角
                       {
                           that.selectRegion.css("top", y);
                           _h = Math.abs(y - select_region_state.startPoint.y);
                           _w = Math.abs(x - _regionpos.left);
                       }
                       else if (x < select_region_state.startPoint.x && y < select_region_state.startPoint.y) //左上角
                       {
                           that.selectRegion.css("top", y);
                           that.selectRegion.css("left", x);
                           _h = Math.abs(y - select_region_state.startPoint.y);
                           _w = Math.abs(x - select_region_state.startPoint.x);
                       }
                       else if (x < select_region_state.startPoint.x && y > select_region_state.startPoint.y)//左下角
                       {
                           that.selectRegion.css("left", x);
                           _h = Math.abs(y - select_region_state.startPoint.y);
                           _w = Math.abs(x - select_region_state.startPoint.x);
                       }
                       else //右下角
                       {
                           _w = Math.abs(x - _regionpos.left);
                           _h = Math.abs(y - _regionpos.top);
                       }
                       that.selectRegion.css({ "width": _w, "height": _h });
                       if (that._region_selecting) _checkRegionedAppbtn();
                   }
                   else if (args) {
                       var btns = that._drag_boxs;
                       for (var index in btns) {
                           var btn = btns[index];
                           if (btn.attr("id") == args.target.attr("id")) continue;
                           var newleft = args.moveLenX + btn.position().left;
                           var newtop = args.moveLenY + btn.position().top;
                           btn.css({ "left": newleft, "top": newtop });
                       }
                   }
               },
               _mouseupHandler);
        })(that);

        /**
        * 设置非跨域窗口打开时的标题，本方法供iframe调用页面调用
        * @method setCurrentOpenedWindowTitle
        * @param {string} key 取得一个窗口iframe的key 它为一个iframe 应用的src
        * @param {string} title 将要设置的标题
        * @param {string} width 将要设置的标题
        * @param {string} height 将要设置的标题
        */
        that.setCurrentOpenedWindowTitle = function (key, title) {
            var set_EzDialogTitle_callback = $("body").find("iframe[src='" + key + "']").data("set_EzDialogTitle_callback");
            if (typeof (set_EzDialogTitle_callback) == "function") set_EzDialogTitle_callback(title);
            //that.checkToSetOrEmptyDeskTitleBar();
        }
        /**
        * 页面请求窗口显示菜单
        * @method setEzDialogMenu
        * @param {string} winid 窗口ID
        * @param {json}   menus 菜单描述对象
        */
        that.setEzDialogMenu = function (winid, menus) {
            var EzDialog = that.dialog.findEzDialog(winid);
            if (EzDialog) {
                EzDialog.args.hasmenu = true;
                EzDialog.createMenu(menus, true);
            }
        }
        /**
        * 设置对话框参数
        * @method setEzDialogMenu
        * @param {string} winid 窗口ID
        * @param {json}   params 菜单描述对象
        */
        that.setEzDialogParam = function (winid, params) {
            var EzDialog = that.dialog.findEzDialog(winid);
            if (EzDialog && params) {
                EzDialog.setParam(params)
            }
        }
        /**
        * 获取对话框参数
        * @method getEzDialogParam
        * @param {string} winid 窗口ID
        */
        that.getEzDialogParam = function (winid) {
            var EzDialog = that.dialog.findEzDialog(winid);
            if (EzDialog) {
                return EzDialog.getParam();
            }
        }
        /**
        * 设置对话框工具条
        * @method setEzDialogToolBar
        * @param {string} winid 窗口ID
        * @param {json}   params 工具条描述对象
        */
        that.setEzDialogToolBar = function (winid, params) {
            var EzDialog = that.dialog.findEzDialog(winid);
            if (EzDialog && params) {
                return EzDialog.setToolBar(params);
            }
        }
    };
    /*实例原型成员列表*/
    _DeskTop.prototype = {
        createEle: function (tagName, classname, id, csstext, attrs, parent) {
            classname = classname || undefined;
            id = id || undefined;
            csstext = csstext || undefined;
            var domele = document.createElement(tagName);
            if (typeof (id) != "undefined") domele.id = id;
            if (typeof (classname) != "undefined") domele.className = classname;
            if (typeof (csstext) != "undefined") domele.style.cssText = csstext;
            if (attrs && typeof (attrs) == "object") {
                for (var key in attrs) {
                    domele.setAttribute(key, attrs[key]);
                }
            }
            if (parent) {
                parent.appendChild(domele);
            }
            return domele;
        },
        ele: function (id, isjq) {
            if (isjq) return $("#" + id);
            return document.getElementById(id);
        },
        deskLayout: function () {
            var that = this, rownum = 0, column = 0;
            if (!that.appBtnInfo.startPos) return;
            that.size = { h: $(window).height(), w: $(window).width() };
            that.desk.height(that.size.h).width(that.size.w);
            that.grid = {
                rows: (that.size.h - that.appBtnInfo.startPos.top) / that.appBtnInfo.outerHeight - 1,
                cols: (that.size.w - that.appBtnInfo.startPos.left) / that.appBtnInfo.outerWidth - 1
            };
            that.appBtns.each(function (i) {
                var shortcut = $(this);
                var top = that.appBtnInfo.startPos.top + rownum * that.appBtnInfo.outerHeight;
                var left = that.appBtnInfo.startPos.left + column * that.appBtnInfo.outerWidth;
                shortcut.css({ "left": left, "top": top });
                if (rownum < that.grid.rows) {
                    rownum++;
                }
                else {
                    column++;
                    rownum = 0;
                }
                if (column > that.grid.cols) {
                    rownum++;
                    column = 0;
                }
            })
        }
    };
    /**
    * 扩展Jquery静态对象，新增桌面实例对象的创建方法
    * @method $.startDeskTop
    * @param {json} langlist,配置信息，格式请参见文档
    * */
    $.startDeskTop = function (langlist) {
        var desk = new _DeskTop(langlist);
        /*绑定头部菜单的窗口打开事件*/
        $("#desktop_bar a[data-iswin='True']").on("click", function () {
            var btn = $(this);
            var src = btn.data("src");
            if (src) {
                var args = btn.data(WINDIW_ARGS) || $("#main div[data-src='" + src + "']").data(WINDIW_ARGS);
                if (!args) {
                    var _hasmenu = btn.data("win-hasmenu"),
                       _hassearch = btn.data("win-hassearch"), _title = btn.data("win-title");

                    if (typeof (_hasmenu) == 'string') _hasmenu = _hasmenu.toLowerCase() == 'true';
                    if (typeof (_hassearch) == 'string') _hassearch = _hassearch.toLowerCase() == 'true';
                    args = { src: src, dropEnable: true, mode: btn.data("win-size"), hasmenu: _hasmenu, hassearch: _hassearch, title: _title };
                    var _width = btn.data("win-width");
                    var _height = btn.data("win-height");
                    if (_width && typeof (_width) == "number" && _width > 0) {
                        args.width = _width;
                    }
                    if (_height && typeof (_height) == "number" && _height > 0) {
                        args.height = _height;
                    }
                    var handler = desk.dialog.openWin(args,btn);
                    btn.data(WINDIW_ARGS, handler.args);
                }
                else {
                    desk.dialog.openWin(args,btn)
                }
            }
            return false;
        })
        /*设置头部菜单的展示效果*/
        $("#desk_top_menu_bar li").on("mouseover", function () { $(this).find(">ul").show(); }).on("mouseout", function () { $(this).find(">ul").hide(); });
        return desk;
    }
})(jQuery, EzDialog)