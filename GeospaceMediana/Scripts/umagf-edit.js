(function () {
    
    function editEvent(e) {
        var currentCell = $(this);
        var id = currentCell.data("id");
        var type = currentCell.data("type");
        var day = currentCell.data("day");

        if (this.children["edit"] != null)
            return;

        var oldValue = $(currentCell).text().trim();

        if (type == "ak")
        {
            oldValue = oldValue.split("=")[1];
        }

        oldValue = oldValue.trim() == "/" || oldValue.trim() == "//" ? "" : oldValue;

        var inputHtml = "<input type=\"text\" id=\"edit\" value=\"" + oldValue + "\" />";

        if (type == "events")
            currentCell.css("padding", "0px");

        var cellWidth = currentCell.width();
        var cellHeight = currentCell.height();

        currentCell.empty().append(inputHtml);

        $('#edit').width(cellWidth - 2); // 1+1 border width
        $('#edit').height(cellHeight - 1);
        currentCell.width(cellWidth);

        $('#edit').focus();
        $('#edit').select();
        $('#edit').blur(function () {

            var newValue = $(this).val().trim();
            if (type == "events")
                currentCell.css("padding", "8px");
            currentCell.width("");

            if (newValue != oldValue) {
                
                if (oldValue == "" && type != "ak")
                {
                    oldValue = "/"
                }

                currentCell.empty().html("<div id=\"facebookG\"><div id=\"blockG_1\" class=\"facebook_blockG\"></div><div id=\"blockG_2\" class=\"facebook_blockG\"></div><div id=\"blockG_3\" class=\"facebook_blockG\"></div></div>");

                var resUrl = window.submitUmagfUrl +
                            "?stationcode=" + window.currentStationCode +
                            "&year=" + window.currentYear +
                            "&month=" + window.currentMonth +
                            "&day=" + day +
                            "&type=" + type +
                            "&newvalue=" + newValue;

                $.ajax({
                    url: resUrl,
                    success: function (data) {

                        var resValue;

                        if (data == "") {
                            resValue = newValue;
                        }
                        else
                        {
                            resValue = oldValue;
                            alert(data);
                        }

                        if(type == "ak")
                        {
                            if (resValue.length == 1) {
                                resValue = "0" + resValue;
                            }

                            resValue = "Ak=" + resValue;
                        }

                        currentCell.empty().text(resValue);

                    }
                });
            }
            else {
                if (type == "ak") {
                    oldValue = "Ak=" + oldValue;
                } else {
                    if (oldValue == "") {
                        oldValue = "/"
                    }
                }
                currentCell.empty().html(oldValue);
            }


        });
    }

    $('td.editable-umagf').dblclick(editEvent);

    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            $('#edit').blur();
        }
    });

})();