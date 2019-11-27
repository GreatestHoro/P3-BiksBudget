function createAlert(locationString) {
    alert(locationString);
};

var x = document.getElementById("demo");

function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else {
        x.innerHTML = "Geolocation is not supported by this browser.";
    }
}

function showPosition(position) {
    document.getElementById("demo").innerHTML = "Latitude: " + position.coords.latitude +
        "<br>Longitude: " + position.coords.longitude;
    alert("hej");
}

async function getCoordinates() {
    if (navigator.geolocation) {
        await navigator.geolocation.getCurrentPosition(UpdateUserLocation);
    } else {
        alert("Sorry, browser does not support geolocation!");
    }
}


async function UpdateUserLocation(position) {
    await DotNet.invokeMethodAsync("FrontEnd2", "UpdateUserLocation", position.coords.latitude, position.coords.longitude);
}

// Remove On Focus on recipe search after click
function onElementFocused() {
    document.activeElement.blur();
}

function makeVisible(strId) {
    document.getElementById(strId).style.display = 'block';
}

function makeInvisible(strId) {
    document.getElementById(strId).style.display = 'none';
}