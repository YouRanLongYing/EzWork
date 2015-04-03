document.write('<script src="/Scripts/lib/Upload.js" type="text/javascript"></script>');
$(function () {
    $("body a").on("click", function () {
        var _desk = top.desk || desk;
        if(!_desk||!_desk.pageloading) return;
        _desk.pageloading.hide();
        var href = $(this).attr("href");
        href = href == "javascript:void(0)" ? $(this).data("src") : href;
        if ((href && /^http:\/\/|https:\/\//g.test(href)) || !href) return;
        if (_desk && typeof (_desk.pageLoadStart)) _desk.pageLoadStart();
    });
    setTimeout(function(){
    //关于上传
    var uploadFn = window["uploadFile"];
        $("a[tagway='upload']").each(function(i){
                                var uptag = $(this); 
                                uptag.attr("id","jquery_file_up_"+i);
                                var dir =uptag.data("dir");
                                var auto =uptag.data("auto");
                                var success =uptag.data("success");
                                var progress =uptag.data("progress");
                                var uptype =uptag.data("uptype");
                                var allownum =uptag.data("allownum");
                                var width = uptag.attr("width");
                                var height = uptag.attr("height");
                                var text = uptag.text();
                                var classname = uptag.attr("class");
                                uploadFn(uptag,dir||"",allownum||1,uptype||"image",window[success],auto,window[progress],classname,text||"browser",width||80,height||20);
        });
    },100);
})

String.prototype.toDateString = function (formatter) {
    var value =$.trim(this.concat());
    if (!/^\d{4}[-|.|/]\d{1,2}[-|.|/]\d{2}\s+([0-23]{1,2}:[0-59]{1,2}:[0-59]{1,2})?(\.\d+)?$/g.test(value)) return value;

                formatter =formatter||"yyyy-MM-dd";
                var _date = new Date(value);
                formatter.replace("yyyy",_date.getFullYear());
                formatter.replace("MM",_date.getFullYear());

                var _y = _date.getFullYear();
                var _m = _date.getMonth() + 1;
                var _d = _date.getDate();

                var _dd = _date.getDay();

                var _hh = _date.getHours();
                var _mm = _date.getMinutes();
                var _ss = _date.getSeconds();

                _hh =_hh<10?"0"+_hh:_hh;
                _mm =_mm<10?"0"+_mm:_mm;
                _ss =_ss<10?"0"+_ss:_ss;

                formatter=formatter.replace("yyyy",_y);
                formatter=formatter.replace("MM",_m);
                formatter=formatter.replace("dd",_d);
                formatter=formatter.replace("HH",_hh);
                formatter=formatter.replace("mm",_mm);
                formatter=formatter.replace("ss",_ss);
                return formatter;
}
//处理框架后台相应的脚本请求
window["responseData"] = function (response, callback) {
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
                if (response.xdialogid !== undefined && typeof (response.xdialogid) == "string" && response.xdialogid != "" && top.desk && top.desk.dialog && typeof (top.desk.dialog.findXdialog) == "function") {
                    win_handler = top.desk.dialog.findXdialog(response.xdialogid);
                }

                if (response.redirect && response.redirect != "") {
                    if (response.redirectToBroswerTab) {
                        if (top) top.location.href = response.redirect;
                        else location.href = response.redirect;
                    }
                    else if (win_handler && win_handler.info && win_handler.info.iframe && win_handler.info.iframe.is("iframe")) {
                        win_handler.info.iframe.attr("src", response.redirect);
                        response.isCloseXdialog = false
                    }
                }
                if (response.isCloseXdialog && win_handler) {
                    win_handler.close();
                }
            }
        }
};
