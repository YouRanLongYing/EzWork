/*!
* WebDesk For Product JavascriptLibrary
* DeskTop2 v1.0
* Jquery v1.7.2
* User: kongjing/endfalse@163.com
* Date: 2014.08.27
* Include Jquery (http://www.jquery.com/)
*/
(function ($, xdialog) {
    var _DeskTop = function (langlist) {
        var desk = this;
        desk.lang = langlist || { WEEK_LIST: "星期日,星期一,星期二,星期三,星期四,星期五,星期六" };
        desk.initVar();
        $(window).on("resize", function () {
            desk.setLayout();
        }).trigger("resize");
        //设置子菜单效果
        (function (desk) {
            var _as = desk.leftMenu.find("a");
            desk.leftMenu.click(function (e) {
                var target = $(e.target);
                var _parent = target.parent();
                if (target.is("a") && (_parent.hasClass("title") || _parent.hasClass("title-cbtn"))) {
                    var _next_ul = _parent.next();
                    if (_next_ul.is("ul")) {
                        var _is_hidden = _next_ul.is(":hidden");
                        if (_is_hidden) {
                            desk.findParent(_parent, 2).find("div.title").next().hide();
                            _next_ul.show();
                        }
                        else {
                            _next_ul.hide();
                        }
                    }
                    var _href = target.data("src") || target.attr("href") || "-";
                    if (_href != "-" && _href != "javascript:void(0)") {
                        desk.iframe.attr("src", _href);
                    }
                }
            })
        })(desk);
        /*设置菜菜单滑动效果*/
        (function (desk) {
            var stack = new Array();
            var _animateToHide = function (pannel) {
                if (pannel)
                    pannel.data("index", 0).stop().animate({ left: 0 }, "fast", function () {
                        pannel.hide();
                    });
            }
            var _hideAll_slide = function () {
                while (stack.length > 0) {
                    _animateToHide(stack.pop());
                }
            }
            var _slide_to_hide_event = function (pannel) {
                if (pannel.position().left > 0) {
                    var _loop_curbox = stack.pop();
                    if (_loop_curbox) {
                        if (pannel.data("index") != _loop_curbox.data("index")) {
                            _animateToHide(_loop_curbox);
                        }
                        else {
                            stack.push(_loop_curbox);
                        }
                    }
                }
                else {
                    _hideAll_slide();
                }
            }
            var _click_event = function (_li, slide) {
                var _a = _li.find("a"),
                 _href = _a.data("src") || _a.attr("href") || "-",
                 _ccount = _li.data("children-count"),
                 _cid = _li.data("children-id"),
                 _intccount = parseInt(_ccount),
                 _cur_box = desk.findParent(_li, 3);
                _cur_box.find("li").removeClass("current");
                _li.addClass("current");
                if (!_intccount && !_ccount) {
                    if (_href == "-" || _href == "javascript:void(0)") return;
                    desk.iframe.attr("src", _href);
                }
                else if (_cid && typeof (_cid) == "string") {
                    var _pannel = $("#" + _cid);
                    if (_intccount > 0 && _pannel) {
                        if (slide) {
                            var _cur_box_index = desk.findParent(_li, 3).data("index") || 0;
                            if ((_pannel.data("index") || 0) == 0) {
                                var _left = _pannel.position().left;
                                if (_cur_box_index == 0) { _hideAll_slide(); }
                                var _menu_opened_num = stack.length == 0 ? 1 : (stack.length + 1);
                                var _left_menu_box_width = desk.leftMenu.width();
                                _pannel.data("index", _menu_opened_num)
                                .css({ top: 0, left: (_menu_opened_num - 1) * _left_menu_box_width }).show()
                                    .stop().animate({ left: (_left_menu_box_width * _menu_opened_num) }, "fast");
                                stack.push(_pannel);
                            }
                            else {
                                _animateToHide(stack.pop());
                            }
                        }
                        else {
                            if (desk.bodyInnerBox.position().left < 0) {
                                desk.leftMenuSwitch();
                            }
                            desk.leftMenuBoxs.css("z-index", 10000).hide();
                            _pannel.data("index", 0).css("z-index", 10002).fadeIn();
                        }
                    }
                }
            };
            var _enterpannel = false;
            desk.leftMenuBoxs.on("mouseenter", function () { _enterpannel = true; _slide_to_hide_event($(this)); })
            .on("mouseleave", function () {
                setTimeout(function () {
                    if (!_enterpannel) {
                        _hideAll_slide();
                    }
                    _enterpannel = false
                }, 500);
            });
            desk.topBarBtns.click(function () { _click_event($(this)); })
            desk.leftMenuLis.click(function () { _click_event($(this), true); })
        })(desk);
        /*任务栏系统时间*/
        (function (desk) {
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
                var weekday = desk.lang.WEEK_LIST.split(",")
                desk.systemtime.attr("title", _y_m_d + " " + weekday[_dd]);
                desk.systemtime.text(_h_f_m);
            }, 1000);
        })(desk);


        /*菜单开关*/
        desk.switchbar.on("click", function () { desk.leftMenuSwitch(); });
        /*监视鼠标调用是否显示以隐藏的左侧菜单*/
        (function (desk) {
            var _handler = 0;
            $(window).on("mousemove", function (e) {
                if (desk.bodyInnerBox.position().left < 0) {
                    if (e.clientX < 1 && e.clientY > 125) {
                        _handler = setTimeout(function () {
                            desk.leftMenu.stop().animate({ left: desk.leftMenu.width() }, "fast")
                        }, 100);
                    }
                }
            })
            var _time_out_handler = 0;
            desk.leftMenuBoxs.on("mouseenter", function () {
                if (_time_out_handler) clearTimeout(_time_out_handler);
            }).on("mouseleave", function () {
                _time_out_handler = setTimeout(function () {
                    if (desk.bodyInnerBox.position().left < 0) {
                        desk.leftMenu.stop().animate({ left: 0 }, "fast")
                    }
                }, 10)
            })
        })(desk);
        /*设置客户菜单*/
        (function (desk) {

            var _w = desk.pageRequiredMenu.outerWidth();
            desk.pageRequiredMenu.hide().on("custom_animate_hidden", function () {
                desk.pageRequiredMenu.stop().animate({ right: (3 - _w) }, "fast", function () {
                    desk.pageRequiredMenu.addClass("hidden");
                });
            });
            desk.pageRequiredMenu.on("custom_animate_show", function () {
                if (desk.pageRequiredMenu.hasClass("hidden")) {
                    desk.pageRequiredMenu.removeClass("hidden");
                    desk.pageRequiredMenu.stop().animate({ right: 0 }, "fast");
                }
            });

            if (desk.pageRequiredMenu.find("li").size() > 0) {
                setTimeout(function () {
                    desk.pageRequiredMenu.trigger("custom_animate_hidden");
                }, 1000)
            }

            desk.pageRequiredMenu.on("mouseenter", function () {
                desk.pageRequiredMenu.trigger("custom_animate_show");
            }).on("mouseleave", function () {
                desk.pageRequiredMenu.trigger("custom_animate_hidden");
            })
        })(desk)
    }
    _DeskTop.prototype = {
        _getRandomStr: function () {
            return (Math.random() + "").split('.')[1];
        },
        initVar: function () {
            this.container = $("#layout_container");
            this.topBox = $("#layout_header_wrapper");
            this.body = $("#layout_body_wrapper");
            this.bodyInnerBox = $("#body_wrapper");
            this.leftMenu = $("#body_left_box");
            this.rightBox = $("#body_right_box");
            this.footBox = $("#layout_footer_wrapper");
            this.topBarBtns = $("#top_bar_btns li");
            this.leftMenuBoxs = this.leftMenu.find(".fun-list-box")
            this.iframe = $("#main");
            this.leftMenuLis = this.leftMenuBoxs.find("li");
            this.systemtime = $("#systemtime");
            this.switchbar = $("#switchbar");
            this.topmenubar = $("#header_topmenu_bar");
            this.pageRequiredMenu = $("#page_required_menu");
            this.pageloading = $("#page_loading").hide();
        },
        currentWindowSize: function () {
            return { height: $(window).height(), width: $(window).width() }
        },
        setLayout: function () {
            this._left_menu_width = this.leftMenu.outerWidth(); //菜单宽度
            this._slide_hidden_len = this.bodyInnerBox.position().left;
            var win = this.currentWindowSize();
            if (this._slide_hidden_len == 0) {
                var _unhide_menu_righ_box_w = win.width - this._left_menu_width + (-this._slide_hidden_len);
                this.bodyInnerBox.width(win.width); //设置body内容容器的宽度同body相同即为显示器窗口宽度
                this.rightBox.width(_unhide_menu_righ_box_w);
            }
            else {//未隐藏左侧菜单
                var _hide_menu_righ_box_w = win.width + (-this._slide_hidden_len);
                this.bodyInnerBox.width(win.width + Math.abs(this._slide_hidden_len));
                this.rightBox.width(win.width); //始终保持与显示器尺寸相同
            }
            var _height = win.height - (this.topBox.outerHeight() + this.footBox.outerHeight());
            this.rightBox.height(_height);
            this.rightBox.find("iframe").height(_height);
            this.leftMenu.height(_height);
            this.leftMenuBoxs.height(_height);
            this.body.height(_height);
            this.container.height(win.height);
            this.pageRequiredMenu.height(_height);
            this.pageloading.height(_height);
            var loadimg = this.pageloading.find("img");
            var left = (this.pageloading.width() - loadimg.width()) / 2;
            var top = (this.pageloading.height() - loadimg.height()) / 2;
            loadimg.css({ left: left, top: top });

        },
        findParent: function (jq_dom_obj, parent_level) {
            if (!parent_level) return jq_dom_obj.parent();
            else {
                var _parent;
                for (var i = 0; i < parent_level; i++) {
                    if (!_parent)
                        _parent = jq_dom_obj.parent();
                    else
                        _parent = _parent.parent();
                }
                return _parent;
            }
        },
        leftMenuSwitch: function () {
            if (typeof (this.menu_animate_play_over) == "undefined") this.menu_animate_play_over = true;
            if (this.menu_animate_play_over) {
                var that = this,
                 _mlw = that.leftMenu.width(),
                _rbw = that.body.width(),
                _rw = that.rightBox.width(),
                _l = that.bodyInnerBox.position().left,
                _speed = "fast";
                that.menu_animate_play_over = false;
                if (_l == 0) {
                    var _newwidth = _rbw + _mlw;
                    that.bodyInnerBox.stop().animate({ left: -_mlw, width: _newwidth }, _speed);
                    that.rightBox.stop().animate({ width: _rw + _mlw }, _speed, function () {
                        that.menu_animate_play_over = true;
                    });
                    that.topmenubar.stop().animate({ left: -_mlw }, _speed, function () {
                        that.switchbar.addClass("current");
                    });

                }
                else {
                    that.bodyInnerBox.stop().animate({ left: 0, width: _rbw }, _speed);
                    that.rightBox.stop().animate({ width: _rw - _mlw }, _speed, function () {
                        that.menu_animate_play_over = true;
                    });
                    that.topmenubar.stop().animate({ left: 0 }, _speed, function () {
                        that.switchbar.removeClass("current");
                    });
                    if (that.leftMenu.position().left > 0)
                        that.leftMenu.stop().animate({ left: 0 }, _speed)
                }
            }
        },
        /**
        * 设置非跨域窗口打开时的标题，本方法供iframe调用页面调用
        * @method setCurrentOpenedWindowTitle
        * @param {string} key 取得一个窗口iframe的key 它为一个iframe 应用的src
        * @param {string} title 将要设置的标题
        * @param {string} width 将要设置的标题
        * @param {string} height 将要设置的标题
        */
        setCurrentOpenedWindowTitle: function (key, title) {
            //            var set_xwindow_callback = $("body").find("iframe[src='" + key + "']").data("set_xwindow_callback");
            //            if (typeof (set_xwindow_callback) == "function") set_xwindow_callback(title);
            //            that.checkToSetOrEmptyDeskTitleBar();
        },
        /**
        * 页面请求窗口显示菜单
        * @method setXdialogMenu
        * @param {string} winid 窗口ID 为 兼容 desktop布局 此处不使用
        * @param {json}   menus 菜单描述对象
        * @param {string} from 来自主引导页面的地址
        */
        setXdialogMenu: function (winid, _menus, from) {//有对应页面调用
            var that = this;
            var htmlpool = [];
            if (_menus && _menus.length !== undefined && _menus.length > 0) {
                htmlpool.push('<ul class="menus-fun-children">');
                for (var i = 0; i < _menus.length; i++) {
                    var group = _menus[i];
                    var replace_keys = {};
                    for (var t = 0; t < group.children.length; t++) {
                        var link_A = group.children[t];
                        if (link_A) {
                            var src = link_A.src;
                            var text = link_A.text;
                            if (!(src && typeof (src) == "string")) {
                                src = "javascript:void(0)";
                            }
                            var li = '<li linkbtn="a"><a href="' + src + '" target="main">' + text + '</a></li>';
                            htmlpool.push(li);
                        }
                    }
                }
                htmlpool.push('</ul>');
                that.pageRequiredMenu.empty();
                that.pageRequiredMenu.attr("from", from);
                that.pageRequiredMenu.html(htmlpool.join(""));
                that.pageRequiredMenu.show().trigger("custom_animate_show");
                setTimeout(function () {
                    that.pageRequiredMenu.trigger("custom_animate_hidden");
                }, 3000);
            }
        }
        ,
        /**
        * 设置对话框参数
        * @method setXdialogMenu
        * @param {string} winid 窗口ID 为 兼容 desktop布局 此处不使用
        * @param {json}   params 菜单描述对象
        */
        setXdialogParam: function (id, params) {
            if (typeof (param) == undefined) return;
            this.iframe.data("iframe_params", params);

            var url_param = new Array();
            $.each(params, function (key, value) {
                if (typeof (value) == "string" || typeof (value) == "number")
                    url_param.push(key + "=" + value);
            });
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
            var url_param_str = url_param.join("&");
            if (url_param_str != "")
                this.pageRequiredMenu.find("li a[href!='']").each(function () {
                    var a = $(this), href = a.attr("href"), canset = true;
                    if (href && href != "javascript:void(0)" && _checkAppendedParam(href)) {
                        a.attr("href", href + (href.indexOf("?") >= 0 ? "&" : "?") + url_param_str);
                    }
                });
        },
        /**
        * 获取对话框参数
        * @method getXdialogParam
        * @param {string} winid 窗口ID
        */
        getXdialogParam: function (winid) {
            return this.iframe.data("iframe_params");
        },
        pageLoadComplete: function () {
            this.pageloading.fadeOut("fast");
        },
        pageLoadStart: function () {
            this.pageloading.show();
        },
        checkClearPageRequiredMenu: function (from) {
            var _from = this.pageRequiredMenu.attr("from");
            if (from !== _from && this.pageRequiredMenu.find("a[href^='" + from + "']").size() == 0) {
                this.pageRequiredMenu.empty().hide() ;
            }
        }
    };
    $.startDeskTop = function (langlist) {
        var desk = new _DeskTop(langlist);
        return desk;
    }
})(jQuery, XDialog)