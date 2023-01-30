function IsValidPinCode(Control) {
    if (Control.value.length > 0 && Control.value.length != 6) {
        alert("Invalid Pincode");
        Control.focus();
        return false;
    }
    else
        return true;
}

function IsValidLength(Control, Length, ValueType) {

    if (Control.value.length > 0 && Control.value.length != Length) {
        alert("Enter " + Length.toString() + " " + ValueType);
        Control.focus();
        return false;
    }
    else
        return true;
}

function IsAlphabetDigit(key) {

    var specialKeys = new Array();
    specialKeys.push(8); //Backspace

    //getting key code of pressed key
    //32-Space
    //37 LEft Arrow

    var keyCode = (key.which) ? key.which : key.keyCode;

    if (keyCode == 8 || keyCode == 37)
        return true;

    if ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1) {
        return true;
    }
    else {
        return false;
    }
}

function IsAlphabetDigitSlash(key) {

    var specialKeys = new Array();
    specialKeys.push(8); //Backspace

    //getting key code of pressed key
    //32-Space
    //37 LEft Arrow
    //47 Slash (/)

    var keyCode = (key.which) ? key.which : key.keyCode
    if (keyCode == 8 || keyCode == 37 || keyCode == 47)
        return true;

    if ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1) {
        return true;
    }
    else {
        return false;
    }
}

/* Validations Final */
function IsPositiveInteger(event) {

    // Backspace, tab, enter, end, home, left, right in number part, 
    // We don't support the del key in Opera because del == . == 46.
    //[BAckspace, tab, enter, end,home,left,right]
    var controlKeys = [8, 9, 13];
    // IE doesn't support indexOf
    var isControlKey = controlKeys.join(",").match(new RegExp(event.which));
    // Some browsers just don't raise events for control keys. Easy.
    // e.g. Safari backspace.
    if (!event.which || // Control keys in most browsers. e.g. Firefox tab is 0
        (48 <= event.which && event.which <= 57) || // Always 1 through 9
        isControlKey) { // Opera assigns values for control keys.
        return;
    } else {
        event.preventDefault();
    }
}

function IsNegativeInteger(Control, event) {
    //45 for Dash
    if (event.which == 45 && Control.value.length == 0)
        return;
    // Backspace, tab, enter, end, home, left, right in number part, 
    // We don't support the del key in Opera because del == . == 46.
    //[BAckspace, tab, enter, end,home,left,right]
    var controlKeys = [8, 9, 13];
    // IE doesn't support indexOf
    var isControlKey = controlKeys.join(",").match(new RegExp(event.which));
    // Some browsers just don't raise events for control keys. Easy.
    // e.g. Safari backspace.
    if (!event.which || // Control keys in most browsers. e.g. Firefox tab is 0
        (48 <= event.which && event.which <= 57) || // Always 1 through 9
        isControlKey) { // Opera assigns values for control keys.
        return;
    } else {
        event.preventDefault();
    }
}

function IsAlphabetName(event) {

    // Backspace, tab, enter, end, home, left, right in number part, 
    // We don't support the del key in Opera because del == . == 46.
    //45 -Dash,  39-Coma
    var controlKeys = [8, 9, 13, 110, 46, 32, 45, 39];
    // IE doesn't support indexOf
    var isControlKey = controlKeys.join(",").match(new RegExp(event.which));
    // Some browsers just don't raise events for control keys. Easy.
    // e.g. Safari backspace.
    if (!event.which || // Control keys in most browsers. e.g. Firefox tab is 0
        (65 <= event.which && event.which <= 90) || // Always a through b
        (97 <= event.which && event.which <= 122) || // Always A through B
        isControlKey) { // Opera assigns values for control keys.
        return;
    } else {
        event.preventDefault();
    }
}

function IsPhoneCSV(event) {

    // Backspace, tab, enter, end, home, left, right,comma,dash in number part, 
    // We don't support the del key in Opera because del == . == 46.
    var controlKeys = [8, 9, 13, 44, 45];
    // IE doesn't support indexOf
    var isControlKey = controlKeys.join(",").match(new RegExp(event.which));
    // Some browsers just don't raise events for control keys. Easy.
    // e.g. Safari backspace.
    if (!event.which || // Control keys in most browsers. e.g. Firefox tab is 0
        (48 <= event.which && event.which <= 57) || // Always 1 through 9
        isControlKey) { // Opera assigns values for control keys.
        return;
    } else {
        event.preventDefault();
    }
}

function IsDecimal2DigitPositive(control, event) {
    // Backspace, tab, enter, end, home, left, right in number part, 
    // We don't support the del key in Opera because del == . == 46.

    var controlKeys = [8, 9, 13];
    // IE doesn't support indexOf
    var isControlKey = controlKeys.join(",").match(new RegExp(event.which));

    //alert(window.getSelection());

    // Some browsers just don't raise events for control keys. Easy.
    // e.g. Safari backspace.
    if (!event.which || // Control keys in most browsers. e.g. Firefox tab is 0
        (48 <= event.which && event.which <= 57) || // Always 1 through 9
        ((46 == event.which || 190 == event.which) && control.value.toString().length > 0 && control.value.toString().lastIndexOf('.') === -1) || //Allow Dot Only once
         //(control.value.toString().indexOf('.')!=-1 && control.value.toString().substring(control.value.toString().indexOf('.'),control.value.toString().length).length <=2) ||
        isControlKey) { // Opera assigns values for control keys.

        if ((control.value.toString().indexOf('.') !== -1 &&
            (control.value.toString().length - control.value.toString().indexOf('.')) > 2) && window.getSelection().length === 0)
            return false;
        else
            return;
    } else {
        event.preventDefault();
    }
}

function IsDecimal2DigitNegative(control, event) {
    // Backspace, tab, enter, end, home, left, right in number part, 
    // We don't support the del key in Opera because del == . == 46.
    if (event.which == 45 && control.value.length == 0)
        return;

    if (control.value.toString().indexOf('.') != -1 && (control.value.toString().length - control.value.toString().indexOf('.')) > 2)
        return false;

    var controlKeys = [8, 9, 13];
    // IE doesn't support indexOf
    var isControlKey = controlKeys.join(",").match(new RegExp(event.which));
    // Some browsers just don't raise events for control keys. Easy.
    // e.g. Safari backspace.
    if (!event.which || // Control keys in most browsers. e.g. Firefox tab is 0
        (48 <= event.which && event.which <= 57) || // Always 1 through 9
        (46 == event.which && control.value.toString().length > 0 && control.value.toString().lastIndexOf('.') == -1) || //Allow Dot Only once
        // (control.value.toString().indexOf('.')!=-1 && control.value.toString().substring(control.value.toString().indexOf('.'),control.value.toString().length).length <=2) ||
        isControlKey) { // Opera assigns values for control keys.
        return;
    } else {
        event.preventDefault();
    }
}

function IsAlphaNumeric(event) {

    // Backspace, tab, enter, end, home, left, right in number part, 
    // We don't support the del key in Opera because del == . == 46.
    //45 -Dash,  39-Coma
    var controlKeys = [8, 9, 13, 110];
    // IE doesn't support indexOf
    var isControlKey = controlKeys.join(",").match(new RegExp(event.which));
    // Some browsers just don't raise events for control keys. Easy.
    // e.g. Safari backspace.
    if (!event.which || // Control keys in most browsers. e.g. Firefox tab is 0
        (65 <= event.which && event.which <= 90) || // Always a through b
        (97 <= event.which && event.which <= 122) || // Always A through B
        (48 <= event.which && event.which <= 57) || // Always 1 through 9
        isControlKey) { // Opera assigns values for control keys.
        return;
    } else {
        event.preventDefault();
    }
}

function IsAlphaNumericSpace(event) {
    // Backspace, tab, enter, end, home, left, right in number part, 
    // We don't support the del key in Opera because del == . == 46.
    //45 -Dash,  39-Coma
    var controlKeys = [8, 9, 13, 110, 32];
    // IE doesn't support indexOf
    var isControlKey = controlKeys.join(",").match(new RegExp(event.which));
    // Some browsers just don't raise events for control keys. Easy.
    // e.g. Safari backspace.
    if (!event.which || // Control keys in most browsers. e.g. Firefox tab is 0
        (65 <= event.which && event.which <= 90) || // Always a through b
        (97 <= event.which && event.which <= 122) || // Always A through B
        (48 <= event.which && event.which <= 57) || // Always 1 through 9
        isControlKey) { // Opera assigns values for control keys.
        return;
    } else {
        event.preventDefault();
    }
}

function IsAlphaNumericSpaceDashComma(event) {

    // Backspace, tab, enter, end, home, left, right in number part, 
    // We don't support the del key in Opera because del == . == 46.
    //45 -Dash,  39-Coma
    var controlKeys = [8, 9, 13, 110, 32, 45, 44];
    // IE doesn't support indexOf
    var isControlKey = controlKeys.join(",").match(new RegExp(event.which));
    // Some browsers just don't raise events for control keys. Easy.
    // e.g. Safari backspace.
    if (!event.which || // Control keys in most browsers. e.g. Firefox tab is 0
        (65 <= event.which && event.which <= 90) || // Always a through b
        (97 <= event.which && event.which <= 122) || // Always A through B
        (48 <= event.which && event.which <= 57) || // Always 1 through 9
        isControlKey) { // Opera assigns values for control keys.
        return;
    } else {
        event.preventDefault();
    }
}

function IsAlphaNumericSlash(event) {
    // Backspace, tab, enter, end, home, left, right in number part, 
    // We don't support the del key in Opera because del == . == 46.
    //45 -Dash,  39-Coma
    var controlKeys = [8, 9, 13, 37, 110, 32, 45, 47];
    // IE doesn't support indexOf
    var isControlKey = controlKeys.join(",").match(new RegExp(event.which));

    // Some browsers just don't raise events for control keys. Easy.
    // e.g. Safari backspace.
    if (!event.which || // Control keys in most browsers. e.g. Firefox tab is 0
        (65 <= event.which && event.which <= 90) || // Always a through b
        (97 <= event.which && event.which <= 122) || // Always A through B
        (48 <= event.which && event.which <= 57) || // Always 1 through 9
        isControlKey) { // Opera assigns values for control keys.
        return;
    } else {
        event.preventDefault();
    }
}














/* oRIGINAL
$(".numeric").keypress(function(event) {
  // Backspace, tab, enter, end, home, left, right,decimal(.)in number part, decimal(.) in alphabet
  // We don't support the del key in Opera because del == . == 46.
  var controlKeys = [8, 9, 13, 35, 36, 37, 39,110,190];
  // IE doesn't support indexOf
  var isControlKey = controlKeys.join(",").match(new RegExp(event.which));
  // Some browsers just don't raise events for control keys. Easy.
  // e.g. Safari backspace.
  if (!event.which || // Control keys in most browsers. e.g. Firefox tab is 0
      (49 <= event.which && event.which <= 57) || // Always 1 through 9
      (96 <= event.which && event.which <= 106) || // Always 1 through 9 from number section 
      (48 == event.which && $(this).attr("value")) || // No 0 first digit
      (96 == event.which && $(this).attr("value")) || // No 0 first digit from number section
      isControlKey) { // Opera assigns values for control keys.
    return;
  } else {
    event.preventDefault();
  }
});

*/