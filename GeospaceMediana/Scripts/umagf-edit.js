(function () {
    function getCell(day, type) {
        return $('td')
                .filter('[data-day="' + day + '"]')
                .filter('[data-type="' + type + '"]');
    }

    function moveEdit(currentCell, keyCode) {
        var day = currentCell.data("day");
        var type = currentCell.data("type");

        if (type == "ak" && (keyCode == 39 || keyCode == 40)) {
            var newCell = getCell(day, "k1");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }

        if (type == "k1" && keyCode == 39) {
            var newCell = getCell(day, "k2");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }

        if (type == "k2" && keyCode == 39) {
            var newCell = getCell(day, "k3");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }


        if (type == "k3" && keyCode == 39) {
            var newCell = getCell(day, "k4");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }

        if (type == "k4" && keyCode == 39) {
            var newCell = getCell(day, "k5");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }

        if (type == "k5" && keyCode == 39) {
            var newCell = getCell(day, "k6");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }
        

        if (type == "k6" && keyCode == 39) {
            var newCell = getCell(day, "k7");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }
        

        if (type == "k7" && keyCode == 39) {
            var newCell = getCell(day, "k8");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }


        if (type == "k8" && keyCode == 39) {
            var newCell = getCell(day, "events");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }

        if((type == "k1" ||
            type == "k2" ||
            type == "k3" ||
            type == "k4" ||
            type == "k5" ||
            type == "k6" ||
            type == "k7" ||
            type == "k8") && keyCode == 40)
        {
            var newCell = getCell(day, "events");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }


        //
        if (type == "events" && (keyCode == 37 || keyCode == 38)) {
            var newCell = getCell(day, "k8");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }

        if (type == "k8" && keyCode == 37) {
            var newCell = getCell(day, "k7");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }

        if (type == "k7" && keyCode == 37) {
            var newCell = getCell(day, "k6");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }


        if (type == "k6" && keyCode == 37) {
            var newCell = getCell(day, "k5");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }

        if (type == "k5" && keyCode == 37) {
            var newCell = getCell(day, "k4");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }

        if (type == "k4" && keyCode == 37) {
            var newCell = getCell(day, "k3");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }


        if (type == "k3" && keyCode == 37) {
            var newCell = getCell(day, "k2");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }


        if (type == "k2" && keyCode == 37) {
            var newCell = getCell(day, "k1");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }


        if (type == "k1" && keyCode == 37) {
            var newCell = getCell(day, "ak");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }

        if ((type == "k1" ||
            type == "k2" ||
            type == "k3" ||
            type == "k4" ||
            type == "k5" ||
            type == "k6" ||
            type == "k7" ||
            type == "k8") && keyCode == 38) {
            var newCell = getCell(day, "ak");
            $('#edit').blur();
            newCell.dblclick();
            return;
        }

        $('#edit').blur();
    }

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

        var needSlash = oldValue.trim() == "/" || oldValue.trim() == "//";

        oldValue = needSlash ? "" : oldValue;

        var inputHtml = "<input type=\"text\" id=\"edit\" value=\"" + oldValue + "\" />";

        if (type == "events")
            currentCell.css("padding", "0px");

        var cellWidth = currentCell.width();
        var cellHeight = currentCell.height();

        currentCell.empty().append(inputHtml);

        $('#edit').width(cellWidth - 2); // 1+1 border width
        $('#edit').height(cellHeight - 1);
        currentCell.width(cellWidth);

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
                if (needSlash) {
                    oldValue = "/"
                }

                if (type == "ak") {
                    oldValue = "Ak=" + oldValue;
                }
                currentCell.empty().html(oldValue);
            }


        });
    }

    $(document).ready(function () {
        $('td.editable-umagf').dblclick(editEvent);

        $(window).keydown(function (event) {
            if (event.keyCode == 13) {
                $('#edit').blur();
            }
        });

    });
    


})();