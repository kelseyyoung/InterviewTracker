
// Will hold BioDataID for this candidate for submittal
var bioDataId = 0;

// Holds SchoolsAttendedID for classes submittal
var schoolDict = {};

// Start submittal chain
function submit() {
    counter = 0;
    submitBioData();
}

function submitBioData() {
    var formData = $("#biodata-form").serialize();
    // Determine if Post or Put
    var type = "Post";
    var append = "";
    if ($("#biodata-form").find("#ID").length > 0 && $("#biodata-form").find("#ID").val() != "") {
        type = "Put";
        append = "/" + $("#biodata-form").find("#ID").val();
    }
    console.log(type);
    console.log(append);
    $.ajax({
        url: baseURL + "/api/BioData/" + type + append + "?" + formData,
        type: type,
        success: function (data) {
            console.log(data);
            // Set bioDataId
            bioDataId = data.ID;
            // Post to SetPrograms
            $.ajax({
                url: baseURL + "/api/BioData/SetPrograms/" + bioDataId + "?" + formData,
                type: "Post",
                success: function (data) {
                    counter++;
                },
                error: function (data) {
                    // Shouldn't get here
                    console.log("error");
                    console.log(data);
                }
            });
        },
        error: function (data) {
            // Shouldn't get here
            console.log("error");
            console.log(data);
        }
    });
    waitUntil(function () { return counter == 1; }, submitSchoolsStart);
}

function submitSchoolsStart() {
    counter = 0;
    for (var i = 0; i < numSchools; i++) {
        submitSchools(i);
    }
    waitUntil(function () { return counter == numSchools; }, submitSchoolStandingsStart);
}

function submitSchools(i) {
    var schoolForms = $(".schools-form");
    var form = schoolForms[i];
    // Set SchoolValue to submit School
    $(form).find("#school-name").attr("name", "SchoolValue");
    $.ajax({
        url: baseURL + "/api/School/GetOrCreate?" + $(form).serialize(),
        type: "Post",
        success: function (data) {
            // Create SchoolsAttended
            // Put in BioDataID and SchoolID
            $(form).find("#BioDataID").val(bioDataId);
            $(form).find("#SchoolID").val(data.SchoolID);
            var schoolName = data.SchoolValue;
            // Determine if Post or Put
            var type = "Post";
            var append = "";
            if ($(form).find("#SchoolsAttendedIDReal").length > 0 && $(form).find("#SchoolsAttendedIDReal").val() != "") {
                type = "Put";
                append = "/" + $(form).find("#SchoolsAttendedIDReal").val();
            } else {
                // Eliminate SchoolsAttendedID
                $(form).find("#SchoolsAttendedID").removeAttr("name");
            }
            console.log(type);
            console.log(append);
            $.ajax({
                url: baseURL + "/api/SchoolsAttended/" + type + append + "?" + $(form).serialize(),
                type: type,
                success: function (data) {
                    // Set SchoolsAttendedID
                    var schoolsAttendedID = data.SchoolsAttendedID;
                    // Put schoolsAttendedID in schoolDict
                    schoolDict[schoolName] = schoolsAttendedID;
                    // Put ID in the form
                    $(form).find("#SchoolsAttendedID").attr("name", "SchoolsAttendedID");
                    $(form).find("#SchoolsAttendedID").val(schoolsAttendedID);
                    // Find or create major
                    $.ajax({
                        url: baseURL + "/api/Major/GetOrCreate?" + $(form).serialize(),
                        type: "Post",
                        success: function (data) {
                            // Set MajorID
                            $(form).find("#MajorID").val(data.MajorID);
                            // Determine if Post or Put
                            var type = "Post";
                            var append = "";
                            if ($(form).find("#DegreeID").length > 0 && $(form).find("#DegreeID").val() != "") {
                                type = "Put";
                                append = "/" + $(form).find("#DegreeID").val();
                            }
                            console.log(type);
                            console.log(append);
                            // Create degree
                            $.ajax({
                                url: baseURL + "/api/Degree/" + type + append + "?" + $(form).serialize(),
                                type: type,
                                success: function (data) {
                                    counter++;
                                },
                                error: function (data) {
                                    console.log("error Post Degree");
                                    console.log(data);
                                }
                            })
                        },
                        error: function (data) {
                            console.log("error GetOrCreate Major");
                            console.log(data);
                        }
                    });
                },
                error: function (data) {
                    //Shouldn't get here
                    console.log("error Post SchoolsAttended");
                    console.log(data);
                }
            });
        },
        error: function (data) {
            // Shouldn't get here
            console.log("error GetOrCreate School");
            console.log(data);
        }
    });
}

function submitSchoolStandingsStart() {
    counter = 0;
    var length = 0;
    var classForms = $(".class-form");
    for (var i = 0; i < classForms.length; i++) {
        //Get all classes for this year
        var classesForms = $(classForms[i]).parent().find(".classes-form");
        length += classesForms.length;
    }
    // Submit SchoolStandings
    for (var j = 0; j < classForms.length; j++) {
        submitSchoolStandings(j);
    }
    waitUntil(function () { return counter == length; }, submitDutyHistory);
}

function submitSchoolStandings(i) {
    var classForms = $(".class-form");
    var classForm = classForms[i];
    // Create school standings
    // Fill in SchoolsAttendedID
    // Get school name
    var schoolName = $(classForm).find("#school-name").val();
    $(classForm).find("#SchoolsAttendedID").val(schoolDict[schoolName]);
    // Fill in BioDataID
    $(classForm).find("#BioDataID").val(bioDataId);
    // Determine if Post or Put
    var type = "Post";
    var append = "";
    if ($(classForm).find("#SchoolStandingsID").length > 0 && $(classForm).find("#SchoolStandingsID").val() != "") {
        type = "Put";
        append = "/" + $(classForm).find("#SchoolStandingsID").val();
    }
    console.log(type);
    console.log(append);
    $.ajax({
        url: baseURL + "/api/SchoolStandings/" + type + append + "?" + $(classForm).serialize(),
        type: type,
        success: function (data) {
        },
        error: function (data) {
            console.log("error Post SchoolStandings");
            console.log(data);
        }
    });
    // Find classes forms within classForm
    var classesForms = $(classForm).parent().find(".classes-form");
    // For each class, get or create classes
    for (var j = 0; j < classesForms.length; j++) {
        submitClasses(j, classForm)
    }
}

function submitClasses(i, classForm) {
    var classesForms = $(classForm).parent().find(".classes-form");
    var classesForm = classesForms[i];
    $.ajax({
        url: baseURL + "/api/Classes/GetOrCreate?" + $(classesForm).serialize(),
        type: "Post",
        success: function (data) {
            // On success, create classes attended
            var classID = data.ClassesID;
            // Set ClassesID
            $(classForm).find("#ClassesID").val(classID);
            // Determine if Post or Put
            var type = "Post";
            var append = "";
            if ($(classesForm).find("#ClassesAttendedID").length > 0 && $(classesForm).find("#ClassesAttendedID").val() != "") {
                type = "Put";
                append = "/" + $(classesForm).find("#ClassesAttendedID").val();
            }
            console.log(type);
            console.log(append);
            $.ajax({
                url: baseURL + "/api/ClassesAttended/" + type + append + "?" + $(classForm).serialize() + $(classesForm).serialize(),
                type: type,
                success: function (data) {
                    counter++;
                },
                error: function (data) {
                    console.log("error Post ClassesAttended");
                    console.log(data);
                }
            });
        },
        error: function (data) {
            console.log("error GetOrCreate Classes");
            console.log(data);
        }
    });
}

function submitDutyHistory() {
    counter = 0;
    var dutyHistoryForm = $("#duty-history-form");
    $(dutyHistoryForm).find("#BioDataID").val(bioDataId);
    // Determine if Post or Put
    var type = "Post";
    var append = "";
    if ($(dutyHistoryForm).find("#DutyHistoryID").length > 0 && $(dutyHistoryForm).find("#DutyHistoryID").val() != "") {
        type = "Put";
        append = "/" + $(dutyHistoryForm).find("#DutyHistoryID").val();
    }
    console.log(type);
    console.log(append);
    $.ajax({
        url: baseURL + "/api/DutyHistory/" + type + append + "?" + $(dutyHistoryForm).serialize(),
        type: type,
        success: function (data) {
            var dutyHistoryID = data.DutyHistoryID;
            // Submit duty stations
            var dutyStationForms = $(".duty-stations-form");
            for (var i = 0; i < dutyStationForms.length; i++) {
                submitDutyStations(i, dutyHistoryID);
            }
        },
        error: function (data) {
            // Shouldn't get here
            console.log("error");
            console.log(data);
        }
    })
    waitUntil(function () { return counter == numDutyStations; }, submitWaiversStart);
}

function submitDutyStations(i, dutyHistoryID) {
    var dutyStationForms = $(".duty-stations-form");
    var form = dutyStationForms[i];
    $(form).find("#DutyHistoryID").val(dutyHistoryID);
    // Determine if Post or Put
    var type = "Post";
    var append = "";
    if ($(form).find("#DutyStationID").length > 0 && $(form).find("#DutyStationID").val() != "") {
        type = "Put";
        append = "/" + $(form).find("#DutyStationID").val();
    }
    console.log(type);
    console.log(append);
    $.ajax({
        url: baseURL + "/api/DutyStation/" + type + append + "?" + $(form).serialize(),
        type: type,
        success: function (data) {
            counter++;
        },
        error: function (data) {
            // Shouldn't get here
            console.log("error");
            console.log(data);
        }
    });
}

function submitWaiversStart() {
    counter = 0;
    var waiverForms = $(".waivers-form");
    for (var i = 0; i < numWaivers; i++) {
        submitWaivers(i);
    }
    waitUntil(function () { return counter == numWaivers; }, submitScreensStart);
}

function submitWaivers(i) {
    var waiverForms = $(".waivers-form");
    var form = waiverForms[i];
    $(form).find("#BioDataID").val(bioDataId); // Set BioDataID
    // Determine if Post or Put
    var type = "Post";
    var append = "";
    if ($(form).find("#WaiverID").length > 0 && $(form).find("#WaiverID").val() != "") {
        type = "Put";
        append = "/" + $(form).find("#WaiverID").val();
    }
    console.log(type);
    console.log(append);
    $.ajax({
        url: baseURL + "/api/Waiver/" + type + append +"?" + $(form).serialize(),
        type: type,
        success: function (data) {
            counter++;
        },
        error: function (data) {
            // Shouldn't get here
            console.log("error");
            console.log(data);
        }
    });
}

function submitScreensStart() {
    counter = 0;
    var screenForms = $(".screens-form");
    for (var i = 0; i < numScreens; i++) {
        submitScreens(i);
    }
    waitUntil(function () { return counter == numScreens; }, submitRDsStart);
}

function submitScreens(i) {
    var screenForms = $(".screens-form");
    var form = screenForms[i];
    $(form).find("#BioDataID").val(bioDataId); // Set BioDataID
    var type = "Post";
    var append = "";
    if ($(form).find("#ScreenID").length > 0 && $(form).find("#ScreenID").val() != "") {
        type = "Put";
        append = "/" + $(form).find("#ScreenID").val();
    }
    console.log(type);
    console.log(append);
    $.ajax({
        url: baseURL + "/api/Screen/" + type + append + "?" + $(form).serialize(),
        type: type,
        success: function (data) {
            console.log(data.ScreenID);
            // Set programs applied for
            $.ajax({
                url: baseURL + "/api/Screen/SetPrograms/" + data.ScreenID + "?" + $(form).serialize(),
                type: "Post",
                success: function (data) {
                    counter++;
                },
                error: function (data) {
                    // Shouldn't get here
                    console.log("error");
                    console.log(data);
                }
            });
        },
        error: function (data) {
            // Shouldn't get here
            console.log("error");
            console.log(data);
        }
    });
}

function submitRDsStart() {
    counter = 0;
    var rdForms = $(".rds-form");
    for (var i = 0; i < numRDs; i++) {
        submitRDs(i);
    }
    waitUntil(function () { return counter == numRDs; }, endSubmit);
}

function submitRDs(i) {
    var rdForms = $(".rds-form");
    var form = rdForms[i];
    $(form).find("#BioDataID").val(bioDataId); // Set BioDataID
    var type = "Post";
    var append = "";
    if ($(form).find("#RDID").length > 0 && $(form).find("#RDID").val() != "") {
        type = "Put";
        append = "/" + $(form).find("#RDID").val();
    }
    console.log(type);
    console.log(append);
    $.ajax({
        url: baseURL + "/api/RD/" + type + append + "?" + $(form).serialize(),
        type: type,
        success: function (data) {
            counter++;
        },
        error: function (data) {
            // Shouldn't get here
            console.log("error");
            console.log(data);
        }
    });
}

function endSubmit() {
    console.log("endSubmit");
    $("#submitting-dialog").dialog("close");
    // Clear schoolDict (not needed probably)
    schoolDict = {};
    // Go to Candidate/Success page
    window.location.href = baseURL + "/Candidates/Success";
}
