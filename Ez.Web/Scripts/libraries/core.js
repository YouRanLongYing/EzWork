//AJAX
(function ($, tmp__EzDialogid__) {
    window["Ez"] = $.Ez = {};
    //扩展jquery的show方法 以满足表单验证效果要求
    var oldFnShow = jQuery.fn.show;
    jQuery.fn.show = function () {
        var enable = true;
        if ($(this).is("div") && $(this).hasClass("am-popover") && $(this).hasClass("am-popover-top")) {
            enable = $(this).data("am-pop-tipExt-enable");
            console.log("拦截了表单验证时用于显示tip的动作，并将其进行了扩展");
        }
        if (enable) {
            return oldFnShow.apply(this, arguments);
        }
        return $(this);
    };
    var request = function (url, datatype, type, data, callback, errorcallback) {
        var tmp__EzDialogid__ = window["cur__EzDialogid__"];
        if (tmp__EzDialogid__ && typeof (tmp__EzDialogid__) == "string") {
            url = url + (url.indexOf("?") >= 0 ? "&" : "?") + "EzDialogid=" + tmp__EzDialogid__;
        }
        errorcallback = errorcallback || function () { };
        $.ajax({
            url: url,
            dataType: datatype || "json",
            type: type || "GET",
            data: data || {},
            success: function (response) {
                Ez.AsyncDataProcessor(response, callback);
            },
            error: function (exp) {
                //alert("抱歉请求发生了异常");
                errorcallback();
            }
        })
    };
    $.fn.initSubmit = function (callback, errorcallback, translateurl) {
        return this.each(function () {
            var form = $(this);
            var button = form.find(":submit").eq(0);
            button.on("click", function () {
                request(form.attr("action"), "", form.attr("method"), form.serialize(), callback, errorcallback);
                return false;
            });
        })
    }
    Ez.AsyncDataProcessor = function (response, callback) {
        if (response) {
            var nextgo;
            if (response.success && typeof (callback) == "function") {
                nextgo = callback(response.data);
            }
            else if (typeof (errorcallback) == "function") {
                nextgo = errorcallback(response);
            }
            if (!nextgo) {
                nextgo = typeof (nextgo) == "undefined";
            }
            if (nextgo) {
                if (response.errorMsg !== undefined && response.errorMsg != "") {
                    alert(response.errorMsg);
                }
                else if (response.waringMsg !== undefined && response.waringMsg != "") {
                    alert(response.waringMsg);
                }
                else if (response.message !== undefined && response.message != "") {
                    alert(response.message);
                }
                var win_handler;
                if (response.EzDialogid !== undefined && typeof (response.EzDialogid) == "string" && response.EzDialogid != "" && top.desk && top.desk.dialog && typeof (top.desk.dialog.findEzDialog) == "function") {
                    win_handler = top.desk.dialog.findEzDialog(response.EzDialogid);
                }

                if (response.redirect && response.redirect != "") {
                    if (response.redirectToBroswerTab) {
                        if (top) top.location.href = response.redirect;
                        else location.href = response.redirect;
                    }
                    else if (win_handler && win_handler.info && win_handler.info.iframe && win_handler.info.iframe.is("iframe")) {
                        win_handler.info.iframe.attr("src", response.redirect);
                        response.isCloseEzDialog = false
                    }
                    else {
                        location.href = response.redirect;
                    }
                }
                if (response.isCloseEzDialog && win_handler) {
                    win_handler.close();
                }
            }
        }
    };
    Ez.submit = function (address, param, callback, errorcallback) {
        param = param || {};
        request(address, "", "POST", param, callback, errorcallback);
    }
    Ez.query = function (address, param, callback, errorcallback) {
        param = param || {};
        request(address, "", "GET", param, callback, errorcallback);
    }

    $(function () {
        $("form").each(function () {//[isajax='True']
            var $from = $(this);
            var submit_btn_ico = $from.find("[type='submit'] i").eq(0);
            var parent = $from.parent();
            if ($from.attr("isajax").toLowerCase() == "true") {
                $from.find("input[call-close-win]").click(function () {
                    var tmp__EzDialogid__ = window["cur__EzDialogid__"];
                    if (typeof (tmp__EzDialogid__) == "string" && tmp__EzDialogid__ != "" && top.desk && top.desk.dialog && typeof (top.desk.dialog.findEzDialog) == "function") {
                        var win_handler = top.desk.dialog.findEzDialog(tmp__EzDialogid__);
                        if (win_handler) win_handler.close();
                    }

                })
                $from.data("_customerSubmit", function () {
                    var trustAction = $from.attr("trustAction");
                    var callback_fn = window[$from.attr("callback")];
                    var error_fun = window[$from.attr("errorcallback")];
                    var callbackagent = function (data) {
                        submit_btn_ico.removeClass("am-icon-spinner").removeClass("am-icon-spin").addClass("am-icon-key");
                        if (typeof (callback_fn) == "function") {
                            callback_fn(data);
                        }
                    }
                    var errorcallbackagent = function (a, b, c, d, e) {
                        submit_btn_ico.removeClass("am-icon-spinner").removeClass("am-icon-spin").addClass("am-icon-key");
                        if (typeof (error_fun) == "function") {
                            error_fun(a, b, c, d, e);
                        }
                    }

                    submit_btn_ico.removeClass("am-icon-key").addClass("am-icon-spinner").addClass("am-icon-spin");

                    if (trustAction || trustAction == "False") {
                        if ($from.attr("method").toUpperCase() == "POST") {
                            UBIQ.submit($from.attr("action"), $from.serialize(), callbackagent, errorcallbackagent);
                        }
                        else {
                            UBIQ.query($from.attr("action"), $from.serialize(), callbackagent, errorcallbackagent);
                        }
                    }
                    else {
                        request($from.attr("action"), "", $from.attr("method"), $from.serialize(), callbackagent, errorcallbackagent);
                    }
                    return false;
                });
            }
        })
    })
})(jQuery)
