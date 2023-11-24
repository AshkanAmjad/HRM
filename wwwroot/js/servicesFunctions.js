//تابع پذیرفتن یک مقدار در چک باکس
$(document).ready(function () {

    $('.checkoption').click(function () {
        $('.checkoption').not(this).prop('checked', false);
    });
    $('.checkoption--2').click(function () {
        $('.checkoption--2').not(this).prop('checked', false);
    });
    $('.checkoption--3').click(function () {
        $('.checkoption--3').not(this).prop('checked', false);
    });
    $('.checkoption--4').click(function () {
        $('.checkoption--4').not(this).prop('checked', false);
    });
    $('.checkoption--5').click(function () {
        $('.checkoption--5').not(this).prop('checked', false);
    });
    $('.checkoption--6').click(function () {
        $('.checkoption--6').not(this).prop('checked', false);
    });

});

//تابع جست وجو
function search() {
    let filterSelcted, filterSelctedValue, input, filter, table, tr, td, txtValue, th;
    filterSelcted = document.getElementById("filter");
    filterSelctedValue = filterSelcted.value;
    input = document.getElementById("search");
    filter = input.value;
    table = document.getElementById("myTable");
    tr = table.getElementsByTagName("tr");
    for (let i = 0; i < tr.length; i++) {
        if (filterSelctedValue == 0) {
            td = tr[i].getElementsByTagName("td")[0]
        }
        else if (filterSelctedValue == 2) {
            td = tr[i].getElementsByTagName("td")[2]
        }
        else if (filterSelctedValue == 3) {
            td = tr[i].getElementsByTagName("td")[3]
        }
        else if (filterSelctedValue == 4) {
            td = tr[i].getElementsByTagName("td")[4]
        }
        else if (filterSelctedValue == 5) {
            td = tr[i].getElementsByTagName("td")[5]
        }
        else if (filterSelctedValue == 6) {
            td = tr[i].getElementsByTagName("td")[6]
        }
        else if (filterSelctedValue == 7) {
            td = tr[i].getElementsByTagName("td")[7]
        }
        else if (filterSelctedValue == 8) {
            td = tr[i].getElementsByTagName("td")[8]
        }
        else if (filterSelctedValue == 9) {
            td = tr[i].getElementsByTagName("td")[9]
        }
        else if (filterSelctedValue == 10) {
            td = tr[i].getElementsByTagName("td")[10]
        }
        else if (filterSelctedValue == 11) {
            td = tr[i].getElementsByTagName("td")[11]
        }
        else if (filterSelctedValue == 12) {
            td = tr[i].getElementsByTagName("td")[12]
        }
        else if (filterSelctedValue == 13) {
            td = tr[i].getElementsByTagName("td")[13]
        }
        else if (filterSelctedValue == 14) {
            td = tr[i].getElementsByTagName("td")[14]
        }
        else if (filterSelctedValue == 15) {
            td = tr[i].getElementsByTagName("td")[15]
        }
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.indexOf(filter) > -1) {
                tr[i].classList.remove("hidden");
                document.getElementById("myTable").classList.remove("hidden");
                document.getElementById("notFound").classList.add("hidden");
            }
            else {
                tr[i].classList.add("hidden");
            }
        }
        else {
            document.getElementById("myTable").classList.add("hidden");
            document.getElementById("notFound").classList.remove("hidden");
            var delay = 5000;
            setTimeout(function () {
                document.getElementById("notFound").classList.add("hidden");
                document.getElementById("myTable").classList.remove("hidden");
                tr.classList.remove("hidden");
            }, delay);
        }
        if (filterSelctedValue == "") {
            tr[i].classList.remove("hidden");
        }
    }
}

//تابع جست وجو در صفحه مدارک 
function searchDocuments() {
    let filterSelcted, filterSelctedValue, input, filter, table, tr, td, txtValue, th;
    filterSelcted = document.getElementById("filter");
    filterSelctedValue = filterSelcted.value;
    input = document.getElementById("searchDocuments");
    filter = input.value;
    table = document.getElementById("myTable");
    tr = table.getElementsByTagName("tr");
    for (let i = 0; i < tr.length; i++) {
        if (filterSelctedValue == 2) {
            td = tr[i].getElementsByTagName("td")[1]
        }
        else if (filterSelctedValue == 3) {
            td = tr[i].getElementsByTagName("td")[2]
        }
        else if (filterSelctedValue == 4) {
            td = tr[i].getElementsByTagName("td")[3]
        }
        else if (filterSelctedValue == 5) {
            td = tr[i].getElementsByTagName("td")[4]
        }
        else if (filterSelctedValue == 6) {
            td = tr[i].getElementsByTagName("td")[5]
        }
        else if (filterSelctedValue == 9) {
            td = tr[i].getElementsByTagName("td")[8]
        }
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.indexOf(filter) > -1) {
                tr[i].classList.remove("hidden");
                document.getElementById("myTable").classList.remove("hidden");
                document.getElementById("notFound").classList.add("hidden");
            }
            else {
                tr[i].classList.add("hidden");
            }
        }
        else {
            document.getElementById("myTable").classList.add("hidden");
            document.getElementById("notFound").classList.remove("hidden");
            var delay = 5000;
            setTimeout(function () {
                document.getElementById("notFound").classList.add("hidden");
                document.getElementById("myTable").classList.remove("hidden");
                tr.classList.remove("hidden");
            }, delay);
        }
        if (filterSelctedValue == "") {
            tr[i].classList.remove("hidden");
        }
    }
}
document.body.addEventListener('keydown', function (e) {
    var keyCode = e.keyCode;
    if (keyCode == 13) {
        document.getElementById('search-btn').click();
    }
});


//تابع جست و جو مدارک انتقالی 
function SearchExchangeDocuments() {
    let filterSelcted, filterSelctedValue, input, filter, table, tr, td, txtValue, th;
    filterSelcted = document.getElementById("filter");
    filterSelctedValue = filterSelcted.value;
    input = document.getElementById("searchDocuments");
    filter = input.value;
    table = document.getElementById("myTable");
    tr = table.getElementsByTagName("tr");
    for (let i = 0; i < tr.length; i++) {
        if (filterSelctedValue == 1) {
            td = tr[i].getElementsByTagName("td")[1]
        }
        else if (filterSelctedValue == 2) {
            td = tr[i].getElementsByTagName("td")[2]
        }
        else if (filterSelctedValue == 3) {
            td = tr[i].getElementsByTagName("td")[3]
        }
        else if (filterSelctedValue == 4) {
            td = tr[i].getElementsByTagName("td")[4]
        }
        else if (filterSelctedValue == 5) {
            td = tr[i].getElementsByTagName("td")[5]
        }
        else if (filterSelctedValue == 6) {
            td = tr[i].getElementsByTagName("td")[6]
        }
        else if (filterSelctedValue == 13) {
            td = tr[i].getElementsByTagName("td")[13]
        }
        else if (filterSelctedValue == 16) {
            td = tr[i].getElementsByTagName("td")[16]
        }
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.indexOf(filter) > -1) {
                tr[i].classList.remove("hidden");
                document.getElementById("myTable").classList.remove("hidden");
                document.getElementById("notFound").classList.add("hidden");
            }
            else {
                tr[i].classList.add("hidden");
            }
        }
        else {
            document.getElementById("myTable").classList.add("hidden");
            document.getElementById("notFound").classList.remove("hidden");
            var delay = 5000;
            setTimeout(function () {
                document.getElementById("notFound").classList.add("hidden");
                document.getElementById("myTable").classList.remove("hidden");
                tr.classList.remove("hidden");
            }, delay);
        }
        if (filterSelctedValue == "") {
            tr[i].classList.remove("hidden");
        }
    }
}
document.body.addEventListener('keydown', function (e) {
    var keyCode = e.keyCode;
    if (keyCode == 13) {
        document.getElementById('search-btn').click();
    }
});

//تابع نمایش پیام موفقیت آمیز بودن عمل فعال سازی
function ShowActivationMessage() {
    document.getElementById("activer").classList.remove("hidden");
}

//تابع نمایش عکس بارگذاری شده در فرم های ثبت نام و ویرایش
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (r) {
            $('#imgAvatar').attr('src', r.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

//تغییر اندازه عکس بارگذاری شده در فرم های ثبت نام و ویرایش برای نمایش
$(document).ready(function () {
    $("#homeImgAvatar").css("width", 90);
    $("#homeImgAvatar").css("height", 90);
    $("#imgAvatar").css("width", 160);
    $("#imgAvatar").css("height", 160);
    $("#listDeleteImgAvatar").css("width", 70);
    $("#listDeleteImgAvatar").css("height", 70);
});

//تابع نمایش پنجره ثبت نقش جدید
function ShowAddRoleAlert() {
    $('.toast').toast('show');
}

//tooltip
$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

//نمایش فرم ارسال مدارک در تبادل مدارک سطح استان
function ShowSendDocumentsFormProvince() {
    let sendDocumentsFormProvince, sendDocumentsFormCounty, sendDocumentsFormDistrict;
    sendDocumentsFormProvince = document.getElementById("sendDocumentsFormProvince");
    sendDocumentsFormCounty = document.getElementById("sendDocumentsFormCounty");
    sendDocumentsFormDistrict = document.getElementById("sendDocumentsFormDistrict");
    if (sendDocumentsFormCounty.classList.contains("hidden") == false) {
        sendDocumentsFormCounty.classList.add("hidden");
    }
    if (sendDocumentsFormDistrict.classList.contains("hidden") == false) {
        sendDocumentsFormDistrict.classList.add("hidden");
    }
    sendDocumentsFormProvince.classList.remove("hidden");
}

//نمایش فرم ارسال مدارک در تبادل مدارک سطح شهرستان
function ShowSendDocumentsFormCounty() {
    let sendDocumentsFormProvince, sendDocumentsFormCounty, sendDocumentsFormDistrict;
    sendDocumentsFormProvince = document.getElementById("sendDocumentsFormProvince");
    sendDocumentsFormCounty = document.getElementById("sendDocumentsFormCounty");
    sendDocumentsFormDistrict = document.getElementById("sendDocumentsFormDistrict");
    if (sendDocumentsFormProvince.classList.contains("hidden") == false) {
        sendDocumentsFormProvince.classList.add("hidden");
    }
    if (sendDocumentsFormDistrict.classList.contains("hidden") == false) {
        sendDocumentsFormDistrict.classList.add("hidden");
    }
    sendDocumentsFormCounty.classList.remove("hidden");
}

//نمایش فرم ارسال مدارک در تبادل مدارک سطح بخش
function ShowSendDocumentsFormDistrict() {
    let sendDocumentsFormProvince, sendDocumentsFormCounty, sendDocumentsFormDistrict;
    sendDocumentsFormProvince = document.getElementById("sendDocumentsFormProvince");
    sendDocumentsFormCounty = document.getElementById("sendDocumentsFormCounty");
    sendDocumentsFormDistrict = document.getElementById("sendDocumentsFormDistrict");
    if (sendDocumentsFormCounty.classList.contains("hidden") == false) {
        sendDocumentsFormCounty.classList.add("hidden");
    }
    if (sendDocumentsFormProvince.classList.contains("hidden") == false) {
        sendDocumentsFormProvince.classList.add("hidden");
    }
    sendDocumentsFormDistrict.classList.remove("hidden");
}


//نمایش گزارش شعبه 1 شهرستان
function ShowCountyOneChart() {
    let countyOneChart, countyTwoChart, countyThreeChart;
    countyOneChart = document.getElementById("countyOneChart");
    countyTwoChart = document.getElementById("countyTwoChart");
    countyThreeChart = document.getElementById("countyThreeChart");
    if (countyTwoChart.classList.contains("hidden") == false) {
        countyTwoChart.classList.add("hidden");
    }
    if (countyThreeChart.classList.contains("hidden") == false) {
        countyThreeChart.classList.add("hidden");
    }
    countyOneChart.classList.remove("hidden");
}

//نمایش گزارش شعبه 2 شهرستان
function ShowCountyTwoChart() {
    let countyOneChart, countyTwoChart, countyThreeChart;
    countyOneChart = document.getElementById("countyOneChart");
    countyTwoChart = document.getElementById("countyTwoChart");
    countyThreeChart = document.getElementById("countyThreeChart");
    if (countyOneChart.classList.contains("hidden") == false) {
        countyOneChart.classList.add("hidden");
    }
    if (countyThreeChart.classList.contains("hidden") == false) {
        countyThreeChart.classList.add("hidden");
    }
    countyTwoChart.classList.remove("hidden");
}

//نمایش گزارش شعبه 3 شهرستان
function ShowCountyThreeChart() {
    let countyOneChart, countyTwoChart, countyThreeChart;
    countyOneChart = document.getElementById("countyOneChart");
    countyTwoChart = document.getElementById("countyTwoChart");
    countyThreeChart = document.getElementById("countyThreeChart");
    if (countyTwoChart.classList.contains("hidden") == false) {
        countyTwoChart.classList.add("hidden");
    }
    if (countyOneChart.classList.contains("hidden") == false) {
        countyOneChart.classList.add("hidden");
    }
    countyThreeChart.classList.remove("hidden");
}
