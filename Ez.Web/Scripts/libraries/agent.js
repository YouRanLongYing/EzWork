(function () {

    var correctPos = function (returnJson) {
        var label = returnJson.jqWrapper.find("label[for='" + returnJson.jqEle.attr("id") + "']");
        if (label.length > 0 && !label.attr("topsetted")) {
            var lalheight = label.height();
            var pertop = returnJson.jqIco.position().top;
            var perleft = returnJson.jqIco.position().left;
            if (!isNaN(lalheight) && !isNaN(pertop)) {
                returnJson.jqIco.css({ "top": pertop + lalheight - 6 }); //, "left": perleft 
            }
            label.attr("topsetted", "true");
        }
    }
    var getItemValidaty = function (valElement) {
        var returnJson = {};
        returnJson["jqEle"] = $(valElement);
        returnJson["jqWrapper"] = returnJson.jqEle.parent();
        returnJson["jqIco"] = returnJson.jqWrapper.find("span[am-val='ico']");
        if (returnJson.jqIco.length == 0) {
            returnJson.jqIco = $("<span am-val=\"ico\"></span>");
            returnJson.jqIco.insertAfter(returnJson.jqEle);
        }
        returnJson["jqPopObj"] = returnJson.jqEle.is("textarea") ? returnJson.jqEle.prev() : returnJson.jqEle;
        return returnJson;
    }
    var valErrorShow = function (arrayobj) {
        if (arrayobj && arrayobj.length > 0) {
            for (var i in arrayobj) {

                var itemVal = getItemValidaty(arrayobj[i].element)
                if (!itemVal.jqEle.hasClass("am-form-field")) {
                    itemVal.jqEle.addClass("am-form-field");
                }

                if (itemVal.jqEle.attr("novalico") != 'yes') {
                    itemVal.jqWrapper.removeClass("am-form-success am-form-icon am-form-feedback").addClass("am-form-warning am-form-icon am-form-feedback");
                    itemVal.jqIco.removeClass("am-icon-check").addClass("am-icon-warning").attr("title", arrayobj[i].message);
                    correctPos(itemVal);
                }
                itemVal.jqPopObj.popover({ content: arrayobj[i].message, trigger: "hover focus" });
                if (itemVal.jqPopObj.data('amui.popover')) {
                    var poptip = itemVal.jqPopObj.data('amui.popover')["$popover"]
                    if (poptip && poptip.length > 0) {
                        poptip.find(".am-popover-inner").html(arrayobj[i].message);
                        poptip.data("am-pop-tipExt-enable", true);
                    }
                }
            }
        }
    }
    var valSuccessShow = function (successList) {

        if (successList && successList.length > 0) {
            for (var i in successList) {
                var itemVal = getItemValidaty(successList[i])
                if (!itemVal.jqEle.hasClass("am-form-field")) {
                    itemVal.jqEle.addClass("am-form-field");
                }
                if (itemVal.jqEle.attr("novalico") != 'yes') {
                    itemVal.jqWrapper.removeClass("am-form-warning am-form-icon am-form-feedback").addClass("am-form-success am-form-icon am-form-feedback");
                    itemVal.jqIco.removeClass("am-icon-warning").addClass("am-icon-check").removeAttr("title");
                    correctPos(itemVal);
                }
                if (itemVal.jqPopObj.data('amui.popover')) {
                    var poptip = itemVal.jqPopObj.data('amui.popover')["$popover"]
                    if (poptip && poptip.length > 0) {
                        poptip.data("am-pop-tipExt-enable", false);
                    }
                }
            }
        }
    }
    window["validatyErrorShow"] = function (form, array, arrayobj) {
        valErrorShow(arrayobj);
        valSuccessShow(this.successList);
    }

    //关于编辑器的绑定
    window["aboutEditor"] = function (textareas) {
        if (textareas.length == 0) return;
        var pluginarr = {
            tables: "/Scripts/plugins/froala_editor/plugins/tables.min.js",
            lists: "/Scripts/plugins/froala_editor/plugins/lists.min.js",
            colors: "/Scripts/plugins/froala_editor/plugins/colors.min.js",
            media_manager: "/Scripts/plugins/froala_editor/plugins/media_manager.min.js",
            font_size: "/Scripts/plugins/froala_editor/plugins/font_size.min.js",
            block_styles: "/Scripts/plugins/froala_editor/plugins/block_styles.min.js",
            block_styles: "/Scripts/plugins/froala_editor/plugins/block_styles.min.js",
            video: "/Scripts/plugins/froala_editor/plugins/video.min.js"
        };
        var config = {
            inlineMode: false, alwaysBlank: true,
            language: "zh_cn",
            imageUploadURL: '/File/Uploads', //上传到本地服务器
            imageUploadParams: { from: "editor", folder: "/files/editor" },
            imageDeleteURL: '/File/Uploads?action=del', //删除图片
            imagesLoadURL: '/File/Uploads?action=load', //管理图片.
            maxHeight: 500,
            minHeight: 200
        };
        var cssarr = ["/Content/plugins/froala_editor/froala_editor.min.css"];
        for (var i = 0; i < cssarr.length; i++) {
            $("body").append(' <link href="' + cssarr[i] + '" rel="stylesheet" type="text/css">');
        }
        $("body").append('<script src="/Scripts/plugins/froala_editor/froala_editor.min.js"></script>\r<!--[if lt IE 9]>\r<script src="/Content/plugins/froala_editor/froala_editor_ie8.min.js"></script>\r<![endif]-->');

        setTimeout(function () {
            textareas.each(function () {
                //config["placeholder"] = $(this).attr("placeholder");
                $(this).editable(config)
              .on('editable.afterRemoveImage', function (e, editor, $img) {
                  // Set the image source to the image delete params.        
                  editor.options.imageDeleteParams = { src: $img.attr('src') };
                  // Make the delete request
                  editor.deleteImage($img);
              });
            });
        }, 100);
    }

    //上传插件的代理脚本
    window["aboutUpload"] = function (upbtns) {
        if (upbtns.length == 0) return;
        $("body").append('<script src="/Scripts/libraries/Upload.js" type="text/javascript"></script>');
        setTimeout(function () {
            //关于上传
            var uploadFn = window["uploadFile"];
            var viewUpload = function (type, src, target) {
                var viewObj;
                var id = target.attr("id") + "_view";
                switch (type) {
                    case "image":
                        {
                            if (!document.getElementById(id)) {
                                viewObj = $("<img id='" + id + "' style='max-width:200px;'/>");
                                viewObj.insertAfter(target);
                            }
                            else {
                                viewObj = $("#" + id);
                            }
                            viewObj.attr("src", src);
                        }; break;

                }
            }
            upbtns.each(function (i) {
                var uptag = $(this);
                var btn_id = "jquery_file_up_" + i;
                uptag.attr("id", btn_id);
                var dir = uptag.data("dir") || "";
                var auto = uptag.data("auto");
                var success = uptag.data("success");
                var progress = uptag.data("progress");
                var uptype = uptag.data("uptype") || "image";
                var allownum = uptag.data("allownum") || 1;
                var width = uptag.attr("width") || 85;
                var height = uptag.attr("height") || 35;
                var text = uptag.text() || "browser";
                var classname = uptag.attr("class");
                var successcall = window[success] || function (data) {

                    try {
                        var btn = $("#" + btn_id);
                        btn.parent().find("[fileup = 'yes']").val(data[0].sourcesrc);
                        viewUpload(uptype, data[0].sourcesrc, btn);
                    }
                    catch (e) {

                    }
                };
                var progresscall = window[progress] || function (file, bytesUploaded, bytesTotal) {
                    var btntxt = $("#" + btn_id + "-button span:eq(0)");
                    var percent = bytesUploaded * 1 / bytesTotal;
                    btntxt.text(percent * 100 + "%");
                    if (percent <= 1) {
                        setTimeout(function () {
                            btntxt.text(text);
                        }, 1000);
                    }
                };
                uploadFn(uptag, dir, allownum, uptype, successcall, auto, progresscall, classname, text, width, height);
            });
        }, 100);
    }
    //关于页面加载动画的绑定
    window["aboutPageLoad"] = function () {
        $("body a").on("click", function () {
            var _desk = top.desk || desk;
            if (!_desk || !_desk.pageloading) return;
            _desk.pageloading.hide();
            var href = $(this).attr("href");
            href = href == "javascript:void(0)" ? $(this).data("src") : href;
            if ((href && /^http:\/\/|https:\/\//g.test(href)) || !href) return;
            if (_desk && typeof (_desk.pageLoadStart)) _desk.pageLoadStart();
        });
    }
    //关于页面列表的加载
    window["aboutIGrid"] = function () {
        var lists = $("table[ezgrid]");
        if (lists.length > 0) {
            lists.each(function (i) {
                $(this).renderEzGrid();
            })
        }
    }
    //关于联动
    window["ddlefect"] = function () {
        $("select[changefor]").on("change", function () {
            var $this = $(this);
            var targetid = $this.attr("changefor");
            var selectvalue = $this.val();
            if (!selectvalue) return;
            var url = $this.attr("from");
            var param = ($this.attr("callm") || function () {
                return {};
            })();
            param[$this.attr("id")] = selectvalue;
            var targetopts = $("#" + targetid);
            targetopts.empty();
            var opts = [];
            UBIQ.query(url, param, function (data) {

                $.each(data, function (key, value) {
                    opts.push("<option value=" + key + ">" + value + "</option>");
                })
                targetopts.html(opts.join("\r\n"));
                if (opts.length == 0) {
                    targetopts.html("<option value='0'>无数据</option>");
                }

                $("#" + targetid).trigger("change");

            })
        })
    }
})()