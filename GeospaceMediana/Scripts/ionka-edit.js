(function() { 
    function getCell(day, hour, type)
    {
        return $('td')
                .filter('[data-day="' + day + '"]')
                .filter('[data-hour="' + hour + '"]')
                .filter('[data-type="' + type + '"]');
    }

    function getCells(day, hour)
    {
        return $('td')
                .filter('[data-day="' + day + '"]')
                .filter('[data-hour="' + hour + '"]');
    }

    function updateCells(day, hour)
    {
        var cell_f0F1 = getCell(day, hour, "f0F1");
        var cell_M3000F1 = getCell(day, hour, "M3000F1");

        var cell_f0F2 = getCell(day, hour, "f0F2");
        var cell_M3000F2 = getCell(day, hour, "M3000F2");

        var cell_f0Es = getCell(day, hour, "f0Es");

        var cell_D = getCell(day, hour, "D");

        if (cell_f0F2.text().trim() == "G" && cell_M3000F2.text().trim() == "G")
        {
            cell_f0F1.text("0");
            cell_M3000F1.text("0");
            if (cell_f0F1.hasClass("editable-ionka") == false)
            {
                cell_f0F1.addClass("editable-ionka");
                cell_M3000F1.addClass("editable-ionka");

                cell_f0F1.dblclick(editEvent);
                cell_M3000F1.dblclick(editEvent);
            }
        
        }
        else
        {
            cell_f0F1.text("");
            cell_M3000F1.text("");
            if (cell_f0F1.hasClass("editable-ionka"))
            {
                cell_f0F1.removeClass("editable-ionka");
                cell_M3000F1.removeClass("editable-ionka");

                cell_f0F1.unbind('dblclick');
                cell_M3000F1.unbind('dblclick');
            }
        }

        if (cell_f0Es.text().trim() == "0")
        {
            cell_f0Es.text("00");
        }

        if (cell_f0F2.text().trim() == "F" && cell_M3000F2.text().trim() == "F")
        {
            cell_D.text("3");
        }
        else if(cell_f0F2.text().trim() == "C" && cell_M3000F2.text().trim() == "C")
        {
            cell_D.text("C");
        }
        else
        {
            cell_D.text("0");
        }
    }

    function fillCells(day, hour)
    {
        var cells = getCells(day, hour);

        cells.removeClass("add-ionka");
        cells.addClass("editable-ionka");

        cells.text("0");

        cells.unbind("dblclick");
        cells.dblclick(editEvent);
    }

    function editEvent(e)
    {
        var currentCell = $(this);

        if (this.children["edit"] != null)
            return;

        var oldValue = $(currentCell).text().trim();

        var inputHtml = "<input type=\"text\" id=\"edit\" value=\"" + oldValue + "\" />";

        var cellWidth = currentCell.width();
        var cellHeight = currentCell.height();

        currentCell.empty().append(inputHtml);

        $('#edit').width(cellWidth - 2); // 1+1 border width
        $('#edit').height(cellHeight - 2);
        currentCell.width(cellWidth);
        currentCell.height(cellHeight);

        $('#edit').focus();
        $('#edit').select();
        $('#edit').blur(function () {

            var newValue = $(this).val().trim();
            currentCell.width("");
            currentCell.height("");

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
                            updateCells(day, hour);
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

    function addEvent(e) {
        var currentCell = $(this);

        if (this.children["edit"] != null)
            return;

        var inputHtml = "<input type=\"text\" id=\"edit\" />";

        currentCell.css("padding", "0px");

        var cellWidth = currentCell.width();
        var cellHeight = currentCell.height();

        currentCell.empty().append(inputHtml);

        $('#edit').width(cellWidth - 2); // 1+1 border width
        $('#edit').height(cellHeight - 2);
        currentCell.width(cellWidth);
        currentCell.height(cellHeight);

        $('#edit').focus();
        $('#edit').blur(function () {

            var newValue = $(this).val().trim();
            currentCell.css("padding", "8px");
            currentCell.width("");
            currentCell.height("");

            if(newValue != "")
            {
                var day = currentCell.data("day");
                var type = currentCell.data("type");
                var hour = currentCell.data("hour");

                currentCell.empty().html("<div id=\"facebookG\"><div id=\"blockG_1\" class=\"facebook_blockG\"></div><div id=\"blockG_2\" class=\"facebook_blockG\"></div><div id=\"blockG_3\" class=\"facebook_blockG\"></div></div>");

                var resUrl = window.addUrl +
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
                            fillCells(day, hour);
                            currentCell.empty().text(newValue.toUpperCase());
                            updateCells(day, hour);

                        }
                        else {
                            currentCell.empty().html("");
                            alert(data);
                        }
                    }
                });
            }
            else {
                currentCell.empty().html("");
            }
            
        });
    }

    $('td.editable-ionka').dblclick(editEvent);
    $('td.add-ionka').dblclick(addEvent);

    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            $('#edit').blur();
        }
    });

})();