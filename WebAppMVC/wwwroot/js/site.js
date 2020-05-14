// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function hoverInUser() {
    document.getElementById("myDropdown").classList.toggle("show");
}

function hoverOutUser() {
    var dropdowns = document.getElementsByClassName("dropdown-content");
    var i;
    for (i = 0; i < dropdowns.length; i++) {
        var openDropdown = dropdowns[i];
        if (openDropdown.classList.contains('show')) {
            openDropdown.classList.remove('show');
        }
    }
}

function hideBlock(checkboxId, elemForHideId) {
	checkbox = document.getElementById(checkboxId);
	elem = document.getElementById(elemForHideId);
	if (checkbox.checked){
		elem.hidden = true;
		elem.value = "#";
	}
	else{
		elem.hidden = false;
		elem.value = "";
	}
}

function changeAttendance(elem, _subjectId, _studentId, _date){
	let data = {
		subjectId : _subjectId,
		studentId : _studentId,
		date : _date	
	};
	if (elem.checked){
		$.post("/Teacher/JournalAdd", data, null, "json");
	}
	else{
		$.post("/Teacher/JournalRemove", data, null, "json");
	}
}

function addToTimetable(_techerId, _recordId) {
	let inputDay = document.getElementById("sel-day-of-week");
	let inputNumber = document.getElementById("inp-number-lesson");
	let data = {
		teacherId: _techerId,
		recordId: _recordId,
		dayOfWeek: inputDay.value,
		numberLesson: inputNumber.value
	};
	$.post("/Admin/AddRecordToTimeTable", data, null, "json");
}