/* 
$(function () {
    UploadFile('fileUploadFile', '/Uploads/Administrator/Memorial/Scriptures/', 1, 'imgswf', fun);
});
function fun(event, queueID, fileObj, response, data) {
    if (response != "") {
        alert('上传成功');
        //$("#隐藏控件ID").val(filePath + response);
        $("#显示上传成功后地址的html标签id").val("/Uploads/Administrator/Memorial/Scriptures/" + response);
        //$("#imgSmallImage").attr("src", "/Uploads/Administrator/Memorial/Product/Images/" + response);
        //showInfo("成功上传 " + response, true); //showInfo方法设置上传结果        
    }
    else {
        alert("文件上传出错！");
    }
}
*/
document.write('<script src="/Scripts/lib/uploadify/jquery.uploadify.min.js" type="text/javascript"></script>');
document.write('<link href="/Scripts/lib/uploadify/uploadify.css" rel="stylesheet" type="text/css" />');

function uploadFile(btn, folder, allownum, type, success, auto, progressfn, btnclass, text, width, height) {

            var fileExt,fileDesc;
            switch (type) {
                case "flash":
                    fileExt = '*.swf;*.SWF;';
                    fileDesc = '*.swf;*.SWF;';
                break;
                case "doc":
                    fileExt += '*.doc;*.pdf;';
                    fileDesc +='*.doc;*.pdf;';
                break;
                case "excel":
                    fileExt += '*.xls;';
                    fileDesc += '*.xls;';
                break;
                case "audio":
                    fileExt = '*.mp3;';
                    fileDesc = '*.mp3;';
                break;
                case "image":
                    fileExt = "*.jpg;*.gif;*.bmp;*.png;";
                    fileDesc = "*.jpg;*.gif;*.bmp;*.png;";
                break;
                default:
                    fileExt = "*.jpg;*.gif;*.bmp;*.png;";
                    fileDesc = "*.jpg;*.gif;*.bmp;*.png;";
                break;
        }

        (typeof (btn) == "string" ? $("#" + btn) : btn).uploadify({
            'height': height||20,
            'width': width||80,
            'buttonClass': btnclass,
            'auto': auto,
            'swf': '/Scripts/lib/uploadify/uploadify.swf?var=' + (new Date()).getTime(),
            'uploader': '/File/Uploads/',
            'buttonText': text||'浏览',
            'formData': { 'folder': folder },
            'cancelImg': '/Scripts/cer/uploadify/uploadify-cancel.png',
            'fileTypeExts': fileExt,
            'fileTypeDesc': fileDesc,
            'fileSizeLimit': 1024 * 1024 * 4, //4M   
            'multi': typeof (allownum) == "number" && allownum > 1,
            'queueSizeLimit': allownum || 1,
            'onUploadSuccess': function (file, data, response) {
               window["responseData"]((new Function("return " + data))(), success);
            },
            'wmode': 'transparent',
            "overrideEvents": ["onSelect"],
            "onUploadProgress": function (file, bytesUploaded, bytesTotal, totalBytesUploaded, totalBytesTotal) {
                if (typeof (progressfn) == 'function')
                    progressfn(file, bytesUploaded, bytesTotal);
            }
        });
}