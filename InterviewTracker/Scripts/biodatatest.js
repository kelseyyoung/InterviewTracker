// Object that holds error response for each component
var passDict = {
    "biodata": {},
    "schools": {},
    "class": {},
    "duty-history": {},
    "duty-stations": {},
    "waivers": {},
    "screens": {},
    "rds": {}
};

/**
 * Function that tests whether a form is valid by using API call
 * Returns: (Boolean) whether or not the form data is valid
 * Params:
 *  forms - (Array of HTML Form) Forms to serialize and pass to the API function
 *  model - (String) Name of the model to use for the API function call
 *  toSet - (String) Name of key to set in passDict
 *  index - (Integer) Index to put toSet's data
 */
function test(forms, model, toSet, index) {
    var formData = "";
    for (var i = 0; i < forms.length; i++) {
        formData += $(forms[i]).serialize();
    }
    var success = function (data) {
        console.log("success");
        console.log(data);
        counter++;
    };
    var error = function (data) {
        console.log(data);
        var response = $.parseJSON(data.responseText).ModelState;
        //delete response.$id;
        var responseString = "";
        $.each(response, function (key, value) {
            for (var i = 0; i < value.length; i++) {
                responseString += value[i] + "<br/>";
            }
        });
        //responseString = responseString.substring(0, responseString.length - 1);
        if (model == "Degree" || model == "ClassesAttended" || model == "SchoolStandings") {
            //We want to append to what is already there in schools
            if (passDict[toSet][index]) {
                passDict[toSet][index] += responseString;
            } else {
                passDict[toSet][index] = responseString;
            }
        } else {
            passDict[toSet][index] = responseString;
        }
        counter++;
    };
    $.ajax({
        url: baseURL + "/api/" + model + "/Test?" + formData,
        type: "Post",
        success: success,
        error: error
    });
}

function testBioData() {
    counter = 0;
    var biodataForm = $("#biodata-form");
    test([biodataForm], "BioData", "biodata", 0);
    waitUntil(function () { return counter == 1; }, testSchools);
}

function testSchools() {
    counter = 0;
    var schoolForms = $(".schools-form");
    //Test SchoolsAttended
    for (var i = 0; i < numSchools; i++) {
        $([schoolForms[i]]).find("#SchoolsAttendedID").removeAttr("name");
        test([schoolForms[i]], "SchoolsAttended", "schools", i);
    }
    waitUntil(function () { return counter == numSchools; }, testDegrees);
}

function testDegrees() {
    counter = 0;
    var schoolForms = $(".schools-form");
    //Test Degree
    for (var i = 0; i < numSchools; i++) {
        // Add SchoolsAttendedID name
        $([schoolForms[i]]).find("#SchoolsAttendedID").attr("name", "SchoolsAttendedID");
        test([schoolForms[i]], "Degree", "schools", i);
    }
    waitUntil(function () { return counter == numSchools; }, testClasses);
}

function testClasses() {
    counter = 0;
    var classForms = $(".class-form");
    var length = $(".classes-form").length;
    console.log(length);
    for (var i = 0; i < classForms.length; i++) {
        // Get classes-forms that only come after classForms[i];
        var classesForms = $(classForms[i]).parent().find(".classes-form");
        for (var j = 0; j < classesForms.length; j++) {
            test([classesForms[j]], "Classes", "class", i);
        }
    }
    /*
    for (var i = 0; i < length; i++) {
        for (var j = 0; j < classForms.length; j++) {
            if ($(classForms[j]).find(classesForms[i]).length > 0) {
                test([classesForms[i]], "Classes", "class", j);
            }
        }
    }
    */
    waitUntil(function () { return counter == length; }, testClassesAttended);
}

function testClassesAttended() {
    counter = 0;
    var classForms = $(".class-form");
    var length = $(".classes-form").length;
    // Figure out how many times for loops will run
    /*
    for (var i = 0; i < classForms.length; i++) {
        //Get all classes for this year
        var classesForms = $(classForms[i]).find(".classes-form");
        length += classesForms.length;
    }
    */
    console.log(length);
    for (var i = 0; i < classForms.length; i++) {
        //Get all classes for this year
        var classesForms = $(classForms[i]).parent().find(".classes-form");
        for (var j = 0; j < classesForms.length; j++) {
            test([classForms[i], classesForms[j]], "ClassesAttended", "class", i);
        }
    }
    waitUntil(function () { return counter == length; }, testSchoolStandings);
}

function testSchoolStandings() {
    counter = 0;
    var classForms = $(".class-form");
    var length = classForms.length;
    for (var i = 0; i < length; i++) {
        test([classForms[i]], "SchoolStandings", "class", i);
    }
    waitUntil(function () { return counter == length; }, testDutyHistory);
}

function testDutyHistory() {
    counter = 0;
    var dutyHistoryForm = $("#duty-history-form");
    test([dutyHistoryForm], "DutyHistory", "duty-history", 0);
    waitUntil(function () { return counter == 1; }, testDutyStations);
}

function testDutyStations() {
    counter = 0;
    var dutyStationForms = $(".duty-stations-form");
    for (var i = 0; i < numDutyStations; i++) {
        test([dutyStationForms[i]], "DutyStation", "duty-stations", i);
    }
    waitUntil(function () { return counter == numDutyStations; }, testWaivers);
}

function testWaivers() {
    counter = 0;
    var waiverForms = $(".waivers-form");
    for (var i = 0; i < numWaivers; i++) {
        test([waiverForms[i]], "Waiver", "waivers", i);
    }
    waitUntil(function () { return counter == numWaivers; }, testScreens);
}

function testScreens() {
    counter = 0;
    var screenForms = $(".screens-form");
    for (var i = 0; i < numScreens; i++) {
        test([screenForms[i]], "Screen", "screens", i);
    }
    waitUntil(function () { return counter == numScreens; }, testRDs);
}

function testRDs() {
    counter = 0;
    var rdForms = $(".rds-form");
    for (var i = 0; i < numRDs; i++) {
        test([rdForms[i]], "RD", "rds", i);
    }
    waitUntil(function () { return counter == numRDs; }, checkErrors);
}