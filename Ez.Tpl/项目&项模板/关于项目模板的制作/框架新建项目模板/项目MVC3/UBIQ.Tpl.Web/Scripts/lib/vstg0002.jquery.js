/*!
 * DialogDrag& For Product JavascriptLibrary
 * XDialog v1.0
 * Jquery v1.7.2
 * User: kongjing/endfalse@163.com
 * Date: 2014.08.08
 * Include Jquery (http://www.jquery.com/)
 */
(function ($) {
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
    };
    var _topZindex = 12, x = 0;
    var _XDialog = function (config, perOpenEventHandler, openedEventHandler, closedEventHanlder, minEventHandler, resizeChangeEventHandler, windowMoveEventHandler, structWindowData) {
        var that = this;
        that._resizeChangeEventhandler = resizeChangeEventHandler;

        that.config = $.extend({}, config, { target: "body", topZindex: 12 }); // targetTag;
        _topZindex = that.config.topZindex;
        that.runtime = that._getRandomStr(); // (Math.random() + "").split('.')[1];
        that.dialogCache = {};
        that._dialogCounter = 0;
        //监视窗口变化
        $(window).on("resize", function () {
            for (var key in that.dialogCache) {
                var _win = that.dialogCache[key];
                _win.changeWinSize();
            }
        });
        var _drop_win_mousedown = false;
        var _OpenWin = function (opts) {
            var win = this;
            win.preWinSize = this.currentWinSize = (opts.winsize || "normal").toLowerCase();
            win.args = opts;
            win.info = that._getDiolagTemplate(opts, win, opts.structWindowData || structWindowData);
            win.window = win.info.window;
            var canopen = true
            if (typeof (opts.perOpenEventHandler) == "function") {
                canopen = opts.perOpenEventHandler(win, opts);
                if (typeof (canopen) == 'undefined') canopen = true;
            }
            if (typeof (perOpenEventHandler) == "function") {
                canopen = perOpenEventHandler(win, opts);
                if (typeof (canopen) == 'undefined') canopen = true;
            }
            if (!canopen) return;
            //关闭窗口
            win.info.closeBtn.on("click", function () {
                win.close();
            });
            //双击切换窗口尺寸和位置
            var _mouse_down_ = false;
            win.info.header.on("dblclick", function (event) {
                var win_state = win.currentWinSize;
                if (win_state == "max") win.changeWinSize("backsize");
                else win.changeWinSize("max");
            }).on("mousedown", function (event) {
                _mouse_down_ = true;
            }).on("mouseup", function (event) {
                _mouse_down_ = false;
            });
            win.info.maxBtn.on("click", function () {
                win.changeWinSize("max");
            });
            win.info.minBtn.on("click", function () {
                var web_window = $(window);
                win.window.stop().animate({ "opacity": 0, "left": (web_window.width() - win.window.width()) / 2, "top": (web_window.height() - win.window.height()) / 2 });
                win.window.playAnimateScale(0.8, function () {
                    win.window.fadeOut("fast");
                });
                if (typeof (minEventHandler) == "function") minEventHandler(win, opts);
                win.active = false;
                win.preWinSize = win.currentWinSize;
                win.currentWinSize = "min";
                that.dialogCache[opts.winid] = win;
            });
            win.info.menuSwitch.on("click", function () {
                if (win.info.menu.is(":hidden")) win.info.menu.show()
                else win.info.menu.hide();
                win.changeWinSize(win.currentWinSize, true);
            });
            var jointime, rejointime;
            //向被拖动对象注入鼠标移动事件
            win.window.data("_moveHandler", function (event) {
                if (!_mouse_down_) return;
                if (Math.abs(event.clientY - 24) < 1) {
                    win.changeWinSize("max");
                    win.autoToMaxActioned = true;
                    jointime = new Date().getTime();
                }
                else if (win.window.position().left > 0 && win.currentWinSize == "max") {
                    win.changeWinSize("backsize");
                }
                else {
                    rejointime = new Date().getTime();
                    if ((rejointime - jointime) < 800) return;
                    if (win.autoToMaxActioned) {
                        win.changeWinSize("backsize");
                        win.autoToMaxActioned = false;
                    }
                    else {//这是在更新窗口拖动后的位置
                        var winpos = win.window.position();
                        var stateinfo = win.window.data("stateinfo");
                        stateinfo.left = winpos.left;
                        stateinfo.top = winpos.top;
                        win.window.data("stateinfo", stateinfo);
                    }
                }
                if (typeof (windowMoveEventHandler) == "function") windowMoveEventHandler(win, win.args);
            });
            win.changeWinSize(this.currentWinSize);
            //初始化尺寸和位置记忆
            var _size = { h: $(window).height(), w: $(window).width() }
            var _wH = win.window.height();
            var _wW = win.window.width();
            var _left = (_size.w - _wW) / 2;
            var _top = (_size.h - _wH) / 2;
            win.window.css({ "left": _left, "top": _top });
            win.window.data("stateinfo", { w: win.window.width(), h: win.window.height(), left: _left, top: _top });

            if (typeof (structWindowData) == 'function') {
                var _menus = structWindowData(win.args.src); /*??*/
                if (_menus) win.args.hasmenu = true;
                this.createMenu(_menus || [], false, "sys");
            }
        };
        _OpenWin.prototype = {
            _updateWinState: function (winsize) {
                var _preWinSize = this.preWinSize;
                this.preWinSize = this.currentWinSize;
                this.currentWinSize = winsize;
                if (winsize == "backsize") this.currentWinSize = _preWinSize;
                if (typeof (that._resizeChangeEventhandler) == "function") that._resizeChangeEventhandler(this, this.args);
                that.dialogCache[this.args.winid] = this;
            },
            _setWinSizeAndPosition: function (width, height, left, top) {
                var wininfo = this.info, _args = this.args;
                wininfo.window.css({ height: height, width: width, top: top, left: left });
                var _left_space_percent = wininfo.menu.outerWidth() / wininfo.window.outerWidth();
                var _h = height - (wininfo.header.height() || 35);
                if (this.args.hasmenu && !wininfo.menu.is(":hidden")) {
                    wininfo.menu.width(width * _left_space_percent - 10).height(_h);
                    wininfo.viewPort.width(width * (1 - _left_space_percent)).height(_h);
                }
                else {
                    wininfo.menu.hide();
                    wininfo.viewPort.width(width).height(_h);
                }
                wininfo.body.height(_h);
                wininfo.iframe.height(_h);
            },
            createMenu: function (_menus, append, flag) {/*[{ title: "", children: [{}, {}, {}]}];*/
                var wininfo = this.info, _args = this.args;
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
                if (_menus && _menus.length !== undefined && _menus.length > 0) {
                    for (var i = 0; i < _menus.length; i++) {
                        var group = _menus[i];
                        htmlpool.push('<li class="menus-fun" from="' + attr + '">');
                        htmlpool.push('<a href="javascript:void(0)" class="fa fa-caret-down menus-fun-title">' + group.title + '</a>');
                        htmlpool.push('<ul class="menus-fun-children">');
                        var replace_keys = {};
                        for (var t = 0; t < group.children.length; t++) {
                            var link_A = group.children[t];
                            if (link_A) {
                                var src = link_A.src;
                                var text = link_A.text;
                                var xdialogid = wininfo.window.attr("id");
                                var iframeid = wininfo.iframe.attr("id");
                                if (src && typeof (src) == "string") {
                                    src = src + (src.indexOf("?") >= 0 ? "&" : "?") + "xdialogid=" + xdialogid;
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
                                var li = '<li linkbtn="a"><a href="' + src + '" class="fa fa-hand-o-right" target="' + iframeid + '" ' + key + '>' + text + '</a></li>';
                                htmlpool.push(li);
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

                }
            },
            changeWinSize: function (winsize, doNotUpdate) {
                var _wininfo = this.info, _args = this.args, _window_size = { h: $(window).height(), w: $(window).width() }, to_width, to_height, to_left, to_top;
                switch (winsize) {
                    case "min": break;
                    case "normal":
                        if (typeof (_args.width) == 'number' && typeof (_args.height) == 'number' && _args.width > 0 && _args.height > 0) {
                            to_width = _args.width, to_height = _args.height, to_left = (_window_size.w - _args.width) / 2, to_top = (_window_size.h - _args.height) / 2;
                        }
                        break;
                    case "backsize":
                        var __stateinfo = _wininfo.window.data("stateinfo");
                        if (!winsize && !__stateinfo) return;
                        __stateinfo.top = __stateinfo.top <= 24 ? 25 : __stateinfo.top;
                        to_width = __stateinfo.w, to_height = __stateinfo.h, to_left = __stateinfo.left, to_top = __stateinfo.top;
                        break;
                    case "max":
                    default:
                        to_width = _window_size.w, to_height = _window_size.h, to_left = 0, to_top = 0;
                        break;
                }
                if (typeof (to_width) == 'number' && typeof (to_height) == 'number' && typeof (to_left) == 'number' && typeof (to_top) == 'number') {
                    this._setWinSizeAndPosition(to_width, to_height, to_left, to_top);
                }
                if (!doNotUpdate) {/*默认需要更新*/
                    this._updateWinState(winsize);
                }
            },
            setXdialogParam: function (params) {
                if (typeof (param) == undefined) return;
                this.window.data("xdialog_params", params);
                var replaceKey = this.info.menuBox.data("rpkey");
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
                            if(canset && canrp) {
                                url_param = replaced_paramstr;
                            }
                            else {
                                url_param = url_param_str;
                            }
                            a.attr("href",href + (href.indexOf("?") >= 0 ? "&" : "?") + url_param);
                        }
                    });
            },
            getXdialogParam: function () {
                return this.window.data("xdialog_params");
            },
            open: function () {
                var handler = this;
                var _animate;
                var _preWinSize = handler.preWinSize;
                if (handler.window.is(":hidden")) {
                    handler.window.css({ "opacity": 0 }).show();
                    var web_window = $(window), left = handler.window.position().left, top = handler.window.position().top;
                    if (handler.currentWinSize == "min") {//窗口处于最小化状态
                        handler.changeWinSize("backsize");
                    }
                    var start_css = { "opacity": 0 }, to_css = { "opacity": 1 }
                    if (_preWinSize != "max") {
                        start_css.left = (web_window.width() - handler.window.width()) / 2;
                        start_css.top = (web_window.height() - handler.window.height()) / 2;
                        to_css.left = left;
                        to_css.top = top;
                    }
                    else if (_preWinSize == "max") {
                        //to_css.left = 0;
                        //to_css.top = 0;
                    }
                    _animate = function () {
                        handler.window.css(start_css).stop().animate(to_css);
                        handler.window.playAnimateScale(1, null, 300);
                    }
                }
                $("body .window").css("z-index", that.config.topZindex - 2);
                handler.window.css("z-index", that.config.topZindex);

                if (typeof (_animate) == 'function')
                    _animate();
                else
                    handler.window.show();

                handler.active = true;
                if (typeof (handler.args.openedEventHandler) == "function") { handler.args.openedEventHandler(handler, handler.args); }
                if (typeof (openedEventHandler) == "function") { openedEventHandler(handler, handler.args); }
                //if (typeof (resizeChangeEventHandler) == "function") { resizeChangeEventHandler(handler, handler.args); }
                handler._updateWinState(handler.currentWinSize);
            },
            close: function () {
                //this.window.hide();
                var win = this;
                var web_window = $(window);
                win.window.stop().animate({ "opacity": 0, "left": (web_window.width() - win.window.width()) / 2, "top": (web_window.height() - win.window.height()) / 2 });
                win.window.playAnimateScale(0.5, function () {
                    win.window.fadeOut("fast");
                    win.active = false;
                    if (win.args.dropEnable) {
                        if (typeof (win.args.closedEventHandler) == "function") win.args.closedEventHandler(win, win.args);
                        if (typeof (closedEventHanlder) == "function") closedEventHanlder(win, win.args);
                        win.window.remove();
                        that.dialogCache[win.args.winid] = null;
                        delete that.dialogCache[win.args.winid];
                        delete win;
                    }
                })
            }
        }
        that.openWin = function (opts) {
            var handler;
            if (opts.winid) {
                handler = that.dialogCache[opts.winid];
            }
            if (!handler) {
                if (typeof (opts.hasmenu) === undefined) opts.hasmenu = true;
                opts = $.extend({}, { dropEnable: false }, opts); //{title:"",html:"",src:"",winsize:"max|min|normal",autoOpen:true,dropEnable:false}
                opts.winid = opts.winid || that._generateId("win");
                handler = new _OpenWin(opts);
                handler.window.data("handler", handler);
            }
            if (opts.autoOpen === undefined || opts.autoOpen) { handler.open(); }
            return handler;
        }
    };
    _XDialog.prototype = {
        _getRandomStr: function () {
            return (Math.random() + "").split('.')[1];
        },
        _generateId: function (per, flag) {
            per = per + "_" + (this._dialogCounter++);
            return per.indexOf(this.runtime) > 0 ? per + "_" + (flag || "") : per + "_" + this.runtime + "_" + (flag || "");
        },
        _getDomElemet: function (id, jq) {
            return jq ? $("#" + id) : document.getElementById(jq);
        },
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
            var _src = opts.src + (opts.src.indexOf("?") >= 0 ? "&" : "?") + "xdialogid=" + _win_id;

            var _dialogTemplate = '<div id="' + _win_id + '" class="window" style="display:none;">\
                <div class="win-wrapper">\
                <div id="' + _id_header_bar + '" class="win-header-bar">\
                <div class="win-header-bar-top">\
                    <div  class="win-header-left" data-isdrag="true" data-drag-id="' + _win_id + '"><a href="javascript:void(0)" style="display:' + (opts.hasmenu ? "" : "none") + ';" class="fa angle-double-right shadow"></a></div>\
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
            iframe.data("set_xwindow_callback", function (titleTxt) {
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
                closeBtn: header.find(".win-header-right a.win-close"),
                maxBtn: header.find(".win-header-right a.win-size-max"),
                minBtn: header.find(".win-header-right a.win-size-min"),
                searchInput: headerBottomBar.find(".win-header-b-right input"),
                searchBtn: headerBottomBar.find(".win-header-b-right a"),
                viewPort: body.find(".win-body-viewport")
            };
        },
        findXdialog: function (id) {
            return $("#" + id).data("handler");
        }
    }
    window["XDialog"] = _XDialog;
    /*Plugin drag*/
    $.fn.onDragListening = function (mouseDownHook, mouseMoveHook, mouseUpHook) {
        var _dragCache = {}, _dragCurrentId, _dragswith = false, _startPoint = { x: 0, y: 0 }, _box = $(this);
        _box.css("position", "relative");
        var _mouseDownHandler = function (event) {
            _box.bind("custompreventDefault", function () {
                preventDefault(event);
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

                var fn = _dragParam.target.data("_moveHandler");
                if (typeof (fn) == "function")
                    fn(event, _dragParam);
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
            }
            _box.unbind("custompreventDefault");
            if (typeof (mouseUpHook) == 'function')
                mouseUpHook(event, _cache)
        }
        $(this).on("mousedown", _mouseDownHandler).on("mousemove", _mouseMoveHandler).on("mouseup", _mouseUpHandler);
    }
})(jQuery)