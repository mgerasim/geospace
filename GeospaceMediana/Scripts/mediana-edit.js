(function () {

    function getCell(day, hour, type) {
        return $('td')
                .filter('[data-day="' + day + '"]')
                .filter('[data-hour="' + hour + '"]')
                .filter('[data-type="' + type + '"]');
    }

    function moveEdit(currentCell, keyCode) {
        var day = currentCell.data("day");
        var type = currentCell.data("type");
        var hour = currentCell.data("hour");

        switch (keyCode) {
            case 37: // left
                hour--;
                break;
            case 38: // up
                day--;
                break;
            case 39: // right
                hour++;
                break;
            case 40: // down
                day++;
                break;
        }

        var newCell = getCell(day, hour, type);

        if (newCell.length == 0)
            return;

        if (newCell.hasClass("editable-mediana") == false) {
            moveEdit(newCell, keyCode);
            return;
        }

        $('#edit').blur();
        newCell.dblclick();
    }


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

        $('#edit').keydown(function (event) {
            if (event.keyCode >= 37 && event.keyCode <= 40) {
                event.preventDefault();
                moveEdit(currentCell, event.keyCode);
            }
        });

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