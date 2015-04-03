//$.validator.settings.showErrors = function (a, b) {


//    var f = 0;
//}

/*验证邮箱*/
$.validator.addMethod("startswith", function (value, element, param) {
    if (value == false) {
        return true;
    }
    if (value.indexOf(param) == -1) {
        return false;
    }
    else {
        return true;
    }
});
$.validator.unobtrusive.adapters.addSingleVal("startswith", "inputstring");

/*验证IP*/
$.validator.addMethod("ip", function (value, element, param) {
    if (value == false) {
        return true;
    };
    if (!/^((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))$/g.test(value)) {
        return false;
    }
    else {
        return true;
    }
});
$.validator.unobtrusive.adapters.add("ip", function (options) {
    options.rules["ip"] = {
        value: options.params
    };
    options.messages["ip"] = options.message;
});

/*验证Url*/
$.validator.addMethod("url", function (value, element, param) {
    if (value == false) {
        return true;
    };
    if (!/^http(s):\/\/([\w-]+\.)+[\w-]+(\/[\w- .\/?%&=]*)?$/g.test(value)) {
        return false;
    }
    else {
        return true;
    }
});
$.validator.unobtrusive.adapters.add("url", function (options) {
    options.rules["url"] = {
        value: options.params
    };
    options.messages["url"] = options.message;
});

/*验证datatype*/
$.validator.addMethod("cktype", function (value, element, param) {
    if (value == false) {
        return true;
    };

    switch(param.valtype.toLowerCase())
    {
       case "int":return /^[1-9]+[0-9]*$/g.test(value);
       case "number":return /^^[1-9]+[0-9]*[.]?[0-9]*$/g.test(value);
       case "datetime":
         try{
              return (new RegExp(param.regex)).test(value)
          }
          catch(e)
          {
           return false;
          }
      default: return false;
    }
});
$.validator.unobtrusive.adapters.add("cktype", ["valtype", "regex"], function (options) {
     options.rules["cktype"] = {
       valtype:options.params.valtype,
         regex:options.params.regex
    };
    options.messages["cktype"] = options.message;
});




/*验证身份证*/
$.validator.addMethod("idcard", function (value, element, param) {
    return /^(11|12|13|14|15|21|22|23|31|32|33|34|35|36|37|41|42|43|44|45|46|50|51|52|53|54|61|62|63|64|65|71|81|82|91)(\d{13}|\d{15}[\dx])$/g.test(value);
});
$.validator.unobtrusive.adapters.add("idcard", function (options) {
    options.rules["idcard"] = {};
    options.messages["idcard"] = options.message;
});

/*验证座机号*/
$.validator.addMethod("phone", function (value, element, param) {
    return /^([0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8})$/g.test(value);
});
$.validator.unobtrusive.adapters.add("phone", function (options) {
    options.rules["phone"] = {};
    options.messages["phone"] = options.message;
});
/*验证手机号*/
$.validator.addMethod("mobile", function (value, element, param) {
    return /(^1[3,5,8][0-9]{1}[0-9]{8}$)/g.test(value);
});
$.validator.unobtrusive.adapters.add("mobile", function (options) {
    options.rules["mobile"] = {};
    options.messages["mobile"] = options.message;
});

/*验证邮编*/
$.validator.addMethod("postcode", function (value, element, param) {
    return /(^\d{6}$)/g.test(value);
});
$.validator.unobtrusive.adapters.add("postcode", function (options) {
    options.rules["postcode"] = {};
    options.messages["postcode"] = options.message;
});

/*验证range*/
$.validator.addMethod("ranges", function (value, element, param) {
    if (/\d*([.])?\d*/g.test(value)) {
        value = parseFloat(value);
        param.min = parseFloat(param.min);
        param.max = parseFloat(param.max);

        if (param.eqmin && param.eqmax) {
            return value >= param.min && value <= param.max;
        }
        else if (param.eqmin && !param.eqmax) {
            return value >= param.min && value < param.max;
        }
        else if (!param.eqmin && param.eqmax) {
            return value > param.min && value <= param.max;
        }
        else {
            return value > param.min && value < param.max;
        }
    }
    return false;
});
$.validator.unobtrusive.adapters.add("ranges", ["min", "max", "eqmin", "eqmax"], function (options) {
    options.rules["ranges"] = {
        min:options.params.min,
        max:options.params.max,
        eqmin: options.params.eqmin.toLowerCase()=="true",
        eqmax: options.params.eqmax.toLowerCase()=="true"
    };
    options.messages["ranges"] = options.message;
});

/*验证lessthan*/
$.validator.addMethod("lessthan", function (value, element, param) {
    if (/\d*([.])?\d*/g.test(value)) {
        value = parseFloat(value);
        param.max = parseFloat(param.max);
        if (param.eqmax) {
            return value <= param.max;
        }
        else {
            return value < param.max;
        }
    }
    return false;
});
$.validator.unobtrusive.adapters.add("lessthan", ["max", "eqmax"], function (options) {
    options.rules["lessthan"] = {
        max: options.params.max,
        eqmax: options.params.eqmax.toLowerCase() == "true"
    };
    options.messages["lessthan"] = options.message;
});

/*验证morethan*/
$.validator.addMethod("greaterthan", function (value, element, param) {
    if (/\d*([.])?\d*/g.test(value)) {
        value = parseFloat(value);
        param.min = parseFloat(param.min);
        if (param.eqmin) {
            return value >= param.min;
        }
        else {
            return value < param.min;
        }
    }
    return false;
});
$.validator.unobtrusive.adapters.add("greaterthan", ["min", "eqmin"], function (options) {
    options.rules["greaterthan"] = {
        min: options.params.min,
        eqmin: options.params.eqmin.toLowerCase() == "true"
    };
    options.messages["greaterthan"] = options.message;
});


/*验证rangelen*/
$.validator.addMethod("rangelenth", function (value, element, param) {
        var len = value.length;
        param.min = parseInt(param.min);
        param.max = parseInt(param.max);
        if (param.eqmin && param.eqmax) {
            return len >= param.min && len <= param.max;
        }
        else if (param.eqmin && !param.eqmax) {
            return len >= param.min && len < param.max;
        }
        else if (!param.eqmin && param.eqmax) {
            return len > param.min && len <= param.max;
        }
        else {
            return len > param.min && len < param.max;
        }
});
$.validator.unobtrusive.adapters.add("rangelenth", ["min", "max", "eqmin", "eqmax"], function (options) {
    options.rules["rangelenth"] = {
        min: options.params.min,
        max: options.params.max,
        eqmin: options.params.eqmin.toLowerCase() == "true",
        eqmax: options.params.eqmax.toLowerCase() == "true"
    };
    options.messages["rangelenth"] = options.message;
});

/*验证lessthanlen*/
$.validator.addMethod("lessthanlen", function (value, element, param)
{ 
        var len = value.length;
        param.max = parseInt(param.max);
        if (param.eqmax) {
            return len <= param.max;
        }
        else {
            return len < param.max;
        }
});
$.validator.unobtrusive.adapters.add("lessthanlen", ["max", "eqmax"], function (options) {
    options.rules["lessthanlen"] = {
        max: options.params.max,
        eqmax: options.params.eqmax.toLowerCase() == "true"
    };
    options.messages["lessthanlen"] = options.message;
});

/*验证geaterthanlen*/
$.validator.addMethod("greaterthanlen", function (value, element, param) {
        var len = value.length;
        param.min = parseInt(param.min);
        if (param.eqmin) {
            return len >= param.min;
        }
        else {
            return len > param.min;
        }
});
$.validator.unobtrusive.adapters.add("greaterthanlen", ["min", "eqmin"], function (options) {
    options.rules["greaterthanlen"] = {
        min: options.params.min,
        eqmin: options.params.eqmin.toLowerCase() == "true"
    };
    options.messages["greaterthanlen"] = options.message;
});