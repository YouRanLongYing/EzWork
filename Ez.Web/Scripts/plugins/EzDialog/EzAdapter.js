$(function () {
    if (top && top.desk && typeof (top.desk.setCurrentOpenedWindowTitle) == 'function') {
        var thisurl = location.href;
        thisurl = thisurl.replace("http://", "");
        var index = thisurl.indexOf("/");
        thisurl = thisurl.substring(index);
        top.desk.setCurrentOpenedWindowTitle(thisurl, $(document).find("title").html());
    }
    /**
    页面记载完成关闭加载层
    */
    if (top && top.desk && typeof (top.desk.pageLoadComplete) == 'function') {
        top.desk.pageLoadComplete();
    }
})
/**
设置对话框toolbar的搜索框
*/
var setToolBar = function (param) {
    if (top && top.desk && typeof (top.desk.setEzDialogToolBar) == 'function') {
        top.desk.setEzDialogToolBar(cur__EzDialogid__, param);
    }
}
/**
设置对话框的菜单
*/
var setMenu = function (mesnus, from) {
    if (top && top.desk && typeof (top.desk.setEzDialogMenu) == 'function') {
        top.desk.setEzDialogMenu(cur__EzDialogid__, mesnus, from);
    }
}
/**
设置对话框的参数
*/
var setParam = function (param) {
    if (top && top.desk && typeof (top.desk.setEzDialogParam) == 'function')
        top.desk.setEzDialogParam(cur__EzDialogid__, param);
}
/**
获取对话框的参数
*/
var getParam = function () {
    if (top && top.desk && typeof (top.desk.getEzDialogParam) == 'function')
        return top.desk.getEzDialogParam(cur__EzDialogid__);
}
/**
判断是否需要清楚上一个页面注入的菜单
*/
if (top && top.desk && typeof (top.desk.checkClearPageRequiredMenu) == 'function') {
    top.desk.checkClearPageRequiredMenu("@ViewBag.RequiredMenuFrom");
}