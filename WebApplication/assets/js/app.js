var appModule = (function () {
    function init() {
        ajaxError();
        initEvents();
    }
    ajaxEvents =
    {
        OnBegin: function () {
            showLoading();
        },
        OnComplete: function (element) {
            hideLoading();
        },
        OnSuccess: function (response, status, xhr) {
            if (response != null) {
                if (response.Message != null) {
                    if (response.Completed) {
                        toastr.options.onHidden = function () {
                            if (response.Completed && response.ReturnUrl != null) {
                                window.location.href = response.ReturnUrl;
                                return;
                            }
                            if (response.Callback != null && response.Callback.length > 0) {
                                eval(atob(response.Callback));
                            }
                        }
                        toastr.success(response.Message);
                    } else {
                        toastr.error(response.Message);
                    }
                } else if (response.Completed) {
                    if (response.returnUrl != null) {
                        window.location.href = response.ReturnUrl;
                        return;
                    }

                    if (response.Callback != null && response.Callback.length > 0) {
                        eval(atob(response.Callback));
                    }
                } else {
                    if (response.ReturnUrl != null) {
                        window.location.href = response.ReturnUrl;
                        return;
                    }
                }
            }
            hideLoading();
        },
        OnFailed: function () {
            hideLoading();
            toastr.error('Quý khách vui lòng thử lại sau.');
        }
    };
    function ajaxError() {
        $(document).ajaxError(function (e, xhr) {
            if (xhr.status === 401 || xhr.status === 403) {
                var response = $.parseJSON(xhr.responseText);
                toastr.options.onHidden = function () {
                    if (response.url != null) {
                        window.location.href = response.url;
                        return;
                    }
                }
                toastr.error(response.message);
            }
        });
    }
    function initEvents() {
        $(document).on('click', '.btn-submit', function (e) {
            e.preventDefault();
            $(this).closest('form').submit();
        });
        $(document).on('click', '.btn-search', function (e) {
            e.preventDefault();
            var form = $(this).closest('form');
            form.find('input[name="Page"]').first().val(1);
            form.attr('method', 'get').submit();
        });
        $(document).on('click', '.btn-export-excel', function (e) {
            e.preventDefault();
            $(this).closest('form').attr('method', 'post').submit();
        });
        $(document).on('click', '.save-customer', function (e) {
            e.preventDefault();
            var self = $(this), tr = self.closest('tr'), customerId = tr.data('id');
            if (typeof customerId != typeof undefined) {
                $(`#customer${customerId}_edit_form`).submit();
            }
        });
        $(document).on('change', 'select[name="ChartsType"]', function () {
            fetchData(
                {
                    type: 'POST',
                    url: '/api/charts/get.html',
                    dataType: 'json',
                    beforeSend: function () {
                        $('#chart-active-users-2').empty();
                    },
                    data: { chartsType: $(this).val() }
                })
                .then((data) => {
                    if (data != null && data.length > 0) {
                        sum = 0;
                        $.each(data, function () { sum += parseFloat(this.Id) || 0; });
                        $('.total-customers').text(sum);

                        window.ApexCharts && (new ApexCharts(document.getElementById('chart-active-users-2'), {
                            chart: {
                                defaultLocale: 'vi',
                                locales: [{
                                    name: 'vi',
                                    options: {
                                        months: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
                                        shortMonths: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
                                        days: ['Chủ nhật', 'Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7'],
                                        shortDays: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
                                        toolbar: {
                                            download: 'Download SVG',
                                            selection: 'Selection',
                                            selectionZoom: 'Selection Zoom',
                                            zoomIn: 'Zoom In',
                                            zoomOut: 'Zoom Out',
                                            pan: 'Panning',
                                            reset: 'Reset Zoom',
                                        }
                                    }
                                }],
                                type: "line",
                                fontFamily: 'inherit',
                                height: 288,
                                parentHeightOffset: 0,
                                toolbar: {
                                    show: false,
                                },
                                animations: {
                                    enabled: true
                                },
                            },
                            fill: {
                                opacity: 1,
                            },
                            stroke: {
                                width: 2,
                                lineCap: "round",
                                curve: "smooth",
                            },
                            series: [{
                                name: "Khách hàng",
                                data: $.map(data, function (obj) {
                                    return obj.Id
                                })
                            }],
                            grid: {
                                padding: {
                                    top: -20,
                                    right: 0,
                                    left: -4,
                                    bottom: -4
                                },
                                strokeDashArray: 4,
                            },
                            xaxis: {
                                labels: {
                                    padding: 0,
                                    format: 'dd/MM/yyyy',
                                },
                                tooltip: {
                                    enabled: true
                                },
                                type: 'datetime',
                            },
                            yaxis: {
                                labels: {
                                    padding: 4
                                },
                            },
                            labels: $.map(data, function (obj) {
                                return obj.Name
                            }),
                            colors: ["#206bc4", "#45aaf2", "#5eba00"],
                            legend: {
                                show: false,
                            },
                        })).render();
                    }
                })
                .catch((error) => {
                    console.log(error);
                    if (error.status != 401 && error.status != 403) {
                        toastr.error('Quý khách vui lòng liên hệ với bộ phận kỹ thuật để biết thêm chi tiết !');
                    }
                });
        });
        $(document).on('change',
            'select[name="Year"], select[name="Month"]',
            function () {
                var y = $('select[name="Year"] option:selected').val();
                var m = $('select[name="Month"] option:selected').val();
                var d = $('select[name="Day"] option:selected').val();
                getBirthDay(d, m, y);
            });
        $(document).on('change', 'input[type=checkbox]', '.form-checkbox', function () {
            var parent = $(this).closest('label');
            if (this.checked) {
                parent.find('input[type=hidden]').remove();
            } else {
                var input = parent.find('input[type=hidden]');
                if (input.length) {
                    input.val($(this).val());
                } else {
                    parent
                        .append('<input type="hidden" value="' + $(this).val() + '" name="RoleIdsRemove" />');
                }
            }
        });
        $("#nav-toggle").on("click", (function (t) {
            t.preventDefault(), $("#db-wrapper").toggleClass("toggled")
        }));

        $(document).on('click', '.edit-customer', function (e) {
            e.preventDefault();
            var self = $(this), tr = self.closest('tr'), customerId = tr.data('id'), orderBy = tr.data('o');
            if (typeof orderBy != typeof undefined && typeof customerId != typeof undefined) {
                fetchData(
                    {
                        type: 'POST',
                        url: '/ui/customer/edit.html',
                        dataType: 'html',
                        data: { customerId: customerId, orderBy: orderBy }
                    })
                    .then((data) => {
                        if (data != null && data.length > 0) {
                            tr.addClass('hight-line').html(data);
                            tr.find('input[name="Mobile"]').first().focus();
                            validatorForm($(`#customer${customerId}_edit_form`));
                        }
                    })
                    .catch((error) => {
                        console.log(error);
                        if (error.status != 401 && error.status != 403) {
                            toastr.error('Quý khách vui lòng liên hệ với bộ phận kỹ thuật để biết thêm chi tiết !');
                        }
                    });
            }
        });

        $(document).on('click', '.view-customer', function (e) {
            e.preventDefault();
            var self = $(this), tr = self.closest('tr'), customerId = tr.data('id'), orderBy = tr.data('o');
            if (typeof orderBy != typeof undefined && typeof customerId != typeof undefined) {
                fetchData(
                    {
                        type: 'POST',
                        url: '/ui/customer/view.html',
                        dataType: 'html',
                        data: { customerId: customerId, orderBy: orderBy }
                    })
                    .then((data) => {
                        if (data != null && data.length > 0) {
                            tr.removeClass('hight-line').html(data);
                        }
                    })
                    .catch((error) => {
                        console.log(error);
                        if (error.status != 403) {
                            toastr.error('Quý khách vui lòng liên hệ với bộ phận kỹ thuật để biết thêm chi tiết !');
                        }
                    });
            }
        });

        $(document).on('click', '.remove-customer', function (e) {
            e.preventDefault();
            var self = $(this), tr = self.closest('tr'), customerId = tr.data('id');
            if (typeof customerId != typeof undefined) {
                if (confirm('Xác nhận xóa khách hàng đã chọn ?')) {
                    fetchData(
                        {
                            type: 'POST',
                            url: '/api/customer/remove.html',
                            dataType: 'json',
                            data: { customerId: customerId }
                        })
                        .then((response) => {
                            if (response != null) {
                                if (response.Message != null) {
                                    if (response.Completed) {
                                        toastr.options.onHidden = function () {
                                            if (response.Callback != null && response.Callback.length > 0) {
                                                eval(atob(response.Callback));
                                            }
                                        }
                                        toastr.success(response.Message);
                                    } else {
                                        toastr.error(response.Message);
                                    }
                                }
                            }
                        })
                        .catch((error) => {
                            console.log(error);
                            if (error.status != 401 && error.status != 403) {
                                toastr.error('Quý khách vui lòng liên hệ với bộ phận kỹ thuật để biết thêm chi tiết !');
                            }
                        });
                }
            }
        });

        $(document).on('click', '.remove-user', function (e) {
            e.preventDefault();
            var self = $(this), url = self.data('url');
            if (typeof url != typeof undefined) {
                showMessage({
                    buttons: [{
                        Name: 'Đóng',
                        ClassName: 'btn-law',
                        ClickEvent: function () { }
                    },
                    {
                        Name: 'Xác nhận',
                        ClassName: 'btn-law-ok',
                        ClickEvent: function () {
                            window.location.href = url;
                        }
                    }],
                    messages: ['Xác nhận xóa biên tập viên đã chọn ?', '<p>Ghi chú:</p>', `<ul>
                        <li> Để tránh thao tác xóa nhầm tài khoản, 
                         sau thao tác này, hệ thống sẽ chuyển tài khoản sang trạng thái <span class="badge bg-warning">Xóa</span></li>
                        <li> Thực hiện thao tác xóa 1 lần nữa để xóa hoàn toàn tài khoản khỏi hệ thống.</li>
                      </ul>`]
                });
            }
        });

        var selectOptions = {
            ajax: {
                url: '/ajax/getmobiles',
                delay: 250,
                async: true,
                cache: true,
                dataType: 'json',
                params: {
                    contentType: 'application/json; charset=utf-8'
                },
                data: function (params) {
                    var query = {
                        name: params.term
                    }
                    return query;
                },
                processResults: function (data) {
                    return {
                        results: $.map(data,
                            function (item) {
                                return {
                                    text: item.Name,
                                    id: item.Id
                                }
                            })
                    };
                }
            }
        }
        $('select.select2').select2({
            minimumResultsForSearch: Infinity,
            placeholder: {
                id: -1,
                text: 'Enter the Student id.',
            }});
        selectOptions.ajax.url = '/api/user/get.html'
        $('select[name="UserId"].select2').select2(selectOptions);
    }
    function dayList(month, year) {
        var day = new Date(year, month, 0);
        return day.getDate();
    }
    function getBirthDay(d, m, y) {
        var date = new Date(), days = dayList(m == 0 ? date.getMonth() + 1 : m, y == 0 ? date.getFullYear() : y),
            selectDay = $('select[name="Day"]');
        selectDay.html('<option value="0"> Ngày </option>');
        for (var i = 1; i <= days; i++) {
            selectDay.append('<option value="' + i + '" ' + (i == d ? 'selected' : '') + '>' + i + '</option>');
        };
    }
    function validatorForm(form) {
        form.removeData('validator');
        form.removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse(form);
    }
    function showLoading() {
        $('#spinner-back').addClass('show');
        $('#spinner-front').addClass('show');
    }
    function hideLoading() {
        $('#spinner-back').removeClass('show');
        $('#spinner-front').removeClass('show');
    }
    function loadScript(src, async = true, type = "text/javascript") {
        return new Promise((resolve, reject) => {
            try {
                const el = document.createElement("script");
                const container = document.head || document.body;

                el.type = type;
                el.async = async;
                el.src = src;

                el.addEventListener("load", () => {
                    resolve({ status: true });
                });

                el.addEventListener("error", () => {
                    reject({
                        status: false,
                        message: `Failed to load the script ${src}`
                    });
                });

                container.appendChild(el);
            } catch (err) {
                reject(err);
            }
        });
    }
    function fetchData(options) {
        return new Promise((resolve, reject) => {
            $.ajax({
                cache: false,
                url: options.url,
                type: options.type || 'GET',
                data: options.data || {},
                dataType: options.dataType || 'json',
                beforeSend: options.beforeSend || showLoading(),
                success: function (data) {
                    resolve(data);
                },
                error: function (error) {
                    reject(error);
                }
            }).always(function () {
                options.always || {};
                hideLoading();
            });
        });
    }
    init();
    return {
        fetchData: fetchData,
        ajaxEvents: ajaxEvents,
        validatorForm: validatorForm,
        loadScript: loadScript
    };
})();

(function () {
    $.validator.unobtrusive.adapters.addSingleVal("allowfilesize", "filesize");
    $.validator.unobtrusive.adapters.addSingleVal("allowfileextensions", "fileextensions");

    $.validator.addMethod('allowfilesize', function (value, element, maxSize) {
        return convertBytesToMegabytes(element.files[0].size) <= parseFloat(maxSize);
    });

    $.validator.addMethod('allowfileextensions', function (value, element, validFileTypes) {
        if (validFileTypes.indexOf(',') > -1) {
            validFileTypes = validFileTypes.split(',');
        } else {
            validFileTypes = [validFileTypes];
        }

        var fileType = value.split('.')[value.split('.').length - 1];

        for (var i = 0; i < validFileTypes.length; i++) {
            if (validFileTypes[i] === fileType) {
                return true;
            }
        }

        return false;
    });

    function convertBytesToMegabytes(bytes) {
        return (bytes / 1024) / 1024;
    }
})();