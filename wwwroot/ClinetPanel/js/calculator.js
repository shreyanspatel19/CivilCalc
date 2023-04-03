function meterTofeet() {
    var dropdown = document.getElementById("ddlUnit");
    var selectedValue = dropdown.value;
    if (selectedValue == "1") {
        document.getElementById("lengthToMF").innerHTML = "meter";
        document.getElementById("lengthToCI").innerHTML = "cm";
        document.getElementById("heightToMF").innerHTML = "meter";
        document.getElementById("heightToCI").innerHTML = "cm";
        document.getElementById("depthToMF").innerHTML = "meter";
        document.getElementById("depthToCI").innerHTML = "cm";
        $("#filterForm").submit();
    } else {
        document.getElementById("lengthToMF").innerHTML = "feet";
        document.getElementById("lengthToCI").innerHTML = "inch";
        document.getElementById("heightToMF").innerHTML = "feet";
        document.getElementById("heightToCI").innerHTML = "inch";
        document.getElementById("depthToMF").innerHTML = "feet";
        document.getElementById("depthToCI").innerHTML = "inch";
        $("#filterForm").submit();
    }
}