/* File Created: February 20, 2012 */
var chat;

$(function () {
    // Created proxy
    chat = $.connection.traceHub;

    // Assign a function to be called by the server
    chat.addMessage = onAddMessage;

    // Register a function with the button click
    $("#broadcast").click(onBroadcast);

    // Start the connection
    $.connection.hub.start();
});

function onAddMessage(message) {
    // Add the message to the list
    $('#messages').append('<li>' + message + '</li>');
}

function onBroadcast() {
    // Call the chat method on the server
    var message = $('#message').val();
    var traceEvent = jQuery.parseJSON('{"Message":"' + message + '"}');
    chat.send(traceEvent);
}