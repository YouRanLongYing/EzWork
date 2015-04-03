(function ($) {
    var _XGrid = {};
    _XGrid = function (target, config) {
        this.grid = target;
        target.jqGrid(config); //.navGrid("#pager5", { edit: false, add: false, del: false });
    };
    _XGrid.prototype = {
        consoleMsg: function (msg, throwExp) {
            console.log(msg);
            if (throwExp) {
                throw new Error(msg);
            }
        },
        setColumnControl: function (columnName, btns, eventHandler) {
            if (!columnName || columnName == "") {
                this.consoleMsg("请指定设置的列名！", true);
                return;
            }
            var ids = this.grid.jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                var btn_array = [];
                for (var t = 0; t < btns.length; t++) {
                    var btn_descp = btns[t];
                    var type = btn_descp.type, text = btn_descp.text, cmd = btn_descp.cmd;
                    var click = eventHandler ? "onclick=\"" + eventHandler + "('" + cl + "','" + cmd + "')\"" : "javascript:void(0)";
                    switch (btn_descp.type) {
                        case "button":
                            {
                                btn_array.push("<input style='height:22px;width:30px;margin-left:1px;' type='button' value='" + text + "' " + click + "  />"); //
                            } break;
                        case "link":
                            {
                                btn_array.push("<a href='javascript:void(0)' style='display:block;height:22px;width:30px;margin-left:1px;float:left;' " + click + " >" + text + "</a>");
                            } break;
                    }
                }
                var json = {};
                json[columnName] = btn_array.join("");
                this.grid.jqGrid('setRowData', ids[i], json);
            }
        }
    };
    $.fn.xGrid = function (config) {
        var defaultcfg = {
            datatype: config.dataType || "json",
            rowNum: config.rowNum || 10,
            rowList: config.rowList || [1, 10, 20, 30],
            pager: "#pager",
            //width: "100%",
            height: 'auto',
            shrinkToFit: true,
            autowidth: true
        };
        var opts = $.extend({}, defaultcfg, config);
        if (opts.url === undefined || opts.url == "") return;
        var _columns = opts.columns;
        opts.columns = null;
        delete opts.columns;
        opts.colNames = [];
        opts.colModel = [];
        for (var index in _columns) {
            var col = _columns[index];
            opts.colNames[index] = col.header;
            col.header = null;
            delete col.header;
            opts.colModel[index] = col;
        }
        opts.jsonReader = { root: "rows", total: "total", page: "page", records: "records", repeatitems: false }
        opts.postData = opts.postData || {};
        opts.postData["xgrid"] = "xgrid"; /*用于后台请求识别*/
        var loadComplete;
        if (opts.loadComplete !== undefined && typeof (opts.loadComplete) == "function")
            loadComplete = opts.loadComplete;
        opts.loadComplete = function (req) {
            if (req._sys_logined == false) {
                if (confirm("您可能长时间未登录，登录已失效！\r\n请重新登录否则将关闭窗口！")) {
                    top.location.href = "/";
                }
                else {
                    window.close();
                }
            }
            else {
                if (loadComplete) loadComplete(req);
            }
        }

        if (typeof (opts.width) == 'string' && opts.width == "100%") {
            //opts.width = $(window).width() - 20;
        };
        /*
        colNames: ['Inv No', 'Date', 'Client', 'Amount', 'Tax', 'Total', 'Notes'],
        colModel: [
        { name: 'id', index: 'id', width: 55 },
        { name: 'invdate', index: 'invdate', width: 90 },
        { name: 'name', index: 'name', width: 100 },
        { name: 'amount', index: 'amount', width: 80, align: "right" },
        { name: 'tax', index: 'tax', width: 80, align: "right" },
        { name: 'total', index: 'total', width: 80, align: "right" },
        { name: 'note', index: 'note', width: 150, sortable: false }
        ],
        */
        var datatable = $(this);
        datatable.after('<div id="pager"></div>');
        return (new _XGrid(datatable, opts));
    }
})(jQuery)