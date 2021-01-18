function onDragStart(event) {
    event
        .dataTransfer
        .setData('text/plain', event.target.id);
}
function onDrop(event) {
    const id = event
        .dataTransfer
        .getData('text');

    const draggableElement = document.getElementById(id);
    const dropzone = event.target;

    var columnData = dropzone.id;
    var columnID = columnData.split("")[1];

    var cardData = draggableElement.id;
    var cardID = cardData.split("_")[1];


    $.ajax({
        type: "GET",
        url: "Task/KartDurumGuncelle/" + cardID + "?stateID=" + columnID,
        success: function () {
            console.log("İşlem başarılı !");
        }
    });

    dropzone.appendChild(draggableElement);
    event
        .dataTransfer
        .clearData();
}
function onDragOver(event) {
    event.preventDefault();
}