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

    function setCellText(cell, text)
    {
        if(cell.find("#edit").length == 0)
        {
            cell.text(text);
        }
    }

    function updateCells(day, hour)
    {
        var cell_f0F1 = getCell(day, hour, "f0F1");
        var cell_M3000F1 = getCell(day, hour, "M3000F1");

        var cell_f0F2 = getCell(day, hour, "f0F2");
        var cell_M3000F2 = getCell(day, hour, "M3000F2");

        var cell_f0Es = getCell(day, hour, "f0Es");

        var cell_D = getCell(day, hour, "D");

        if ((cell_f0F2.text().trim() == "G" && cell_M3000F2.text().trim() == "G") ||
            (cell_f0F2.text().trim() == "C" && cell_M3000F2.text().trim() == "C"))
        {
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
            setCellText(cell_f0F1, "");
            setCellText(cell_M3000F1, "");
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
            setCellText(cell_f0Es, "00");
        }

        if (cell_f0F2.text().trim() != "" && cell_M3000F2.text().trim() != "")
        {
            if (cell_f0F2.text().trim() == "F" && cell_M3000F2.text().trim() == "F") {
                setCellText(cell_D, "3");
            }
            else if (cell_f0F2.text().trim() == "C" && cell_M3000F2.text().trim() == "C") {
                setCellText(cell_D, "C");
            }
            else if (cell_f0F2.text().trim() == "B" && cell_M3000F2.text().trim() == "B") {
                setCellText(cell_D, "B");
            }
            else if (cell_D.text() != "1" && cell_D.text() != "2" && cell_D.text() != "3") {
                setCellText(cell_D, "0");
            }
        }
        else
        {
            setCellText(cell_D, "");
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

    var lineHeaders = ["f0F2", "M3000F2", "f0F1", "M3000F1", "f0Es", "D", "fmin"];

    var numberTypeDictionary = {};

    for(var i=0;i<7;i++)
    {
        numberTypeDictionary[lineHeaders[i]] = i;
    }

    function moveEdit(currentCell, keyCode)
    {
        var day = currentCell.data("day");
        var type = currentCell.data("type");
        var hour = currentCell.data("hour");

        var numberType = numberTypeDictionary[type];

        switch (keyCode) {
            case 37: // left
                numberType--;
                break;
            case 38: // up
                hour--;
                break;
            case 39: // right
                numberType++;
                break;
            case 40: // down
                hour++;
                break;
        }

        if(numberType < 0)
        {
            numberType = 6;
            day--;
        }

        if(numberType > 6)
        {
            numberType = 0;
            day++;
        }

        type = lineHeaders[numberType];

        var newCell = getCell(day, hour, type);

        if (newCell.length == 0)
            return;

        if (newCell.hasClass("editable-ionka") == false)
        {
            moveEdit(newCell, keyCode);
            return;
        }

        $('#edit').blur();
        newCell.dblclick();
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

        $('#edit').keydown(function (event) {
            if (event.keyCode >= 37 && event.keyCode <= 40)
            {
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

    $('td.editable-ionka').dblclick(editEvent);

    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            $('#edit').blur();
        }
    });

})();