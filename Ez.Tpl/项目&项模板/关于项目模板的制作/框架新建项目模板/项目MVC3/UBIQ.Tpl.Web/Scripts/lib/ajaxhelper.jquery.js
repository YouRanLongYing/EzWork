(function ($, tmp__xdialogid__) {

    $.request = function (url, datatype, type, data, callback, errorcallback) {
        var tmp__xdialogid__ = window["cur__xdialogid__"];
        if (tmp__xdialogid__ && typeof (tmp__xdialogid__) == "string") {
            url = url + (url.indexOf("?") >= 0 ? "&" : "?") + "xdialogid=" + tmp__xdialogid__;
        }
        $.ajax({
            url: url,
            dataType: datatype || "json",
            type: type || "GET",
            data: data || {},
            success: function (response) {
                window["responseData"](response, callback);
            },
            error: function (exp) {
                //alert("抱歉请求发生了异常");
            }
        })
    };
    $.fn.initSubmit = function (callback, errorcallback, translateurl) {
        return this.each(function () {
            var form = $(this);
            var button = form.find(":submit").eq(0);
            button.on("click", function () {
                $.request(form.attr("action"), "", form.attr("method"), form.serialize(), callback, errorcallback);
                return false;
            });
        })
    }
    $.submit = function (address, param, callback, errorcallback) {
        param = param || {};
        $.request(address, "", "POST", param, callback, errorcallback);
    }
    $.query = function (address, param, callback, errorcallback) {
        param = param || {};
        $.request(address, "", "GET", param, callback, errorcallback);
    }

    $(function () {
        $("form").each(function () {//[isajax='True']
            var $from = $(this);

            var parent = $from.parent();
            if (parent && parent.hasClass("context")) {
                parent.parent().find(".form-alpha-cover").height(parent.height());
                parent.parent().find(".form-alpha-cover").width(parent.width());
            }
            if ($from.attr("isajax") == "True") {
                $from.find("input[call-close-win]").click(function () {
                    var tmp__xdialogid__ = window["cur__xdialogid__"];
                    if (typeof (tmp__xdialogid__) == "string" && tmp__xdialogid__ != "" && top.desk && top.desk.dialog && typeof (top.desk.dialog.findXdialog) == "function") {
                        var win_handler = top.desk.dialog.findXdialog(tmp__xdialogid__);
                        if (win_handler) win_handler.close();
                    }

                })
                $from.data("_customerSubmit", function () {
                    var trustAction = $from.attr("trustAction");
                    var callback = window[$from.attr("callback")];
                    var errorcallback = window[$from.attr("errorcallback")];
                    if (trustAction || trustAction == "False") {
                        if ($from.attr("method").toUpperCase() == "POST") {
                            $.submit($from.attr("action"), $from.serialize(), callback, errorcallback);
                        }
                        else {
                            $.query($from.attr("action"), $from.serialize(), callback, errorcallback);
                        }
                    }
                    else {
                        $.request($from.attr("action"), "", $from.attr("method"), $from.serialize(), callback, errorcallback);
                    }
                    return false;
                });
            }
        })
    })
})(jQuery)