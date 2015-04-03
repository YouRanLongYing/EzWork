//前端初始化
$(function () {
    window["aboutPageLoad"]();
    window["aboutUpload"]($("a[tagway='upload']"))
    window["aboutEditor"]($("textarea[editor='yes']"));
    window["aboutIGrid"]();
    window["ddlefect"]();
})

String.prototype.toDateString = function (formatter) {
    var value =$.trim(this.concat());
    if (!/^\d{4}[-|.|/](0[1-9]|1[0-2]|[1-9])[-|.|/](0[1-9]|[1-2][0-9]|3[0-1]|[1-9])\s+((0[0-9]|[1-2][0-9]|[0-9])(:(0[0-9]|[1-5][0-9]|[0-9])){2})?$/g.test(value)) return value;

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
