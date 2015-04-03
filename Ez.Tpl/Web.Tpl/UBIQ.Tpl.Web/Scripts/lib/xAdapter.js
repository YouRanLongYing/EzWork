$(function () {
    if (top && top.desk && typeof (top.desk.setCurrentOpenedWindowTitle) == 'function') {
        var thisurl = location.href;
        thisurl = thisurl.replace("http://", "");
        var index = thisurl.indexOf("/");
        thisurl = thisurl.substring(index);
        top.desk.setCurrentOpenedWindowTitle(thisurl, $(document).find("title").html());
    }
})
/**
设置对话框toolbar的搜索框
*/
var setToolBar = function (param) {
    if (top && top.desk && typeof (top.desk.setXdialogToolBar) == 'function') {
        top.desk.setXdialogToolBar(cur__xdialogid__, param);
    }
}
/**
设置对话框的菜单
*/
var setMenu = function (mesnus, from) {
    if (top && top.desk && typeof (top.desk.setXdialogMenu) == 'function') {
        top.desk.setXdialogMenu(cur__xdialogid__, mesnus, from);
    }
}
/**
设置对话框的参数
*/
var setParam = function (param) {
    if (top && top.desk && typeof (top.desk.setXdialogParam) == 'function')
        top.desk.setXdialogParam(cur__xdialogid__, param);
}
/**
获取对话框的参数
*/
var getParam = function () {
    if (top && top.desk && typeof (top.desk.getXdialogParam) == 'function')
        return top.desk.getXdialogParam(cur__xdialogid__);
}
/**
页面记载完成关闭加载层
*/
$(function () {
    if (top && top.desk && typeof (top.desk.pageLoadComplete) == 'function')
        top.desk.pageLoadComplete();
})

/**
判断是否需要清楚上一个页面注入的菜单
*/
if (top && top.desk && typeof (top.desk.checkClearPageRequiredMenu) == 'function') {
    top.desk.checkClearPageRequiredMenu("@ViewBag.RequiredMenuFrom");
}