
function Ajax(url) {
    return $.ajax({
        url: url,
        // data: dataParametes,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            obj = response.d;
        },
        error: function (data) {
            return encodeURIComponent(data.responseText);
        }
    });

}

function cargarLista(idLstSelect, arr, nombreColumnaToShow, seleccionado, valor) {
    arr2 = parseJSON(arr);
    var h = [];
    $('#' + idLstSelect).empty().append('');
    // if (!seleccionado) {
    //h.push('<option value="0">Seleccione...</option>');
    //  }
    $.each(arr2, function (i, val) {
        if (nombreColumnaToShow == undefined)
            h.push('<option value="' + val.Id + '">' + val.NombreLargo + '</option>');    
        else
            h.push('<option value="' + val.Id + '">' + val[nombreColumnaToShow] + '</option>');    
    });
    $("#" + idLstSelect).empty().append(h.join(''));

    if (seleccionado) {
        $("#" + idLstSelect).val(valor);

    } else {
        $("#" + idLstSelect).val("0");
    }
}

function setCookie(cname, cvalue, exmins) {
    var d = new Date();
    d.setTime(d.getTime() + (exmins * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function parseJSON(data) {
    return window.JSON && window.JSON.parse ? window.JSON.parse(decodeURIComponent(data)) : (new Function("return " + decodeURIComponent(data)))();
}

function validarEmail(email) {
    expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!expr.test(email))
        return false;
    return true;
}

function replaceAll(str, find, replace )
{
    while (str.indexOf(find) != -1) {
        str = str.replace(find, replace);
    }
    return str;
}

(function ($) {
    $.QueryString = (function (a) {
        if (a == "") return {};
        var b = {};
        for (var i = 0; i < a.length; ++i) {
            var p = a[i].split('=', 2);
            if (p.length != 2) continue;
            b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
        }
        return b;
    })(window.location.search.substr(1).split('&'))
})(jQuery);
