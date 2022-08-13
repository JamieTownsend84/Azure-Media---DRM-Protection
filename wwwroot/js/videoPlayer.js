$(document).ready(function () {
    initVideoPlayer();
});

function initVideoPlayer() {
    shaka.polyfill.installAll();

    var video = document.getElementById('videoPlayer');
    var player = new shaka.Player(video);
    window.player = player;

    player.getNetworkingEngine().registerRequestFilter(function (type, request) {
        if (type === shaka.net.NetworkingEngine.RequestType.LICENSE) {
            request.headers['Authorization'] = 'Bearer ' + $("#token").val();;
        }
    });

    var manifestUrl = $("#manifestUrl").val();
    player.load(manifestUrl);
}