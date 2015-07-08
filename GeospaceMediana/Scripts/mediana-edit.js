(function() { 
    function editEvent(e) {
        var currentCell = $(this);

        if (this.children["edit"] != null)
            return;

        var oldValue = $(currentCell).text().trim();

        var inputHtml = "<input type=\"text\" id=\"edit\" value=\"" + oldValue + "\" />";

        var cellWidth = currentCell.width();
        var cellHeight = currentCell.height();

        currentCell.empty().append(inputHtml);

        currentCell.css("padding", "0px");
        $('#edit').width(cellWidth - 2 + 16); // 1+1 border width
        $('#edit').height(cellHeight - 2 + 16);
        currentCell.width(cellWidth);
        currentCell.height(cellHeight);

        $('#edit').focus();
        $('#edit').select();
        $('#edit').blur(function () {

            var newValue = $(this).val().trim();
            currentCell.width("");
            currentCell.height("");
            currentCell.css("padding", "8px");

            if (newValue != oldValue) {
                var day = currentCell.data("day");
                var type = currentCell.data("type");
                var hour = currentCell.data("hour");

                currentCell.empty().html("<div id=\"facebookG\"><div id=\"blockG_1\" class=\"facebook_blockG\"></div><div id=\"blockG_2\" class=\"facebook_blockG\"></div><div id=\"blockG_3\" class=\"facebook_blockG\"></div></div>");

                var resUrl = window.submitUrl +
                            "?stationcode=" + window.currentStationCode +
                            "&year=" + window.currentYear +
                            "&month=" + window.currentMonth +
                            "&day=" + day +
                            "&type=" + type +
                            "&hour=" + hour +
                            "&newvalue=" + newValue;

                $.ajax({
                    url: resUrl,
                    success: function (data) {
                        if (data == "") {
                            currentCell.empty().text(newValue.toUpperCase());
                        }
                        else {
                            currentCell.empty().text(oldValue);
                            alert(data);
                        }
                    }
                });
            }
            else {
                currentCell.empty().html(oldValue);
            }


        });
    }

    $('td.editable-mediana').dblclick(editEvent);

    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            $('#edit').blur();
        }
    });

})();