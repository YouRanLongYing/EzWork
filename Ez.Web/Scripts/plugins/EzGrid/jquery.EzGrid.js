/*
author:kongjing
date:2015.03.30
function:for pagenation
example:
<!--定义分页数据的来源和分页参数-->
<table id="viewlist" ezgrid="yes" url="/zone/view_async" ezgrid-autoload="true" ezgrid-rows="1" ezgrid-page="1">
<thead >
<!--定义分页信息的索引和其他相关参数-->
<tr>
<th grid-header="zone_id"     grid-header-sortable="false" style="width:50px;">编号</th>
<th grid-header="zone_name"   grid-header-sortable="false">名称</th>
<th grid-header="dev_time"    grid-header-sortable="true" style="width:300px;">建造时间</th>
<th grid-header="finish_time" grid-header-sortable="false"  style="width:300px;">竣工时间</th>
<th grid-header="zone_id"     grid-header-sortable="false">操作</th>
</tr>
</thead>
<tbody>
<!--用于显示分页数据的模板-->
<tr>
<td>${zone_id}</td>
<td>${zone_name}</td>
<td formater="yyyy-MM-dd">${dev_time}</td>
<td formater="yyyy-MM-dd">${finish_time}</td>
<td style="text-align:center;"><a href="/zone/edit?zone_id=${zone_id}" >编辑</a></td>
</tr>

</tbody>
<!--若需要分页按钮则不需定义-->
<tfoot></tfoot>
</table>
*/
(function ($) {
    //扫描页面需要显示列表的标签
    var ScanTable = function (jqTableDom) {
        var that = this;

        that.jqTableDom = jqTableDom;
        that.id = jqTableDom.attr("id");
        if (that.id === undefined) {
            that.id = "ez-list-grid-" + Math.random().replace(".", "");
            that.jqTableDom.attr("id", that.id);
        }

        var param = that.jqTableDom.attr("igrid");
        param = (new Function("return " + param))();
        param = $.extend({ url: "", autoload: true, rows: 20, page: 1 }, param);

        that.url = param.url;
        that.page = param.page;
        that.rows = param.rows;

        that.jqWrap = $("<div class=\"ez-igrid-wraper\" style=\"position:relative;\"></div>");
        that.jqTableDom.wrap(that.jqWrap);
        that.jqLoading = $("<div class=\"ez-igrid-loading\" style=\"position:absolute;color:#ff9966;font-size:20px;\"><i class=\"am-icon-spinner am-icon-spin\"></i>loading...</div>");
        that.jqLoading.hide();
        that.jqTableDom.after(that.jqLoading);
        if (that._setENVParams() && param.autoload) {
            var queryParam = that.jqTableDom.attr("igrid-param");
            that.query((new Function("return " + queryParam))());
        }
    }
    ScanTable.prototype = {
        __setPagenationHtmlContent: function (page, pages, records) {
            var pagNavHtml = [];
            if (pages > 1) {
                pagNavHtml.push("<ul class='am-pagination am-pagination-right ez-igrid-nav'>");
                pagNavHtml.push(page == 1 ? "<li class=\"am-disabled\"><a href=\"javascript:void(0)\"><span class=\"am-icon-angle-double-left\" prev=\"yes\"></span></a></li>" : "<li><a href=\"javascript:void(0)\"><span class=\"am-icon-angle-double-left\" prev=\"yes\"></span></a></li>");
                for (var i = 1; i <= pages; i++) {
                    pagNavHtml.push(page == i ? "<li class=\"am-active\"><a href=\"javascript:void(0)\">" + i + "</a></li>" : "<li><a href=\"javascript:void(0)\" nav=\"yes\">" + i + "</a></li>");
                }
                pagNavHtml.push(page == pages ? "<li class=\"am-disabled\"><a href=\"javascript:void(0)\"><span class=\"am-icon-angle-double-right\" next=\"yes\"></span></a></li>" : "<li><a href=\"javascript:void(0)\"><span class=\"am-icon-angle-double-right\" next=\"yes\"></span></a></li>");
                pagNavHtml.push("<li class=\"ez-igrid-nav-goto\"><input type='number' value=\"" + page + "\"><button>GO</button></li></ul>")
            }
            var nav = "<tr><td colspan='" + this.headerDescript["headerDoms"].length + "'>" + pagNavHtml.join("") + "<ul class='ez-igrid-nav-info'><li>第" + page + "/" + pages + "页</li><li>共" + records + "条数据</li></ul></td></tr>";
            this.footerDescript["jqFooterDom"].html(nav);
            this.__bindNavEvent();
        },
        __setListHtmlContent: function (data) {
            var that = this;
            var bodyAppend = [];
            var trContentTml = that.bodyDescript.jqTrDom[0].outerHTML;
            this.pages = data.total;
            this.page = data.page;
            this.records = data.records;
            var td = [];
            for (var i = 0; i < data.rows.length; i++) {
                var row = data.rows[i];
                var rowContent = trContentTml;
                $.each(that.headerDescript["headerDoms"], function (i) {
                    var th = $(that.headerDescript["headerDoms"][i]);
                    var param = th.data("param");
                    if (!td[i]) {
                        td[i] = that.bodyDescript.jqTrDom.find("td:eq(" + i + ")");
                    }
                    var cell = row[param.index];
                    var format = td[i].attr("formater");
                    if (format) {
                        cell = cell.toDateString(format);
                    }
                    var reg = new RegExp("\\$\{" + param.index + "\}", "gi");
                    rowContent = rowContent.replace(reg, cell);
                })
                bodyAppend.push(rowContent);
            }
            that.bodyDescript["jqBodyDom"].html(bodyAppend.join("\r\n"));
        },
        __getHeaderDescript: function () {
            this.headerDescript = {};
            var firstTh = this.jqTableDom.find("thead").first();
            this.headerDescript["headerDoms"] = firstTh.find("th[grid-header]");
            if (this.headerDescript["headerDoms"].length == 0) return false;
            this.headerDescript["headerDoms"].each(function () {
                var th = $(this);
                var descript = { "index": th.attr("grid-header"), "sortable": (th.attr("grid-header-sortable") == 'true') };
                $(this).data("param", descript);
                if (descript["sortable"]) {
                    th.html("<span class=\"ez-igrid-sortable\">" + th.text() + "<i class=\"am-icon-sort-alpha-desc\"></i></span>");
                }
                else {
                    th.html("<span>" + th.text() + "</span>");
                }
            })
            return this.headerDescript;
        },
        __getBodyDescript: function () {
            this.bodyDescript = {};
            this.bodyDescript["jqBodyDom"] = this.jqTableDom.find("tbody").first();
            this.bodyDescript["jqTrDom"] = this.bodyDescript["jqBodyDom"].find("tr").first();
            this.bodyDescript["jqBodyDom"].empty();
            return this.bodyDescript;
        },
        __getFooterDescript: function () {
            this.footerDescript = {};
            this.footerDescript["jqFooterDom"] = this.jqTableDom.find("tfoot").first();
            return this.footerDescript;
        },
        __bindHeaderEvent: function () {
            var that = this;
            this.headerDescript["headerDoms"].filter("[grid-header-sortable='true']").click(function () {
                var param = $(this).data("param");
                if (param && param["sortable"] && param["index"]) {
                    that.queryParam = that.queryParam || {};
                    that.queryParam["sidx"] = param["index"];
                    var sort = that.queryParam["sord"] == "desc" ? "asc" : "desc";
                    $(this).find("i").removeClass("am-icon-sort-alpha-" + that.queryParam["sord"]).addClass("am-icon-sort-alpha-" + sort);
                    that.queryParam["sord"] = sort;
                    that.query(that.queryParam);
                }
            })
        },
        __bindNavEvent: function () {
            var that = this;
            this.footerDescript["jqFooterDom"].find("ul.ez-grid-nav a").click(function (e) {
                var btn = $(e.target);

                if (btn.attr("prev") == "yes") {
                    that.prevPage();
                }
                else if (btn.attr("next") == "yes") {
                    that.nextPage();
                }
                else if (btn.attr("nav") == "yes") {
                    that.goto(btn.text());
                }
            })
            this.jqTableDom.find("tfoot li.ez-grid-nav-goto>button").click(function () {
                var gopage = $(this).prev().val();
                that.goto(gopage);
            });

        },
        _setENVParams: function () {
            if (this.url !== undefined) {
                if (!this.jqTableDom.data("headerDescript_" + this.id)) {
                    this.jqTableDom.data("headerDescript_" + this.id, this.__getHeaderDescript());
                    this.jqTableDom.data("bodyDescript_" + this.id, this.__getBodyDescript());
                    this.jqTableDom.data("footerDescript_" + this.id, this.__getFooterDescript());
                    this.__bindHeaderEvent();
                }
                else {
                    this.headerDescript = this.jqTableDom.data("headerDescript_" + this.id);
                    this.bodyDescript = this.jqTableDom.data("bodyDescript_" + this.id)
                    this.footerDescript = this.jqTableDom.data("footerDescript_" + this.id)
                }
                this.jqTableDom.addClass("am-table am-table-bordered am-table-striped am-table-hover ez-igrid");
                return true;
            }
            return false;
        },
        _render: function (data, queryParam) {
            if (data.rows.length > 0) {
                this.queryParam = queryParam;
                this.__setListHtmlContent(data);
                if (!this.footerDescript["jqFooterDom"].data("setted")) {
                    this.footerDescript["jqFooterDom"].data("setted", true);
                    this.__setPagenationHtmlContent(this.page, this.pages, this.records);
                }
                this.footerDescript["jqFooterDom"].find("ul").show();
            }
            else {
                this.bodyDescript["jqBodyDom"].html("<tr><td colspan=\"" + this.headerDescript["headerDoms"].length + "\"><span class=\"ez-igrid-nothing\">未查询到数据!</span></td></tr>");
                this.footerDescript["jqFooterDom"].hide();
            }
        },
        _wait: function (over) {
            if (over) {
                this.jqLoading.fadeOut("slow");
            }
            else {
                var wrap_h = this.jqTableDom.height();
                var wrap_w = this.jqTableDom.width();
                var load_h = this.jqLoading.height();
                var load_w = this.jqLoading.width();
                this.jqLoading.css({ "top": (wrap_h - load_h) / 2, "left": (wrap_w - load_w) / 2 }).fadeIn("fast");
            }
        },
        query: function (queryParam) {
            var that = this;
            queryParam = queryParam || {};
            queryParam["page"] = queryParam["page"] || this.page;
            queryParam["rows"] = queryParam["rows"] || this.rows;
            queryParam["sidx"] = queryParam["sidx"] || "";
            queryParam["sord"] = queryParam["sord"] || "desc";
            if (that.footerDescript["jqFooterDom"].data("setted")) {
                that.__setPagenationHtmlContent(queryParam["page"], that.pages, that.records);
            }
            that._wait(false);
            $.ajax({
                url: this.url,
                dataType: "json",
                type: "GET",
                data: queryParam,
                success: function (data) {
                    that._render(data, queryParam);
                    that._wait(true);
                }
            })
        },
        goto: function (page) {
            page = parseInt(page, 10);
            if (isNaN(page)) return;
            if (this.pages && page > this.pages) return;
            if (page <= 0) return;
            this.queryParam["page"] = page;
            this.query(this.queryParam);
        },
        nextPage: function () {
            this.goto(this.queryParam["page"] + 1);
        },
        prevPage: function () {
            this.goto(this.queryParam["page"] - 1);
        },
        refresh: function () {
            this.footerDescript["jqFooterDom"].data("setted", false);
            this.query(this.queryParam);
        }
    }

    $.fn.renderEzGrid = function () {
        $(this).data("EzGridApi",new ScanTable($(this)));
    }
})(jQuery)