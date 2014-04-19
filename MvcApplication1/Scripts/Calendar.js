
function Save() {
    // Spara värderna från input fälten
    var start = $('#StartTime').val();
    var end = $('#EndTime').val();
    var date = $('#Events_Date').val();
    var head = $('#Header').val();
    var desc = $('#Description').val();
    
    // Ajax-anrop till lägg till event
    $.ajax({
        url: '/Calendar/AddEvent',
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ StartTime: start, EndTime: end, Date: date, Header: head, Description: desc }) // Gör om detta till en JSON sträng
    }).success(function () {
        // Ladda om sidan
        window.location.reload();
    });
}
