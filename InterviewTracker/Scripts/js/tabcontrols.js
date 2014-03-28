//Counters for tabs
var numSchools = 0;
var schoolCounter = 0;
var classCounter = 0;
var numDutyStations = 0;
var dutyStationCounter = 0;
var numWaivers = 0;
var waiverCounter = 0;
var numScreens = 0;
var screenCounter = 0;
var numRDs = 0;
var rdCounter = 0;

$(document).ready(function () {

    // HTML clones for dynamic adding/removing of tabs
    // Remove everything after cloning it
    var schoolTabClone = $("#school-tabs > ul").children().first().clone().removeClass("hide");
    $("#school-tabs > ul").children().first().remove();
    var schoolContentClone = $("#school-content-1").clone().removeClass("hide");
    $("#school-content-1").remove();
    var yearTabClone = $("#year-tab-1").clone();
    $("#year-tab-1").remove();
    var yearContentClone = $("#year-content-1").clone();
    $("#year-content-1").remove();
    var classTabClone = $("#class-tabs > ul").children().first().clone().removeClass("hide");
    $("#class-tabs > ul").children().first().remove();
    var classContentClone = $("#class-content-1").clone().removeClass("hide");
    $("#class-content-1").remove();
    var dutyStationTabClone = $("#duty-station-tabs > ul").children().first().clone().removeClass("hide");
    $("#duty-station-tabs > ul").children().first().remove();
    var dutyStationContentClone = $("#duty-station-content-1").clone().removeClass("hide");
    $("#duty-station-content-1").remove();
    var waiverTabClone = $("#waiver-tabs > ul").children().first().clone().removeClass("hide");
    $("#waiver-tabs > ul").children().first().remove();
    var waiverContentClone = $("#waiver-content-1").clone().removeClass("hide");
    $("#waiver-content-1").remove();
    var screenTabClone = $("#screen-tabs > ul").children().first().clone().removeClass("hide");
    $("#screen-tabs > ul").children().first().remove();
    var screenContentClone = $("#screen-content-1").clone().removeClass("hide");
    $("#screen-content-1").remove();
    var rdTabClone = $("#rd-tabs > ul").children().first().clone().removeClass("hide");
    $("#rd-tabs > ul").children().first().remove();
    var rdContentClone = $("#rd-content-1").clone().removeClass("hide");
    $("#rd-content-1").remove();

    //List of schools that are currently added
    var schoolList = [];

    /** Add functionalities **/

    $("#add-school-button").click(function () {
        var schoolID = $("#SchoolValue").val(); // Get school ID from dropdown
        var schoolName = $("#SchoolValue option[value='" + schoolID + "']").text(); // Get school name from dropdown
        if (schoolList.indexOf(schoolName) != -1) {
            showDialog("School Exists", "This school has already been added", true, function () { });
            return;
        }
        numSchools++;
        schoolCounter++;
        schoolList.push(schoolName);
        // Clone and add content
        var li = $(schoolTabClone).clone();
        $(li).children().first().attr("href", "#school-content-" + schoolCounter);
        // Put school name in link
        $(li).children().first().html(schoolName);
        var content = $(schoolContentClone).clone();
        $(content).find("input").addClass("ui-corner-all");
        $(content).find("textarea").addClass("ui-corner-all");
        $(content).find(".date").attr("id", "")
            .removeClass("hasDatepicker")
            .removeData("datepicker")
            .unbind()
            .datepicker({
                changeYear: true,
                yearRange: "-90:+0"
            });
        $(content).attr("id", "school-content-" + schoolCounter);
        // Put school name in hidden input
        $(content).find("#school-name").val(schoolName);
        // Put school id in #SchoolID
        $(content).find("#SchoolID").val(schoolID);
        $("#school-tabs > ul").prepend(li);
        $("#school-tabs > ul").after(content);
        // Re-init tabs
        $("#school-tabs").tabs("refresh");
    });

    $(document).on("click", ".generate-classes", function (e) {
        // Make sure YearStart and YearEnd are filled
        var YearStart = parseInt($(this).parent().parent().parent().find("#YearStart").val());
        var YearEnd = parseInt($(this).parent().parent().parent().find("#YearEnd").val());
        if (isNaN(YearStart) || isNaN(YearEnd) || (YearStart >= YearEnd)) {
            showDialog("Only Numbers Allowed", "Please enter numbers for Year Start and Year End", true, function () { });
            return;
        }
        var schoolName = $(this).prev().val();
        classCounter++;
        var content;
        // Use clones
        var li = $(classTabClone).clone();
        content = $(classContentClone).clone();
        $(content).find("input").addClass("ui-corner-all");
        $(content).find("textarea").addClass("ui-corner-all");
        // Set tab to school name
        $(li).children().first().attr("href", "#class-content-" + classCounter);
        $(li).children().first().html(schoolName);
        $(content).attr("id", "class-content-" + classCounter);
        $("#class-tabs > ul").prepend(li);
        $("#class-tabs > ul").after(content);
        // Re-init tabs
        $("#class-tabs").tabs("refresh");
        // Clone/create vertical tabs
        var numYears = YearEnd - YearStart;
        for (var i = 0; i < numYears; i++) {
            // Clone year tab and content, append to 'content'
            var yearTab = $(yearTabClone).clone();
            var yearContent = $(yearContentClone).clone();
            // Set year tab html and href
            $(yearTab).find("a").html("Year " + (i + 1));
            $(yearTab).find("a").attr("href", "#year-content-" + (i + 1));
            // Set year content id
            $(yearContent).attr("id", "year-content-" + (i + 1));
            $(yearContent).find("input").addClass("ui-corner-all");
            $(yearContent).find("textarea").addClass("ui-corner-all");
            // Set YearTaken and Year of Record
            $(yearContent).find("#YearTaken").val((i + 1));
            $(yearContent).find("#YearOfRecord").val((i + 1));
            $(content).find("ul").append(yearTab); // Make consecutive downward
            $(content).find("ul").after(yearContent);
        }
        // Init year tabs
        $(content).find(".tabs-vertical").tabs().addClass("ui-tabs-vertical ui-helper-clearfix");
        $(content).find(".tabs-vertical li").removeClass("ui-corner-top").addClass("ui-corner-left");
        //TODO: disable generate button afterwards?
    });

    $(document).on("click", ".add-class", function (e) {
        var table = $(this).parent().parent().find(".classes-table");
        var name = $(this).parent().parent().find("#Name").val();
        var subject = $(this).parent().parent().find("#Subject").val().toUpperCase();
        var code = $(this).parent().parent().find("#Code").val();
        var tech = $(this).parent().parent().find("#Technical").val();
        var grade = $(this).parent().parent().find("#Grade").val();
        if (tech == "" || name == "" || subject == "" || code == "" || grade == "") {
            showDialog("Empty Fields", "Please enter values in all of the fields", true, function () { });
            return;
        }
        var techDisplay = tech == "true" ? "Yes" : "No";
        // Add to table
        $(table).find("tbody").append("" +
        "<tr><form class='classes-form'>" +
        "<td>" + subject + "<input type='hidden' name='Subject' value='" + subject + "'></td>" +
        "<td>" + code + "<input type='hidden' name='Code' value='" + code + "'></td>" +
        "<td>" + name + "<input type='hidden' name='Name' value='" + name + "'></td>" +
        "<td>" + grade + "<input type='hidden' name='Grade' value='" + grade + "'></td>" +
        "<td>" + techDisplay + "<input type='hidden' name='Technical' value='" + tech + "'></td>" +
        "<td class='text-center'><button class='btn-ignore btn-danger remove-class' type='button'>Remove</button></td>" +
        "</form></tr>");
        // Clear out fields
        $(this).parent().parent().find("#Name").val("");
        $(this).parent().parent().find("#Subject").val("");
        $(this).parent().parent().find("#Code").val("");
        $(this).parent().parent().find("#Technical").val("true");
        $(this).parent().parent().find("#Grade").val("");
    });

    $("#add-duty-station-button").click(function () {
        numDutyStations++;
        dutyStationCounter++;
        // Clone and add content
        var li = $(dutyStationTabClone).clone();
        $(li).children().first().attr("href", "#duty-station-content-" + dutyStationCounter);
        var content = $(dutyStationContentClone).clone();
        $(content).find("input").addClass("ui-corner-all");
        $(content).find("textarea").addClass("ui-corner-all");
        $(content).find(".date").attr("id", "")
            .removeClass("hasDatepicker")
            .removeData("datepicker")
            .unbind()
            .datepicker({
                changeYear: true,
                yearRange: "-90:+0"
            });
        $(content).attr("id", "duty-station-content-" + dutyStationCounter);
        $("#duty-station-tabs > ul").prepend(li);
        $("#duty-station-tabs > ul").after(content);
        // Re-init tabs
        $("#duty-station-tabs").tabs("refresh");

    });

    $("#add-waiver-button").click(function () {
        numWaivers++;
        waiverCounter++;
        // Clone and add content
        var li = $(waiverTabClone).clone();
        $(li).children().first().attr("href", "#waiver-content-" + waiverCounter);
        var content = $(waiverContentClone).clone();
        $(content).find("input").addClass("ui-corner-all");
        $(content).find("textarea").addClass("ui-corner-all");
        $(content).find(".date").attr("id", "")
            .removeClass("hasDatepicker")
            .removeData("datepicker")
            .unbind()
            .datepicker({
                changeYear: true,
                yearRange: "-90:+0"
            });
        $(content).attr("id", "waiver-content-" + waiverCounter);
        $("#waiver-tabs > ul").prepend(li);
        $("#waiver-tabs > ul").after(content);
        // Re-init tabs
        $("#waiver-tabs").tabs("refresh");
    });

    $("#add-screen-button").click(function () {
        numScreens++;
        screenCounter++;
        // Clone and add content
        var li = $(screenTabClone).clone();
        $(li).children().first().attr("href", "#screen-content-" + screenCounter);
        var content = $(screenContentClone).clone();
        $(content).find("input").addClass("ui-corner-all");
        $(content).find("textarea").addClass("ui-corner-all");
        $(content).find(".date").attr("id", "")
            .removeClass("hasDatepicker")
            .removeData("datepicker")
            .unbind()
            .datepicker({
                changeYear: true,
                yearRange: "-90:+0"
            });
        $(content).attr("id", "screen-content-" + screenCounter);
        $("#screen-tabs > ul").prepend(li);
        $("#screen-tabs > ul").after(content);
        // Re-init tabs
        $("#screen-tabs").tabs("refresh");
    });

    $("#add-rd-button").click(function () {
        numRDs++;
        rdCounter++;
        // Clone and add content
        var li = $(rdTabClone).clone();
        $(li).children().first().attr("href", "#rd-content-" + rdCounter);
        var content = $(rdContentClone).clone();
        $(content).find("input").addClass("ui-corner-all");
        $(content).find("textarea").addClass("ui-corner-all");
        $(content).find(".date").attr("id", "")
            .removeClass("hasDatepicker")
            .removeData("datepicker")
            .unbind()
            .datepicker({
                changeYear: true,
                yearRange: "-90:+0"
            });
        $(content).attr("id", "rd-content-" + rdCounter);
        $("#rd-tabs > ul").prepend(li);
        $("#rd-tabs > ul").after(content);
        // Re-init tabs
        $("#rd-tabs").tabs("refresh");
    });

    /** Remove Functionalities **/

    $(document).on("click", ".remove-school", function (e) {
        var btn = $(this);
        var f = function () {
            console.log("remove school");
            var schoolName = $(btn).prev().prev().val();
            //Remove school from schoolList
            schoolList.splice(schoolList.indexOf(schoolName), 1);
            var links = $("a:contains(" + schoolName + ")").filter(function () { return $(this).text() == schoolName });
            //Delete link and associated content in links
            //Accounts for class tab
            for (var i = 0; i < links.length; i++) {
                var link = links[i];
                var contentId = $(link).attr("href");
                //Remove link
                $(link).remove();
                //Remove content
                $(contentId).remove();
            }
            numSchools--;
        }
        showDialog("Remove School?", "Are you sure you want to remove this school (and class info) entry?", false, f);
    });

    $(document).on("click", ".remove-class", function (e) {
        var btn = $(this);
        var f = function () {
            console.log("remove class");
            $(btn).parent().parent().remove();
        }
        showDialog("Remove Class?", "Are you sure you want to remove this class entry?", false, f);
    });

    $(document).on("click", ".remove-duty-station", function (e) {
        // Remove duty station tab and content
        var btn = $(this);
        var f = function () {
            console.log("remove duty station");
            var contentDiv = $(btn).parent().parent().parent().parent();
            var num = $(contentDiv).attr("id").slice(-1);
            //Remove content
            $(contentDiv).remove();
            //Remove tab
            $("a[href='#duty-station-content-" + num + "']").parent().remove();
            numDutyStations--;
        }
        showDialog("Remove Duty Station?", "Are you sure you want to remove this duty station entry?", false, f);
    });

    $(document).on("click", ".remove-waiver", function (e) {
        // Remove waiver tab and content
        var btn = $(this);
        var f = function () {
            console.log("remove waiver");
            var contentDiv = $(btn).parent().parent().parent().parent();
            var num = $(contentDiv).attr("id").slice(-1);
            //Remove content
            $(contentDiv).remove();
            //Remove tab
            $("a[href='#waiver-content-" + num + "']").parent().remove();
            numWaivers--;
            console.log(numWaivers);
        }
        showDialog("Remove Waiver?", "Are you sure you want to remove this waiver entry?", false, f);
    });

    $(document).on("click", ".remove-screen", function (e) {
        // Remove screen tab and content
        var btn = $(this);
        var f = function () {
            console.log("remove screen");
            var contentDiv = $(btn).parent().parent().parent().parent();
            var num = $(contentDiv).attr("id").slice(-1);
            //Remove content
            $(contentDiv).remove();
            //Remove tab
            $("a[href='#screen-content-" + num + "']").parent().remove();
            numScreens--;
        };
        showDialog("Remove Screen?", "Are you sure you want to remove this screen entry?", false, f);
    });

    $(document).on("click", ".remove-rd", function (e) {
        // Remove rd tab and content
        var btn = $(this);
        var f = function () {
            console.log("remove rd");
            var contentDiv = $(btn).parent().parent().parent().parent();
            var num = $(contentDiv).attr("id").slice(-1);
            //Remove content
            $(contentDiv).remove();
            //Remove tab
            $("a[href='#rd-content-" + num + "']").parent().remove();
            numRDs--;
        }
        showDialog("Remove RD?", "Are you sure you want to remove this RD entry?", false, f);
    });

});