$(document).ready(function () {
    var selectedTab = localStorage.getItem("selectedTab");
    if (selectedTab == 1) {
        $("#btnVehicleSearchTab").click();

    } else if (selectedTab == 2) {
        $("#btnSparePartSearchTab").click();
    }

    //$("#navbutton").click(function (event) {
    //        $('#navbar').toggleClass("collapse");
    //});
    //$("#inpGrp").click(function (event) {
    //    $('#navbar').toggleClass("collapse");
    //});
    
});



//  ** SideBar
$("#fader1").mousemove(function (e) {
    $("#volume").html($(this).val());
});
$("#fader2").mousemove(function (e) {
    $("#volume2").html($(this).val());
});
$("#fader3").mousemove(function (e) {
    $("#volume3").html($(this).val());
});
$("#fader4").mousemove(function (e) {
    $("#volume4").html($(this).val());
});

//***************

$(".side").sidecontent({
    classmodifier: "sidecontent",
    attachto: "leftside",
    width: "300px",
    pulloutpadding: "5",
    textdirection: "vertical"
});

$("#searchTextIdService").focus(function () {
    if (this.value == this.defaultValue)
        this.value = '';
    $(this).removeClass("searchGray");
    $(this).addClass("searchBlack");
});

$("#searchTextIdService").blur(function () {
    if (this.value == '')
        this.value = this.defaultValue;
    $('#hdnInpSearch').val(this.value);
    if (this.value == "Hizmet adı ya da açıklamasında") {
        $(this).removeClass("searchBlack");
        $(this).addClass("searchGray");
    } else { }
});

function loadGeneralSearch() {
    $.ajax({
        //beforeSend: function () { $.mobile.showPageLoadingMsg(); }, //Show spinner
        //complete: function () { $.mobile.hidePageLoadingMsg() }, //Hide spinner
        url: '/otomobilvasita/_SearchProductVhicle/?criteria=' + searchTextIdSparepartGeneral.value,
        success: function (result) {
            $('#vhiclePage').css("display", "block");
            $('#searchProd').html(result);

            $('#profileVehicleDiv').addClass("androidFix").scrollTop(0).removeClass("androidFix");

            UpdateWebGrid();

            $('html, body').stop().animate({
                scrollTop: $(window).scrollTop() + 700
            }, 500, 'swing');

        }
    });
}

$("#araSparepart").click(function () {
    $("#searchType").val("2");
    $("#catForm").submit();
});

$("#araGeneral").click(function () {
    //sessionStorage.setItem("gsearch", searchTextIdSparepartGeneral.value);
    window.location = 'http://www.otomotivist.com/otomobilvasita/yeniikinciel?gsearch=' + searchTextIdSparepartGeneral.value;
});

$("#searchTextIdSparepartGeneral").keypress(function () {
    if (event.which == 13) {
        //sessionStorage.setItem("gsearch", searchTextIdSparepartGeneral.value);
        window.location = 'http://www.otomotivist.com/otomobilvasita/yeniikinciel?gsearch=' + searchTextIdSparepartGeneral.value;
        //window.location = 'http://localhost:51226/otomobilvasita/yeniikinciel?gsearch=' + searchTextIdSparepartGeneral.value;
    }
});

$("#navsearch").keyup(function (event) {
    if (event.which == 13) {
        event.preventDefault();
        if (document.getElementById("searchTextIdSparepart").value == '')
            document.getElementById("searchTextIdSparepart").value = document.getElementById("searchTextIdSparepart").defaultValue;
        $('#hdnInpSearch').val(document.getElementById("searchTextIdSparepart").value);
        if (localStorage.getItem("selectedTab") == 1) {
            document.getElementById("searchType").value = "1";
        } else if (localStorage.getItem("selectedTab") == 2) {
            document.getElementById("searchType").value = "2";
        } else if (localStorage.getItem("selectedTab") == 3) {
            document.getElementById("searchType").value = "3";
        }
        if (document.getElementById("catForm") != null) {
            $("#catForm").submit();
        } else {
            $("#navsearch").submit();
        }
    }
});

$("#btnVehicleSearchTab").click(function () {
    $("#btnSparePartSearchTab").removeClass("selectedTab");
    $("#btnServiceSearchTab").removeClass("selectedTab");
    $(this).addClass("selectedTab");
    localStorage.setItem("selectedTab", 1);
    $("#sTspareParts").hide();
    $("#sTsevices").hide();
    $("#sTvehicles").show();
    $('#wfcs').show();
    $('#wfcsSparepart').hide();

    var elem2 = $('#btnSparePartSearchTab');
    elem2.removeClass("SelectedPart");

    var elem = $('#btnVehicleSearchTab');
    elem.removeClass("tabbtnsinput1");
    elem2.addClass("tabbtnbtn1");
    elem.addClass("SelectedWhicle");

    document.getElementById("sidecontent_0_pullout").style.visibility = "hidden";
    document.getElementById("lblBasketInfo").style.visibility = "hidden";
});

$("#btnSparePartSearchTab").click(function () {
    $("#btnVehicleSearchTab").removeClass("selectedTab");
    $("#btnServiceSearchTab").removeClass("selectedTab");
    $(this).addClass("selectedTab");
    localStorage.setItem("selectedTab", 2);
    $("#sTspareParts").show();
    $("#sTsevices").hide();
    $("#sTvehicles").hide();
    $('#wfcs').hide();
    $('#wfcsSparepart').show();

    var elem2 = $('#btnVehicleSearchTab');
    elem2.removeClass("SelectedWhicle");
    

    var elem = $('#btnSparePartSearchTab');
    elem.removeClass("tabbtnbtn1");
    elem2.addClass("tabbtnsinput1");
    elem.addClass("SelectedPart");

    document.getElementById("sidecontent_0_pullout").style.visibility = "visible";
    document.getElementById("lblBasketInfo").style.visibility = "visible";
});

$("#btnServiceSearchTab").click(function () {
    $("#btnVehicleSearchTab").removeClass("selectedTab");
    $("#btnSparePartSearchTab").removeClass("selectedTab");
    $(this).addClass("selectedTab");
    localStorage.setItem("selectedTab", 3);
    $("#sTspareParts").hide();
    $("#sTsevices").show();
    $("#sTvehicles").hide();
});

$("#araVehicleY").click(function () {
    var categoryContainer = document.getElementById("otomobilvasitaCat1Y");
    var category = categoryContainer.options[categoryContainer.selectedIndex].value;
    var markContainer = document.getElementById("otomobilvasitaCat2Y");
    var mark = markContainer.options[markContainer.selectedIndex].value;
    var textCriteria = document.getElementById("searchTextIdVehicleY").value

    $.ajax({
        url: '/otomobilvasita/_SearchProductSpareparts/?categoryId=' + category + '&markId=' + mark + '&criteria=' + textCriteria,
        success: function (result) {

            UpdateWebGrid();

            $('#sparepartsPage').css("display", "block");
            $('#searchProdSpareparts').html(result);
        }
    });
});

$("#araVehicle").click(function () {
    //document.getElementById("searchType").value = "1";
    //$("#catForm").submit();
    var categoryContainer = document.getElementById("otomobilvasitaCat1");
    var category = categoryContainer.options[categoryContainer.selectedIndex].value;
    var markContainer = document.getElementById("otomobilvasitaCat2");
    var mark = markContainer.options[markContainer.selectedIndex].value;
    var modelContainer = document.getElementById("otomobilvasitaCat3");
    var model = modelContainer.options[modelContainer.selectedIndex].value;
    var subModelContainer = document.getElementById("otomobilvasitaCat6");
    var subModel = subModelContainer.options[subModelContainer.selectedIndex].value;
    var fueltypeContainer = document.getElementById("product.FuelType");
    var fueltype = fueltypeContainer.options[fueltypeContainer.selectedIndex].value;
    var casetypeContainer = document.getElementById("product.CaseType");
    var casetype = casetypeContainer.options[casetypeContainer.selectedIndex].value;
    var geartypeContainer = document.getElementById("product.GearType");
    var geartype = geartypeContainer.options[geartypeContainer.selectedIndex].value;
    var modelYearStart = document.getElementById("volume").value;
    var modelYearEnd = document.getElementById("volume2").value
    var cycileStart = document.getElementById("volume3").value;
    var cycileEnd = document.getElementById("volume4").value
    var priceStart = document.getElementById("startPrice").value;
    var priceEnd = document.getElementById("endPrice").value
    var textCriteria = document.getElementById("searchTextIdVehicle").value

    var cityContainer = document.getElementById("product.City");
    var loka = cityContainer.options[cityContainer.selectedIndex].value;


    $.ajax({
        url: '/otomobilvasita/_SearchProductVhicle/?categoryId=' + category + '&markId=' + mark + '&model=' + model + '&altModeId=' + subModel + '&kasaTipi=' + casetype + '&vitesTipi=' + geartype + '&lokasyon='+ loka + '&yakitTipi=' + fueltype + '&modelYearStart=' + modelYearStart + '&modelYearEnd=' + modelYearEnd + '&silindirStart=' + cycileStart + '&silindirEnd=' + cycileEnd + '&priceStart=' + priceStart + '&priceEnd=' + priceEnd + '&criteria=' + textCriteria,
        success: function (result) {
            $('#vhiclePage').css("display", "block");
            $('#searchProd').html(result);

            UpdateWebGrid();
        }
    });
});
$("#araSparepart").click(function () {
    document.getElementById("searchType").value = "2";
    $("#catFormSparepart").submit();
});


//$(function () {
//    $('#recivemessage').dialog({
//        autoOpen: false,
//        width: 400,
//        resizable: false,
//        title: 'Mesaj Gönder',
//        modal: true
//    });

//    $('.modal').click(function () {
//        $('#recivemessage').load(this.href, function () {
//            $(this).dialog('open');
//        });
//        return false;
//    });
//});

function SetSender(th) {
    var titles = th.title.split('|');
    document.getElementsByName('accountId')[0].value = titles[0];
    document.getElementsByName('advertisementId')[0].value = -1;
    document.getElementsByName('txtSubject')[0].value = 'Re ' + titles[1];
    //titles[2]
}


// otomotivistindex.js ***********************

//$(document).ready(function () {
//    $('#otomobilvasitaCat4').val('');
//    resizeDiv();
//    $(".bxslider").bxSlider({
//        auto: true,
//        autoControls: true
//    });
//});

function UpdateWebGrid() {
    $('td:nth-child(2),th:nth-child(2)').addClass("hidden-xs");
    //$('td:nth-child(4),th:nth-child(4)').addClass("hidden-xs");
    $('td:nth-child(5),th:nth-child(5)').addClass("hidden-xs");
    $('td:nth-child(6),th:nth-child(6)').addClass("visible-lg");
    $('td:nth-child(7),th:nth-child(7)').addClass("hidden-xs");
    $('td:nth-child(8),th:nth-child(8)').addClass("hidden-xs");

    var tr = $('#profileVehicleGrid').find('tr');
    tr.bind('click', function (event) {
        location.href = '/otomobilvasita/aracozellikleri/?prdcId=' + $(this).find('#productId').val() + '&productGroup=' + $(this).find('#productGroup').val() + '&productCat=' + $(this).find('#productCat').val() + '&productCurr=' + $(this).find('#productCurr').val();
    })

    ChangeLink();
};

window.onresize = function (event) {
    resizeDiv();
};

function resizeDiv() {
    vpw = $(window).width();
    vph = $(window).height();

};

$(function () {
    $('#otomobilvasitaCat1').click(function () {
        $.getJSON('/otomobilvasita/CategoryList/' + $('#otomobilvasitaCat1').val(), function (data) {
            var items = '<option value=\'null\'>Seçim yapınız.</option>';
            $.each(data, function (i, chosen) {
                items += "<option value='" + chosen.Value + "'>" + chosen.Text + "</option>";
            });
            if (items != '') {
                $('#otomobilvasitaCat2').html(items);
                localStorage.setItem("otomobilvasitacat2items", items);
            }
        });
        localStorage.setItem("otomobilvasitacat1selectedindex", document.getElementById('otomobilvasitaCat1').selectedIndex);
        document.getElementById('otomobilvasitaCat3').selectedIndex = 0;
        document.getElementById('otomobilvasitaCat6').selectedIndex = 0;

    });
});



$(function () {
    $('#otomobilvasitaCat1').change(function () {
        localStorage.setItem("otomobilvasitacat1selectedindex", document.getElementById('otomobilvasitaCat1').selectedIndex);
        document.getElementById('otomobilvasitaCat3').selectedIndex = 0;
        document.getElementById('otomobilvasitaCat6').selectedIndex = 0;

        var cat = document.getElementById("otomobilvasitaCat1");
        var index = cat.options[cat.selectedIndex].value;

        $.ajax({
            url: '/otomobilvasita/_SearchProductVhicle/?categoryId=' + index,
            success: function (result) {
                $('#vhiclePage').css("display", "block");
                $('#searchProd').html(result);

                UpdateWebGrid();
            }
        });

    });
});

function ChangeLink() {
    if ($('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=1').length > 0)
        $('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=1"]').attr('href', 'javascript:callPager(1);');
    if ($('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=2').length > 0)
        $('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=2"]').attr('href', 'javascript:callPager(2);');
    if ($('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=3').length > 0)
        $('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=3"]').attr('href', 'javascript:callPager(3);');
    if ($('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=4').length > 0)
        $('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=4"]').attr('href', 'javascript:callPager(4);');
    if ($('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=5').length > 0)
        $('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=5"]').attr('href', 'javascript:callPager(5);');
    if ($('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=6').length > 0)
        $('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=6"]').attr('href', 'javascript:callPager(6);');
    if ($('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=7').length > 0)
        $('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=7"]').attr('href', 'javascript:callPager(7);');
    if ($('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=8').length > 0)
        $('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=8"]').attr('href', 'javascript:callPager(8);');
    if ($('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=9').length > 0)
        $('a[href*="/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=9"]').attr('href', 'javascript:callPager(9);');
}

function callPager(e) {
    $.ajax({
        url: '/otomobilvasita/_SearchProductVhicle/?categoryId=1&page=' + e,
        type: 'GET',
        cache: false,
        //data: { value: value },
        success: function (result) {
            $('#vhiclePage').html(result);
            ChangeLink();
        }
    });
}

//$.get('@Url.Action("_SearchProductVhicle","otomobilvasita", new { categoryId=, markId, modelId, altModeId, kasaTipi, vitesTipi, yakitTipi, model, silindir, fiyat } )', function (data) {

$(function () {
    $('#otomobilvasitaCat2').click(function () {
        $.getJSON('/otomobilvasita/CategoryList/' + $('#otomobilvasitaCat2').val(), function (data) {
            var items = '<option value=\'null\'>Seçim yapınız.</option>';
            $.each(data, function (i, chosen) {
                items += "<option value='" + chosen.Value + "'>" + chosen.Text + "</option>";
            });
            if (items != '') {
                $('#otomobilvasitaCat3').html(items);
                localStorage.setItem("otomobilvasitacat3items", items);
            }
        });
        localStorage.setItem("otomobilvasitacat2selectedindex", document.getElementById('otomobilvasitaCat2').selectedIndex);
    });
});
$(function () {
    $('#otomobilvasitaCat2').change(function () {
        localStorage.setItem("otomobilvasitacat2selectedindex", document.getElementById('otomobilvasitaCat2').selectedIndex);
        document.getElementById('otomobilvasitaCat6').selectedIndex = 0;

        var cat = document.getElementById("otomobilvasitaCat1");
        var index1 = cat.options[cat.selectedIndex].value;
        var cat2 = document.getElementById("otomobilvasitaCat2");
        var index2 = cat2.options[cat2.selectedIndex].value;

        $.ajax({
            url: '/otomobilvasita/_SearchProductVhicle/?categoryId=' + index1 + '&markId=' + index2,
            success: function (result) {
                $('#vhiclePage').css("display", "block");
                $('#searchProd').html(result);

                UpdateWebGrid();
            }
        });
    });
});

$(function () {
    $('#otomobilvasitaCat3').click(function () {
        $.getJSON('/otomobilvasita/CategoryList/' + $('#otomobilvasitaCat3').val(), function (data) {
            var items = '<option value=\'null\'>Seçim yapınız.</option>';
            $.each(data, function (i, chosen) {
                items += "<option value='" + chosen.Value + "'>" + chosen.Text + "</option>";
            });
            if (items != '') {
                $('#otomobilvasitaCat6').html(items);
                localStorage.setItem("otomobilvasitacat6items", items);
            }
        });
        localStorage.setItem("otomobilvasitacat3selectedindex", document.getElementById('otomobilvasitaCat3').selectedIndex);
    });
});
$(function () {
    $('#otomobilvasitaCat3').change(function () {
        $.getJSON('/otomobilvasita/CategoryList/' + $('#otomobilvasitaCat3').val(), function (data) {
            var items = '<option value=\'null\'>Seçim yapınız.</option>';
            $.each(data, function (i, chosen) {
                items += "<option value='" + chosen.Value + "'>" + chosen.Text + "</option>";
            });
            if (items != '') {
                $('#otomobilvasitaCat6').html(items);
                localStorage.setItem("otomobilvasitacat6items", items);
            }
        });
        localStorage.setItem("otomobilvasitacat3selectedindex", document.getElementById('otomobilvasitaCat3').selectedIndex);
    });
});



$(function () {
    $('#otomobilvasitaCat1Y').click(function () {
        $.getJSON('/otomobilvasita/CategoryListSpareParts/' + $('#otomobilvasitaCat1Y').val(), function (data) {
            var items = '<option value=\'null\'>Seçim yapınız.</option>';
            $.each(data, function (i, chosen) {
                items += "<option value='" + chosen.Value + "'>" + chosen.Text + "</option>";
            });
            if (items != '') {
                $('#otomobilvasitaCat2Y').html(items);
                localStorage.setItem("otomobilvasitacat2Yitems", items);
            }
        });
        localStorage.setItem("otomobilvasitacat1Yselectedindex", document.getElementById('otomobilvasitaCat1Y').selectedIndex);
    });
});
$(function () {
    $('#otomobilvasitaCat1Y').change(function () {
        $.getJSON('/otomobilvasita/CategoryListSpareParts/' + $('#otomobilvasitaCat1Y').val(), function (data) {
            var items = '<option value=\'null\'>Seçim yapınız.</option>';
            $.each(data, function (i, chosen) {
                items += "<option value='" + chosen.Value + "'>" + chosen.Text + "</option>";
            });
            if (items != '') {
                $('#otomobilvasitaCat2Y').html(items);
                localStorage.setItem("otomobilvasitacat2Yitems", items);
            }
        });
        localStorage.setItem("otomobilvasitacat1Yselectedindex", document.getElementById('otomobilvasitaCat1Y').selectedIndex);

        UpdateWebGrid();
    });
});
$(function () {
    $('#otomobilvasitaCat1Y').change(function () {
        localStorage.setItem("otomobilvasitacat1Yselectedindex", document.getElementById('otomobilvasitaCat1Y').selectedIndex);
    });
});
$(function () {
    $('#otomobilvasitaCat2Y').change(function () {
        localStorage.setItem("otomobilvasitacat2Yselectedindex", document.getElementById('otomobilvasitaCat2Y').selectedIndex);
    });
});
$(function () {
    $('#otomobilvasitaCat6').change(function () {
        localStorage.setItem("otomobilvasitacat6selectedindex", document.getElementById('otomobilvasitaCat6').selectedIndex);
    });
});

//  Google Facebook etc ///*********************

//(function (i, s, o, g, r, a, m) {
//    i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
//        (i[r].q = i[r].q || []).push(arguments)
//    }, i[r].l = 1 * new Date(); a = s.createElement(o),
//    m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
//})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

//ga('create', 'UA-54979446-1', 'auto');
//ga('send', 'pageview');


//$(function () {
//    $("#tabs").tabs();
//});

//********************



var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};

$(document).ready(function () {
    var gs = getUrlParameter('gsearch');
    if (gs != null) {
        loadGeneralSearch();
    }

    $('.tab-pane').btsListFilter('#searchinput', { itemChild: 'a' });
})

//*****************************

$("#favorite").change(function () {
    $.ajax({
        url: '@Url.Action("aracozellikleri", "otomobilvasita")',
        type: 'GET',
        data: { prdcId: $('#favorite').val().split('=')[1].toString() },
        success: function (result) {
            var url = "@Url.Action('aracozellikleri', 'otomobilvasita')?prdcId=" + $('#favorite').val().split('=')[1].toString();
            window.location.href = url;
        }
    });
});

$(document).ready(function () {
    $('#prdcDetSmallPics2').magnificPopup({
        delegate: 'a',
        type: 'image',
        tLoading: 'Resim Yükleniyor #%curr%...',
        mainClass: 'mfp-img-mobile maximagesize',
        gallery: {
            enabled: true,
            navigateByImgClick: true,
            preload: [0, 1]
        },
        image: {
            tError: '<a href="%url%">Resim #%curr%</a> yüklenemedi.',
            titleSrc: function (item) {
                return item.el.attr('title');
            }
        },
        removalDelay: 200,
        mainClass: 'mfp-fade'
    });
});

$("#editPrdcBtn").click(function () {
    location.href = '/otomobilvasita/aracozellikleriduzenle?prdcId=' + document.getElementById('productId').value + '&productCatIds=' + document.getElementById('productCatIds').value + '&productGroupIds=' + document.getElementById('productGroupIds').value;
});

$(".prdcDetSmallPic").click(function () {
    document.getElementById("prdcDetailImg").src = this.src;
});

function prdClick(product) {
    $.ajax({
        type: "POST",
        url: "/otomobilvasita/PrdDelete",
        data: "id=" + product,
        success: function (data) {
            alert(data.message);
        }
    })
}

var $ = jQuery.noConflict();

$(function () {
    $('#modalPopup1').click(function () {
        var url = $(this).attr('title');
        var dialog = $('<div style="display:none; width:640px; height:480px; overflow-y: scroll;position:relative;z-index:9999;"></div>').appendTo('body');
        dialog.load(url, {},
            function (responseText, textStatus, XMLHttpRequest) {
                dialog.dialog({
                    maxHeight: 480,
                    width: 640,
                    close: function (event, ui) {
                        dialog.remove();
                    }
                });
            });
        return false;
    });
});
$(function () {
    $('#modalPopup2').click(function () {
        var url = $(this).attr('title');
        var dialog = $('<div style="display:none; width:640px; height:480px; overflow-y: scroll;position:relative;z-index:9999;"></div>').appendTo('body');
        dialog.load(url, {},
            function (responseText, textStatus, XMLHttpRequest) {
                dialog.dialog({
                    maxHeight: 480,
                    width: 640,
                    close: function (event, ui) {
                        dialog.remove();
                    }
                });
            });
        return false;
    });
});

//**** Spareparts
$('#otomobilvasitaGrid2').on('hover', 'tr', function () {
    if (this.parentElement.nodeName == "TBODY") {
        $(this).toggleClass('clickable')
    }
}).on('click', 'tr', function () {
    if (this.parentElement.nodeName == "TBODY") {
        location.href = '/otomobilvasita/aracozellikleri/?prdcId=' + $(this).find('#productId').val() + '&productGroup=' + $(this).find('#productGroup').val() + '&productCat=' + $(this).find('#productCat').val() + '&productCurr=' + $(this).find('#productCurr').val();
    }
    else if (this.parentElement.nodeName == "THEAD") {
        location.href = '/otomobilvasita/SearchProductVehicle/?CatagoryId=-1&MarkId=1';
    }
});

$("#searchTextIdSparepart").keyup(function (event) {
    if (event.which == 13) {
        TempData["searchText"] = document.getElementById("searchTextIdSparepart").value;
    }
});

$(function () {
    $('#otomobilvasitaCat1').click(function () {
        $.getJSON('/otomobilvasita/CategoryList/' + $('#otomobilvasitaCat1').val(), function (data) {
            var items = '<option value=\'null\'>Seçim yapınız.</option>';
            $.each(data, function (i, chosen) {
                items += "<option value='" + chosen.Value + "'>" + chosen.Text + "</option>";
            });
            if (items != '') {
                $('#otomobilvasitaCat2').html(items);
                localStorage.setItem("otomobilvasitacat2items", items);
            }
        });
        localStorage.setItem("otomobilvasitacat1selectedindex", document.getElementById('otomobilvasitaCat1').selectedIndex);
    });
});
$(function () {
    $('#otomobilvasitaCat1').change(function () {
        $.getJSON('/otomobilvasita/CategoryList/' + $('#otomobilvasitaCat1').val(), function (data) {
            var items = '<option value=\'null\'>Seçim yapınız.</option>';
            $.each(data, function (i, chosen) {
                items += "<option value='" + chosen.Value + "'>" + chosen.Text + "</option>";
            });
            if (items != '') {
                $('#otomobilvasitaCat2').html(items);
                localStorage.setItem("otomobilvasitacat2items", items);
            }
        });
        localStorage.setItem("otomobilvasitacat1selectedindex", document.getElementById('otomobilvasitaCat1').selectedIndex);
        document.getElementById('otomobilvasitaCat3').selectedIndex = 0;
        document.getElementById('otomobilvasitaCat6').selectedIndex = 0;
    });
});

$(function () {
    $('#otomobilvasitaCat2').click(function () {
        $.getJSON('/otomobilvasita/CategoryList/' + $('#otomobilvasitaCat2').val(), function (data) {
            var items = '<option value=\'null\'>Seçim yapınız.</option>';
            $.each(data, function (i, chosen) {
                items += "<option value='" + chosen.Value + "'>" + chosen.Text + "</option>";
            });
            if (items != '') {
                $('#otomobilvasitaCat3').html(items);
                localStorage.setItem("otomobilvasitacat3items", items);
            }
        });
        localStorage.setItem("otomobilvasitacat2selectedindex", document.getElementById('otomobilvasitaCat2').selectedIndex);
    });
});
$(function () {
    $('#otomobilvasitaCat2').change(function () {
        $.getJSON('/otomobilvasita/CategoryList/' + $('#otomobilvasitaCat2').val(), function (data) {
            var items = '<option value=\'null\'>Seçim yapınız.</option>';
            $.each(data, function (i, chosen) {
                items += "<option value='" + chosen.Value + "'>" + chosen.Text + "</option>";
            });
            if (items != '') {
                $('#otomobilvasitaCat3').html(items);
                localStorage.setItem("otomobilvasitacat3items", items);
            }
        });
        localStorage.setItem("otomobilvasitacat2selectedindex", document.getElementById('otomobilvasitaCat2').selectedIndex);
        document.getElementById('otomobilvasitaCat6').selectedIndex = 0;
    });
});

$(function () {
    $('#otomobilvasitaCat3').click(function () {
        $.getJSON('/otomobilvasita/CategoryList/' + $('#otomobilvasitaCat3').val(), function (data) {
            var items = '<option value=\'null\'>Seçim yapınız.</option>';
            $.each(data, function (i, chosen) {
                items += "<option value='" + chosen.Value + "'>" + chosen.Text + "</option>";
            });
            if (items != '') {
                $('#otomobilvasitaCat6').html(items);
                localStorage.setItem("otomobilvasitacat6items", items);
            }
        });
        localStorage.setItem("otomobilvasitacat3selectedindex", document.getElementById('otomobilvasitaCat3').selectedIndex);
    });
});
$(function () {
    $('#otomobilvasitaCat3').change(function () {
        $.getJSON('/otomobilvasita/CategoryList/' + $('#otomobilvasitaCat3').val(), function (data) {
            var items = '<option value=\'null\'>Seçim yapınız.</option>';
            $.each(data, function (i, chosen) {
                items += "<option value='" + chosen.Value + "'>" + chosen.Text + "</option>";
            });
            if (items != '') {
                $('#otomobilvasitaCat6').html(items);
                localStorage.setItem("otomobilvasitacat6items", items);
            }
        });
        localStorage.setItem("otomobilvasitacat3selectedindex", document.getElementById('otomobilvasitaCat3').selectedIndex);
    });
});



$(function () {
    $('#otomobilvasitaCat1Y').click(function () {
        $.getJSON('/otomobilvasita/CategoryListSpareParts/' + $('#otomobilvasitaCat1Y').val(), function (data) {
            var items = '<option value=\'null\'>Seçim yapınız.</option>';
            $.each(data, function (i, chosen) {
                items += "<option value='" + chosen.Value + "'>" + chosen.Text + "</option>";
            });
            if (items != '') {
                $('#otomobilvasitaCat2Y').html(items);
                localStorage.setItem("otomobilvasitacat2Yitems", items);
            }
        });
        localStorage.setItem("otomobilvasitacat1Yselectedindex", document.getElementById('otomobilvasitaCat1Y').selectedIndex);
    });
});
$(function () {
    $('#otomobilvasitaCat1Y').change(function () {
        $.getJSON('/otomobilvasita/CategoryListSpareParts/' + $('#otomobilvasitaCat1Y').val(), function (data) {
            var items = '<option value=\'null\'>Seçim yapınız.</option>';
            $.each(data, function (i, chosen) {
                items += "<option value='" + chosen.Value + "'>" + chosen.Text + "</option>";
            });
            if (items != '') {
                $('#otomobilvasitaCat2Y').html(items);
                localStorage.setItem("otomobilvasitacat2Yitems", items);
            }
        });
        localStorage.setItem("otomobilvasitacat1Yselectedindex", document.getElementById('otomobilvasitaCat1Y').selectedIndex);
    });
});
$(function () {
    $('#otomobilvasitaCat1Y').change(function () {
        localStorage.setItem("otomobilvasitacat1Yselectedindex", document.getElementById('otomobilvasitaCat1Y').selectedIndex);
    });
});
$(function () {
    $('#otomobilvasitaCat2Y').change(function () {
        localStorage.setItem("otomobilvasitacat2Yselectedindex", document.getElementById('otomobilvasitaCat2Y').selectedIndex);
    });
});
$(function () {
    $('#otomobilvasitaCat6').change(function () {
        localStorage.setItem("otomobilvasitacat6selectedindex", document.getElementById('otomobilvasitaCat6').selectedIndex);
    });
});
//***********************