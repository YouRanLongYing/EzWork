/*!
 * DialogDrag& For Product JavascriptLibrary
 * EzDialog v1.0
 * Jquery v1.7.2
 * User: kongjing/endfalse@163.com
 * Date: 2014.08.08
 * Include Jquery (http://www.jquery.com/)
 */
(function ($,Ez) {
    var preventDefault = function stopDefault(e) {
        if (e && e.preventDefault) {//如果是FF下执行这个
            e.preventDefault();
        } else {
            window.event.returnValue = false; //如果是IE下执行这个
        }
        return false;
    };
    $.fn.playAnimateScale = function (ops, finishfn, speed) {
        var scaleto = typeof (ops) == 'number' ? ops : ops.scale;
        speed = speed == "slow" ? 1200 : (speed || 800);
        var scale = 0;
        var obj = $(this);
        var obj_pos = obj.position();
        var curScale = document.body.offsetWidth / window.screen.availWidth;
        var normal_scale = 1;
        if (scaleto == 1) {
            normal_scale = obj.data("scale");
        }
        var step = Math.abs(normal_scale - scaleto) / speed;
        var pos_step_left;
        var pos_step_top;
        if (ops.left) {
            pos_step_left = Math.abs(obj_pos.left + (obj.width() * (normal_scale - scaleto)) - ops.left) / speed;
        }
        if (ops.top) {
            pos_step_top = Math.abs(obj_pos.top + obj.height() * (normal_scale - scaleto) - ops.top) / speed;
        }
        var startstamp = new Date().getTime();
        var timer = setInterval(function () {
            var nowstamp = new Date().getTime();
            var diff = nowstamp - startstamp;
            if (diff < speed) {
                if (scaleto > normal_scale) { scale = normal_scale + step * diff; }
                else { scale = normal_scale - step * diff; }

                obj.css({ "-moz-transform": "scale(" + scale + ")", "-webkit-transform": "scale(" + scale + ")" });
                obj.data("scale", scale);

                if (pos_step_left) {
                    var _l = obj_pos.left > ops.left ? (obj_pos.left - pos_step_left * diff) : (obj_pos.left + pos_step_left * diff);
                    obj.css({ "left": _l });
                }
                if (pos_step_top) {
                    var _t = obj_pos.top > ops.top ? (obj_pos.top - pos_step_top * diff) : (obj_pos.top + pos_step_top * diff);
                    obj.css({ "top": _t });
                }
            }
            else {
                clearInterval(timer);
                if (scaleto == 1) {
                    obj.css({ "-moz-transform": "scale(" + curScale + ")", "-webkit-transform": "scale(" + curScale + ")" });
                    obj.data("scale", 1);
                }
                if (typeof (finishfn) == 'function') finishfn();
            }
        }, 13);
        return obj;
    };
    var _topZindex = 12, //顶级层叠
        _lessToMaxMode = 24; //当窗口在拖动时，鼠标的Y位置小于此数值则自动式窗口到最大模式
    /**
    * EzDialog类
    * @method _EzDialog
    * @param {实例此模块需要的配置信息,格式请参见文档,json}  config
    * @param {窗口打开前调用,function} perOpenEventHandler
    * @param {窗口打开后调用,function} openedEventHandler
    * @param {窗口关闭后调用,function} closedEventHanlder
    * @param {窗口最小化时调用,function} minEventHandler
    * @param {窗口尺寸变化时调用,function} resizeChangeEventHandler
    * @param {窗体配拖动时调用,function} windowMoveEventHandler
    * @param {构造窗口菜单所需数据时调用,function} structWindowData
    * */
    var _EzDialog = function (config, perOpenEventHandler, openedEventHandler, closedEventHanlder, minEventHandler, resizeChangeEventHandler, windowMoveEventHandler, structWindowData) {
        var that = this;
        that.config = $.extend({}, config, { target: "body", topZindex: 12, useanimate: true, openSpeed: "slow", closeSpeed: "fast", resizeSpeed: "slow", lessToMaxMode: 24 }); // targetTag;
        _topZindex = that.config.topZindex;
        that.runtime = that._getRandomStr();
        that.dialogCache = {};
        that._dialogCounter = 0;
        //监视窗口变化
        $(window).on("resize", function () {
            for (var key in that.dialogCache) {
                var _win = that.dialogCache[key];
                _win.setWillMode("keep-mode", null);
            }
        });
        /**
        * EzDialog core 模块
        * @param {json,打开窗口的配置参数，格式请参见文档}
        * */
        var _OpenWin = function (opts) {
            var win = this;
            win.args = opts;
            win.id = win.args.winid;
            //窗口打开时需要的参数，其中包括 播放的尺寸和位置等
            win._animateParams = {};
            win.currentMode = (opts.mode || "normal").toLowerCase();
            win.info = that._getDiolagTemplate(opts, win, structWindowData);
            win.window = win.info.window;
            var canopen = true;
            if (typeof (perOpenEventHandler) == "function") {
                canopen = perOpenEventHandler(win, opts);
                if (typeof (canopen) == 'undefined') canopen = true;
            }
            // 一般用于在启动前检测是否满足启动条件，默认为true
            if (!(typeof (canopen) == 'undefined' ? true : canopen)) return;
            win._eventBindAll();
            if (typeof (structWindowData) == 'function') {
                var _menus = structWindowData(win.args.src); /*?待最终确定?*/
                if (_menus) win.args.hasmenu = true;
                this.createMenu(_menus || [], false, "sys");
            }
            /*设置到即将显示的尺寸*/
            //win.setWillMode(this.currentMode, null);
        };
        _OpenWin.prototype = {
            /**
            * 设置窗口在显示动画所需的参数信息，私有方法，不允许外部调用或修改
            * @method _eventBindAll
            * @param {int,显示的宽} width
            * @param {int,显示的高} height
            * @param {int,左边距位置（X）} left
            * @param {int,上边距位置(Y)} top
            * @param {string,当前要显示的模式min|normal|max} mode
            */
            _setWinSizeAndPosition: function (width, height, left, top, mode) {
                var wininfo = this.info;
                var _keep_mode_w = wininfo.window.width(),
                    _keep_mode_h = wininfo.window.height(),
                    _keep_mode_left = wininfo.window.position().left,
                    _keep_mode_top = wininfo.window.position().top;

                if (mode != "toolbar") {
                    this._animateParams = {
                        mode: mode,
                        speed: "fast",
                        width: 0, height: 0, left: 0, top: 0,
                        body: { width: 0, height: 0, left: 0, top: 0 },
                        menu: { width: 0, height: 0, left: 0, top: 0 },
                        viewPort: { width: 0, height: 0, left: 0, top: 0 },
                        iframe: { width: 0, height: 0, left: 0, top: 0 }
                    };
                    var _setProperty = function (sourceObj, args) {
                        var keys = ["width", "height", "left", "top"];
                        for (index in keys) {
                            sourceObj[keys[index]] = args[index];
                        }
                    }
                    var _header_h = wininfo.header.height() || 35,
                        _left_space_percent = wininfo.menu.outerWidth() / width,
                        _menu_cur_width = width * _left_space_percent - 10,
                        _hadMenuSpace = (this.args.hasmenu && !wininfo.menu.is(":hidden")), _H = height - _header_h;
                    if (mode == "toolbar") {
                        _H = _H - wininfo.headerBottomBar.height();
                    }
                    _setProperty(this._animateParams, [width, height, left, top]);
                    _setProperty(this._animateParams["menu"], [0, _H, _hadMenuSpace ? 0 : (-_menu_cur_width), 0]);
                    _setProperty(this._animateParams["body"], [0, _H, 0, 0]);
                    _setProperty(this._animateParams["viewPort"], [_hadMenuSpace ? width * (1 - _left_space_percent) : width, _H, _hadMenuSpace ? _menu_cur_width : 0, 0]);
                    _setProperty(this._animateParams["iframe"], [0, _H, 0, 0]);
                    this.currentMode = mode;
                }
                else if (this._animateParams) {
                    this.currentMode = this.args.mode;
                    this._animateParams["menu"].height =
                    this._animateParams["menu"].height =
                    this._animateParams["viewPort"].height =
                    this._animateParams["iframe"].height =
                    this._animateParams["menu"].height - wininfo.headerBottomBar.height();
                }
                if (mode == "min") {
                    /*只有存在可显示尺寸的情况才记录尺寸*/
                    this._keep_mode = {
                        mode: this.currentMode,
                        w: _keep_mode_w,
                        h: _keep_mode_h,
                        left: _keep_mode_left,
                        top: _keep_mode_top
                    };
                }

                that.dialogCache[this.id] = this;
                //重新定义层次
                $("body .window").css("z-index", that.config.topZindex - 2);
                this.window.css("z-index", that.config.topZindex);
            },
            /**
            * 在实例化窗口时绑定所有的事件，私有方法，不允许外部调用或修改
            * @method _eventBindAll
            */
            _eventBindAll: function () {
                var win = this;
                //注册拖动改变尺寸的事件
                win.window.on("mousemove", function () {
                    if (!win.window.resizing) {
                        var pos = win.window.position();
                        var width = win.window.width();
                        var height = win.window.height();
                        var left = width + pos.left;
                        var top = pos.top - height;
                        var inRegion = (event.clientX - pos.left) <= width && (event.clientY - pos.top) <= top;
                        if (inRegion) {   //显示可改变尺寸的方向
                            if (Math.abs(event.clientX - left) <= 1 && Math.abs(event.clientY - top) > 1) {
                                win._moveResizeDirection = "right";
                            }
                            else if (Math.abs(event.clientY - top) <= 1 && Math.abs(event.clientX - left) > 1) {
                                win._moveResizeDirection = "down";
                            }
                            else if (Math.abs(event.clientY - top) <= 2 && Math.abs(event.clientX - left) <= 2) {
                                win._moveResizeDirection = "rightDown";
                            }
                        }
                    } else {
                        alert(win._moveResizeDirection);
                    }
                }).on("mouseDown", function () {
                    win.window.resizing = true;
                }).on("mouseup", function () {
                    win.window.resizing = false;
                });
                //关闭窗口
                win.info.closeBtn.on("click", function () { win.close(); });
                //双击切换窗口尺寸和位置
                var _mouse_down_ = false;
                win.info.header.on("dblclick", function (event) {
                    win.activeTo(win.currentMode == "normal" ? "max" : "normal", null, true);
                }).on("mousedown", function (event) {
                    _mouse_down_ = true;
                }).on("mouseup", function (event) {
                    _mouse_down_ = false;
                });

                win.info.maxBtn.on("click", function (event) {
                    win.activeTo("max", event, true);
                });
                win.info.minBtn.on("click", function (event) {
                    win.activeTo("min", event, true);
                    win.active = false;
                });
                win.info.menuSwitch.on("click", function () {
                    var wininfo = win.info;
                    if (!wininfo.menu.data("init-width")) {
                        wininfo.menu.data("init-width", wininfo.menu.outerWidth());
                    }
                    var win_width = wininfo.window.width();
                    var win_height = wininfo.window.height();
                    var _h = win_height - (wininfo.header.height() || 35);

                    var _left_space_percent = (wininfo.menu.data("init-width") || wininfo.menu.outerWidth()) / wininfo.window.outerWidth();
                    var menu_cur_width = win_width * _left_space_percent;

                    if (wininfo.menu.is(":hidden")) {
                        wininfo.menu.show().stop().animate({ "left": 0, "height": _h }, "fast");
                        wininfo.viewPort.stop().animate({ "width": win_width * (1 - _left_space_percent), "left": menu_cur_width });
                    }
                    else {
                        wininfo.menu.stop().animate({ "left": -menu_cur_width, "height": _h }, "fast", function () {
                            $(this).hide();
                        });
                        wininfo.viewPort.stop().animate({ "width": win_width, "left": 0 }, "fast");
                    }
                });

                win.window.data("_mouseDownOnWinHandler", function (event, param) {

                });

                win.window.data("_mouseUpOnWinHandler", function (event, param) {

                });
                //向被拖动对象注入鼠标移动事件
                win.window.data("_moveWinHandler", function (event, param) {
                    if (!_mouse_down_) return;
                    if (event.clientY <= _lessToMaxMode && !win.autoToMaxActioned) {//24:桌面头部bar的高度
                        win.activeTo("max", event, false)
                        win.autoToMaxActioned = true;
                    }
                    else if (event.clientY > _lessToMaxMode && win.autoToMaxActioned) {
                        win.activeTo("normal", event, false)
                        win.autoToMaxActioned = false;
                    }
                    else if (win.currentMode == "normal") {//这是在更新窗口拖动后的位置
                        var _winpos = win.window.position();
                        win._normalSize = {};
                        win._normalSize.left = _winpos.left;
                        win._normalSize.top = _winpos.top;
                    }
                    if (typeof (windowMoveEventHandler) == "function") windowMoveEventHandler(win, win.args);
                });
            },
            /**
            * 创建窗口显示的菜单
            * @method createMenu
            * @param {json,包含菜单信息的json数据_格式请参见文档}   menus
            * @param {bool,true追加,false替换菜单容器}  append
            * @param {string,是否为系统菜单的表示_如果为系统则flag为sys否则为用户定义菜单即内嵌页面定义的菜单} flag
            */
            createMenu: function (menus, append, flag) {/*[{ title: "", children: [{}, {}, {}]}];*/
                if (!this.args.hasmenu) return;
                var wininfo = this.info;
                if (wininfo.menuSwitch.is(":hidden")) wininfo.menuSwitch.show();
                if (append && wininfo.menuBox.find("li[from = 'page_required_menu']").size() > 0) return;
                if (flag == "sys" && wininfo.menuBox.find("li[from = 'sys']").size() > 0) return;
                var attr = "";
                if (flag == "sys") {
                    attr = "sys";
                }
                else {
                    attr = "page_required_menu";
                }

                var htmlpool = [];
                if (menus && menus.length !== undefined && menus.length > 0) {
                    for (var i = 0; i < menus.length; i++) {
                        var group = menus[i];
                        htmlpool.push('<li class="menus-fun" from="' + attr + '">');
                        htmlpool.push('<a href="javascript:void(0)" class="fa fa-caret-down menus-fun-title">' + group.title + '</a>');
                        htmlpool.push('<ul class="menus-fun-children">');
                        var replace_keys = {};
                        for (var t = 0; t < group.children.length; t++) {
                            var link_A = group.children[t];
                            if (link_A) {
                                var src = link_A.src;
                                var text = link_A.text;
                                var EzDialogid = wininfo.window.attr("id");
                                var iframeid = wininfo.iframe.attr("id");
                                if (src && typeof (src) == "string") {
                                    src = src + (src.indexOf("?") >= 0 ? "&" : "?") + "EzDialogid=" + EzDialogid;
                                }
                                else {
                                    src = "javascript:void(0)";
                                }
                                var key = "";
                                if (link_A.rpKey !== undefined && typeof (link_A.rpKey) == 'object') {
                                    key = "rpk_" + that._getRandomStr() + "_" + t;
                                    replace_keys[key] = link_A.rpKey;
                                    key = "rpkey='" + key + "'";
                                    wininfo.menuBox.data("rpkey", replace_keys)
                                }
                                htmlpool.push('<li linkbtn="a"><a href="javascript:void(0)" data-src="' + src + '" class="fa fa-hand-o-right" target="' + iframeid + '" ' + key + '>' + text + '</a></li>');
                            }
                        }
                        htmlpool.push('</ul>');
                        htmlpool.push('</li>');
                    }
                    if (append) {
                        wininfo.menuBox.append(htmlpool.join(""));
                    }
                    else {
                        wininfo.menuBox.html(htmlpool.join(""));
                    }
                    wininfo.menuBox.find("a[data-src]").off("click").on("click", function () {
                        wininfo.iframe.attr("src", $(this).data("src"));
                        return false;
                    });
                }
            },
            /**
            * 设置窗口即将显示的模式（尺寸）
            * @method setWillMode
            * @param {string,要设置的显示模式_min|max|normal} mode
            * @param {object,鼠标事件对象_当在normal模式下_重定义窗口的位置} event
            */
            setWillMode: function (mode, event) {
                var _args = this.args, _window_size = { h: $(window).height(), w: $(window).width() }, to_width, to_height, to_left, to_top;
                switch (mode) {
                    case "min": /*最小化即隐藏*/
                        to_width = to_height = 0;
                        to_left = _window_size.w / 2;
                        to_top = _window_size.h / 2;
                        break;
                    case "normal": /*常规尺寸，即初始定义尺寸*/
                        if (!(typeof (_args.width) == 'number' && typeof (_args.height) == 'number' && _args.width > 0 && _args.height > 0)) _args.width = 720, _args.height = 380;
                        to_width = _args.width, to_height = _args.height;
                        if (this._normalSize && this._normalSize.left && this._normalSize && this._normalSize.top) {
                            to_left = this._normalSize.left;
                            to_top = this._normalSize.top;
                        }
                        else {
                            to_left = (_window_size.w - _args.width) / 2;
                            to_top = (_window_size.h - _args.height) / 2;
                        }
                        break;
                    case "keep-mode": /*backsize,静态尺寸，用于还原，只有在手动拖动时可更改尺寸，此处表示还原到静态尺寸显示*/
                        var __keepmode = this._keep_mode;
                        mode = __keepmode.mode, to_width = __keepmode.w, to_height = __keepmode.h, to_left = __keepmode.left, to_top = __keepmode.top;
                        break;
                    case "max": /*窗口到最大尺寸显示*/
                    default:
                        to_width = _window_size.w, to_height = _window_size.h, to_left = 0, to_top = 0;
                        break;
                }
                if (typeof (to_width) == 'number' && typeof (to_height) == 'number' && typeof (to_left) == 'number' && typeof (to_top) == 'number') {
                    if (event && mode == "normal") {/*修复拖动窗口还原到normal模式时 位置不正确的问题*/
                        var x = event.clientX;
                        var y = event.clientY;
                        var winpos = this.window.position();
                        to_top = winpos.top;
                        to_left = x - ((x - winpos.left) / this.window.width()) * to_width;
                        this._setWinSizeAndPosition(to_width, to_height, to_left, to_top, mode);
                    }
                    else {
                        this._setWinSizeAndPosition(to_width, to_height, to_left, to_top, mode);
                    }
                }
            },
            /**
            * 设置窗口参数
            * @method setParam
            * @param {json,包含要设置的参数信息的json数据_格式请参见文档} params
            */
            setParam: function (params) {
                if (typeof (param) == undefined) return;
                this.window.data("EzDialog_params", params);
                var replaceKey = this.info.menuBox.data("rpkey"); //rpkey 在createMenu 中进行了设置 起作用是拼接重复的 传递参数
                var replaced_paramstr = "";
                var _replaceParamKey = function (a) {
                    var canset = true;
                    var rpkey = a.attr("rpkey");
                    var _replaceKey = typeof (rpkey) == 'string' ? replaceKey[rpkey] : false;
                    if (_replaceKey) {
                        canset = typeof (_replaceKey) == 'object'
                        if (canset) {
                            $.each(_replaceKey, function (key, value) {
                                var regex = new RegExp(key + "=", "g");
                                if (regex.test(url_param_str)) {
                                    replaced_paramstr = url_param_str.replace(regex, value + "=");
                                }
                            });
                        }
                    }
                    else {
                        canset = false;
                    }
                    return canset;
                };
                var _checkAppendedParam = function (href) {
                    var canset = true;
                    $.each(params, function (key) {
                        if (typeof (key) == "string") {
                            var regex = new RegExp(key + "=", "g");
                            if (regex.test(href)) {
                                canset = false;
                                return false;
                            }
                        }
                    });
                    return canset;
                }
                var url_param = new Array();
                $.each(params, function (key, value) {
                    if (typeof (value) == "string" || typeof (value) == "number")
                        url_param.push(key + "=" + value);
                });
                var url_param_str = url_param.join("&");
                if (url_param_str != "")
                    this.info.menuBox.find("li[linkbtn='a'] a[href!='']").each(function () {
                        var a = $(this), href = a.attr("href"), canset = true;
                        if (href && href != "javascript:void(0)") {
                            canset = _checkAppendedParam(href);
                            var canrp = _replaceParamKey(a);
                            var url_param = "";
                            if (canset && canrp) {
                                url_param = replaced_paramstr;
                            }
                            else {
                                url_param = url_param_str;
                            }
                            a.attr("href", href + (href.indexOf("?") >= 0 ? "&" : "?") + url_param);
                        }
                    });
            },
            /**
            * 获取窗口参数信息的json数据
            * @method getParam
            */
            getParam: function () {
                return this.window.data("EzDialog_params");
            },
            /**
            * 设置窗口工具条,后期可以扩展功能
            * @method setToolBar
            */
            setToolBar: function (params) {
                var handler = this;
                if (typeof (params) == "function") {//为扩展之前临时定义，以防止后期参数丰富
                    params = { callback: params, auto: true };
                }
                handler.args.hassearch = true;
                handler.orgin.data("win-hassearch", true);
                handler.info.searchBtn.data("ref-src", handler.info.iframe.attr("src")).off("click").click(function () {
                    var cur_iframe_src = handler.info.iframe.attr("src");
                    var ref_src = handler.info.searchBtn.data("ref-src");
                    if (cur_iframe_src != ref_src) {
                        handler.info.iframe.attr("src", ref_src);
                        handler.info.iframe.load(function () {
                            params.callback.call(handler, handler.info.searchInput.val(), handler.info.searchBtn);
                        });
                    }
                    else {
                        try {
                            params.callback.call(handler, handler.info.searchInput.val(), handler.info.searchBtn);
                        }
                        catch (e) {

                        }
                    }
                    return false;
                });
                if (params.auto) {
                    handler.info.searchInput.on("onkeyup", function () {
                        if (params.auto) {
                            //var timer = setTimeout(function () {
                            //     clearTimeout(timer);
                            //     handler.info.searchBtn.trigger("click");
                            //}, 500);
                        }
                    }).on("onkeydown", function () {
                        if (params.auto && timer) {
                            //clearTimeout(timer);
                        }
                    });
                }
                handler.activeTo("toolbar", null, true);
            },
            /**
            * 打开窗口
            * @method activeTo
            * @param {string,min|max|normal_可以为空_表示默认方式打开|mode为空时表示打开操作} mode
            * @param {object,鼠标事件对象_当在normal模式下_重定义窗口的位置} event
            * @param {bool,是否使用动画方式打开}   useAnimate
            */
            activeTo: function (mode, event, useAnimate) {
                var handler = this, useAnimate = typeof (useAnimate) == 'undefined' ? true : useAnimate, _isOpenActive = typeof (mode) == "undefined";
                if (handler.currentMode == "min") {
                    //窗口处于最小化状态时执行尺寸还原
                    handler.setWillMode("keep-mode", event);
                }
                else if (typeof (mode) == "string" && ("max" == mode || "normal" == mode || "min" == mode || "toolbar" == mode)) {
                    handler.setWillMode(mode, event);
                }
                else {
                    handler.setWillMode((_isOpenActive ? handler.args.mode : handler.currentMode), event);
                }

                (function (handler) {
                    var _animateParams = handler._animateParams;
                    _animateParams.speed = _isOpenActive ? (that.config.resizeSpeed || "fast") : (that.config.openSpeed || "fast");
                    if (mode != "min") {
                        handler.active = true;
                        if (useAnimate) {
                            handler.window.css({ "opacity": (_isOpenActive ? 0 : 1) }).show().stop().animate({
                                opacity: 1,
                                width: _animateParams.width,
                                height: _animateParams.height,
                                left: _animateParams.left,
                                top: _animateParams.top
                            }, _animateParams.speed).playAnimateScale(1, null, 300);
                            handler.info.body.stop().animate({ height: _animateParams["body"].height }, _animateParams.speed);
                            handler.info.viewPort.stop().animate({
                                width: _animateParams["viewPort"].width,
                                height: _animateParams["viewPort"].height,
                                left: _animateParams["viewPort"].left
                            }, _animateParams.speed);
                            handler.info.menu.stop().animate({
                                height: _animateParams["menu"].height,
                                left: _animateParams["menu"].left
                            }, _animateParams.speed);
                            handler.info.iframe.stop().animate({ height: _animateParams["iframe"].height }, _animateParams.speed, function () {

                                if (typeof (resizeChangeEventHandler) == "function") {
                                    resizeChangeEventHandler(handler, handler.args);
                                }
                                if (typeof (openedEventHandler) == "function") {
                                    openedEventHandler(handler, handler.args);
                                }
                                if ("toolbar" == mode) {
                                    if (handler.info.headerBottomBar.is(":hidden")) {
                                        handler.info.headerBottomBar.css({ opacity: 0 }).show();
                                    }
                                    handler.info.headerBottomBar.stop().animate({ opacity: 1 });
                                }
                            });
                        }
                        else {
                            handler.window.css({
                                "opacity": 1,
                                width: _animateParams.width,
                                height: _animateParams.height,
                                left: _animateParams.left,
                                top: _animateParams.top
                            });
                            handler.info.body.css({ height: _animateParams["body"].height });
                            handler.info.viewPort.css({ height: _animateParams["viewPort"].height })
                                    .stop().animate({
                                        width: _animateParams["viewPort"].width,
                                        left: _animateParams["viewPort"].left
                                    }, _animateParams.speed);
                            handler.info.menu.css({ height: _animateParams["menu"].height })
                                    .stop().animate({ left: _animateParams["menu"].left }, _animateParams.speed);
                            handler.info.iframe.css({ height: _animateParams["iframe"].height });
                            if (typeof (resizeChangeEventHandler) == "function") {
                                resizeChangeEventHandler(handler, handler.args);
                            }
                            if (typeof (openedEventHandler) == "function") {
                                openedEventHandler(handler, handler.args);
                            }
                            if ("toolbar" == mode) {
                                handler.info.headerBottomBar.fadeIn();
                            }
                        }
                    }
                    else {
                        handler.active = false;
                        handler.window.fadeOut("fast", function () {
                            if (typeof (resizeChangeEventHandler) == "function") {
                                resizeChangeEventHandler(handler, handler.args);
                            }
                        });
                    }
                    if (mode == "max" || mode == "toolbar") {
                        handler.info.menuSwitchBox.stop().animate({ "top": handler.args.hassearch ? 40 : 23 }, _animateParams.speed);
                    }
                    else {
                        handler.info.menuSwitchBox.stop().animate({ "top": 8 }, _animateParams.speed);
                    }
                })(handler);

            },
            /**
            * 关闭窗口，根据配置可能会销毁对象，配置的key为‘dropEnable’
            * @method close
            */
            close: function () {
                var win = this;
                var web_window = $(window);
                win.window.stop().animate({ "opacity": 0, "left": (web_window.width() - win.window.width()) / 2, "top": (web_window.height() - win.window.height()) / 2 }, that.config.closeSpeed || "fast");
                win.window.playAnimateScale(0.5, function () {
                    win.active = false;
                    if (typeof (closedEventHanlder) == "function") closedEventHanlder(win, win.args);
                    if (win.args.dropEnable) {
                        win.window.remove();
                        that.dialogCache[win.args.winid] = null;
                        delete that.dialogCache[win.args.winid];
                        delete win;
                    }
                })
            }
        }
        /**
        * 打开一个窗口实例，如果窗口已经缓存那么直接提取缓存的窗口并打开
        * @method open
        * @param {json,配置信息，格式请参见文档} opts
        * @param {object,鼠标事件参数对象} event
        */
        that.openWin = function (opts, orgin) {
            var handler;
            if (opts.winid) {
                handler = that.dialogCache[opts.winid];
            }
            if (!handler) {
                if (typeof (opts.hasmenu) === undefined) opts.hasmenu = true;
                opts = $.extend({}, { dropEnable: false }, opts);
                opts.winid = opts.winid || that._generateId("win");
                handler = new _OpenWin(opts);
                handler.orgin = orgin || [];
                handler.window.data("handler", handler);
            }
            handler._animateParams.speed = that.config.openSpeed || "fast";
            if (opts.autoOpen === undefined || opts.autoOpen) { handler.activeTo(); }
            return handler;
        }
    };
    _EzDialog.prototype = {
        /**
        * 生成一个随机串，私有方法，不允许外部调用或修改
        * @method _getRandomStr
        */
        _getRandomStr: function () {
            return (Math.random() + "").split('.')[1];
        },
        /**
        * 生成一个唯一的ID串
        * @method _generateId
        * @param {string,前缀} per
        * @param {string,区分标记} flag
        */
        _generateId: function (per, flag) {
            per = per + "_" + (this._dialogCounter++);
            return per.indexOf(this.runtime) > 0 ? per + "_" + (flag || "") : per + "_" + this.runtime + "_" + (flag || "");
        },
        /**
        * 获取一个dom对象
        * @method _getDomElemet
        * @param {string,dom元素id} id
        * @param {bool,是否获取的为jq对象} jq
        */
        _getDomElemet: function (id, jq) {
            return jq ? $("#" + id) : document.getElementById(jq);
        },
        /**
        * 获取对话框模板
        * @method _getDiolagTemplate
        * @param {json,窗口配置信息，格式请参见文档} opts
        * @param {object,win  窗口实例} win
        */
        _getDiolagTemplate: function (opts, win) {
            var that = this,
            _win_id = opts.winid,
            _title = opts.title || "", // + x++,
            _id_header_bar = that._generateId(_win_id, "header"),
            _id_header_title = that._generateId(_win_id, "title"),
            _id_body = that._generateId(_win_id, "body"),
            _id_footer = that._generateId(_win_id, "footer"),
            _id_iframe = that._generateId(_win_id, "iframe");
            opts.title = _title;
            var _src = opts.src + (opts.src.indexOf("?") >= 0 ? "&" : "?") + "EzDialogid=" + _win_id;

            var _dialogTemplate = '<div id="' + _win_id + '" class="window" style="display:none;">\
                <div class="win-wrapper">\
                <div id="' + _id_header_bar + '" class="win-header-bar">\
                <div class="win-menu-switchbox"><a href="javascript:void(0)" style="display:' + (opts.hasmenu ? "" : "none") + ';" class="fa angle-double-right shadow"></a></div>\
                <div class="win-header-bar-top">\
                    <div  class="win-header-left" data-isdrag="true" data-drag-id="' + _win_id + '"> </div>\
                    <div id="' + _id_header_title + '" class="win-header-mid" data-isdrag="true" data-drag-id="' + _win_id + '">' + _title + '</div>\
                    <div class="win-header-right" data-isdrag="true" data-drag-id="' + _win_id + '">\
                    <a class="fa win-close shadow"  title="关闭" href="javascript:void(0)"></a>\
                    <a class="fa win-size-max shadow" title="最大化" href="javascript:void(0)"></a>\
                    <a class="fa win-size-min shadow"  title="最小化" href="javascript:void(0)"></a>\
                    </div>\
                </div>\
                <div class="win-header-bar-bottom" data-isdrag="true" data-drag-id="' + _win_id + '" style="display:' + (opts.hassearch ? "" : "none") + ';">\
                <div class="win-header-b-left" data-isdrag="true" data-drag-id="' + _win_id + '"></div>\
                <div class="win-header-b-right" data-isdrag="true" data-drag-id="' + _win_id + '"><div class="searchbox"><input type="text" /><a href="javascript:void(0)" class="fa ico-search"></a></div></div>\
                </div>\
                </div>\
                <div id="' + _id_body + '" class="win-body">\
                <div class="win-body-menus" style="display:' + (opts.hasmenu ? "" : "none") + ';">\
                <ul class="menus-fun-list">\
                <li class="menus-fun">\
                <a href="javascript:void(0)" class="fa fa-caret-down menus-fun-title">账户管理</a>\
                <ul class="menus-fun-children">\
                </ul>\
                </li>\
                </ul>\
                </div>\
                <div class="win-body-viewport"><iframe id="' + _id_iframe + '" name="' + _id_iframe + '" frameborder="0" scrolling="yes" src="' + _src + '" width="100%" height="420"></iframe></div>\
                </div>\
                <div id="' + _id_footer + '" class="win-footer"></div>\
                </div>\
                </div>';
            $(that.config.target).append(_dialogTemplate);
            var header = that._getDomElemet(_id_header_bar, true),
                body = that._getDomElemet(_id_body, true),
                footer = that._getDomElemet(_id_footer, true),
                headerBottomBar = header.find(".win-header-bar-bottom"),
                menu = body.find(".win-body-menus"), iframe = $("#" + _id_iframe), title = header.find(".win-header-mid"), menuBox = menu.find(".menus-fun-list");
            iframe.data("set_EzDialogTitle_callback", function (titleTxt) {
                $("#" + _id_header_title).html(titleTxt);
                if (win) {
                    win.args.title = titleTxt;
                }
            });

            return {
                window: that._getDomElemet(_win_id, true),
                header: header, body: body, footer: footer, title: title, menu: menu, menuBox: menuBox, iframe: iframe, headerBottomBar: headerBottomBar,
                swithBtn: header.find(".win-header-left a"),
                menuSwitch: header.find(".angle-double-right"),
                menuSwitchBox: header.find(".win-menu-switchbox"),
                closeBtn: header.find(".win-header-right a.win-close"),
                maxBtn: header.find(".win-header-right a.win-size-max"),
                minBtn: header.find(".win-header-right a.win-size-min"),
                searchInput: headerBottomBar.find(".win-header-b-right input"),
                searchBtn: headerBottomBar.find(".win-header-b-right a"),
                viewPort: body.find(".win-body-viewport")
            };
        },
        /**
        * 根据窗口id查找窗口
        * @method findEzDialog
        * @param {string,窗口id} id
        */
        findEzDialog: function (id) {
            return $("#" + id).data("handler");
        }
    }
    window["EzDialog"] = _EzDialog;
    /*Plugin drag,如果dom对象存在isdrag=true属性 则此模块可以作用于此对象执行拖动操作*/
    $.fn.onDragListening = function (mouseDownHook, mouseMoveHook, mouseUpHook) {
        var _dragCache = {}, _dragCurrentId, _dragswith = false, _startPoint = { x: 0, y: 0 }, _box = $(this);
        _box.css("position", "relative");
        var _mouseDownHandler = function (event) {
            _box.bind("custompreventDefault", function () {
                if (!$(event.target).is("input") && !$(event.target).is("select") && !$(event.target).is("radio") && !$(event.target).is("text")) {
                    preventDefault(event);
                }
            }).trigger("custompreventDefault");
            var _target = $(event.target), _isdrag = _target.data("isdrag");
            event.dragEnable = _isdrag;
            var _dCache;
            if (!event.dragEnable) {/*清除拖动状态信息*/
                var _dragParam = _dragCache[_dragCurrentId];
                _dragParam = null;
                delete _dragParam;
            }
            else {
                _dragCurrentId = _target.data("drag-id");
                _dCache = { moveLenX: 0, moveLenY: 0 };
                _dCache.target = _dragCurrentId ? $("#" + _dragCurrentId) : _target;
                _dCache.target.css("position", "absolute");

                /*防止 在拖动时 鼠标进入 iframe 区域 导致拖动失败的情况*/
                /*鼠标进入当前可拖动模块的iframe区域*/
                var body_converid = "drog_box_conver";
                var body_conver = $("#" + body_converid);
                if (!body_conver.is("div")) {
                    body_conver = $('<div id="' + body_converid + '" style="width:' + $(window).width() + 'px;height:' + $(window).height() + 'px;position:absolute;left:0px;top:0px;z-index:' + (_topZindex - 1) + ';"></div>');
                    body_conver.appendTo("body");
                }
                /*鼠标进入其他可拖动模块的iframe区域*/
                var converid = (_dCache.target.attr("id") || (Math.random() + "").split('.')[1]) + "_conver";
                var conver = $("#" + converid);
                if (!conver.is("div")) {
                    conver = $('<div id="' + converid + '" style="width:' + _dCache.target.width() + 'px;height:' + _dCache.target.height() + 'px;position:absolute;left:0px;top:0px;"></div>');
                    conver.appendTo(_dCache.target);
                }

                _dCache.conver = conver;
                _dCache.bodyConver = body_conver;
                _dragCache[_dragCurrentId] = _dCache;
                _startPoint = { x: event.clientX, y: event.clientY }, _dragswith = true;

                var _mouseDownOnWinHandler = _dCache.target.data("_mouseDownOnWinHandler");
                if (_mouseDownOnWinHandler) {
                    _mouseDownOnWinHandler(event, _dCache);
                }
            }
            if (typeof (mouseDownHook) == 'function')
                mouseDownHook(event, _dCache);
        }
        var _mouseMoveHandler = function (event) {
            event.x = 1000;
            if (_dragswith) {
                var newPoint_X = event.clientX,
                        newPoint_Y = event.clientY,
                        _moveLenX = newPoint_X - _startPoint.x,
                        _moveLenY = newPoint_Y - _startPoint.y,
                        _dragParam = _dragCache[_dragCurrentId],
                        _newleft = _moveLenX + _dragParam.target.position().left,
                        _newtop = _moveLenY + _dragParam.target.position().top;
                _startPoint = { x: newPoint_X, y: newPoint_Y };
                _dragParam.moveLenX = _moveLenX;
                _dragParam.moveLenY = _moveLenY;
                _dragParam.target.css({ "left": _newleft, "top": _newtop });
                _dragParam.conver.show();
                _dragParam.bodyConver.show();

                var _moveWinHandler = _dragParam.target.data("_moveWinHandler");
                if (typeof (_moveWinHandler) == "function")
                    _moveWinHandler(event, _dragParam);
            }
            if (typeof (mouseMoveHook) == 'function')
                mouseMoveHook(event, _dragParam);
        }
        var _mouseUpHandler = function (event) {
            _dragswith = false;
            var _cache = _dragCache[_dragCurrentId];
            if (_cache) {
                _cache.conver.hide();
                _cache.bodyConver.hide();
                var _mouseUpOnWinHandler = _cache.target.data("_mouseUpOnWinHandler");
                if (typeof (_mouseUpOnWinHandler) == 'function') {
                    _mouseUpOnWinHandler(event, _cache);
                }
            }
            _box.unbind("custompreventDefault");
            if (typeof (mouseUpHook) == 'function')
                mouseUpHook(event, _cache);
        }
        $(this).on("mousedown", _mouseDownHandler).on("mousemove", _mouseMoveHandler).on("mouseup", _mouseUpHandler);
    }
})(jQuery)