function validateForm()
{
    var x = document.forms["myForm"]["Name"].value;
    var xx = document.forms["myForm"]["Email"].value;
    var xxx = document.forms["myForm"]["Password"].value;

        if (x == null || x == "") {
            $(".label-warning").text("Du måste fylla i alla fält!");
            return false;
        }
        if (xx == null || xx == "") {
            $(".label-warning").text("Du måste fylla i alla fält!");
            return false;
        }
        if (xxx == null || xxx == "") {
            $(".label-warning").text("Du måste fylla i alla fält!");
            return false;
        }
return true;
}

