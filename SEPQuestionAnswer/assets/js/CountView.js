var check = 0;
var view = {
    init: function () {
        view.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            if (check == 0) {
                check = id;
                $.ajax({
                    type: "POST",
                    url: "http://cntttest.vanlanguni.edu.vn:18080/SEP24Team10/Home/countView",
                    data: { id: id },
                    dataType: "json",
                });
            } else if (check == id) {
                check = 0;
                $.ajax({
                    url: "http://cntttest.vanlanguni.edu.vn:18080/SEP24Team10/Home/countView2",
                    data: { id: id },
                    dataType: "json",
                    type: "POST",
                });
            } else {
                check = id;
                $.ajax({
                    url: "http://cntttest.vanlanguni.edu.vn:18080/SEP24Team10/Home/countView",
                    data: { id: id },
                    dataType: "json",
                    type: "POST",
                });
            }
        });
    }
}
view.init();
