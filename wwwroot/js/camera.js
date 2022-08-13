var cameraInterval = 10000;
var width = 320;
var height = 0;
var cameraVideo = document.getElementById('camera');
var cameraCanvas = document.getElementById('canvas');
var photo = document.getElementById('photo');
var streaming = false;
var cameraJob = null;

function checkForCamera() {
    return new Promise((resolve, reject) => {
        navigator.mediaDevices.getUserMedia({
                video: true,
                audio: false
            })
            .then(function(stream) {
                cameraVideo.srcObject = stream;
                cameraVideo.play();
                resolve();
            })
            .catch(function(err) {
                console.log("An error occurred: " + err);
                $("#cameraError").attr("hidden", false);
                reject();
            });

        cameraVideo.addEventListener('canplay',
            function(ev) {
                if (!streaming) {
                    height = cameraVideo.videoHeight / (cameraVideo.videoWidth / width);

                    if (isNaN(height)) {
                        height = width / (4 / 3);
                    }

                    cameraVideo.setAttribute('width', width);
                    cameraVideo.setAttribute('height', height);
                    cameraCanvas.setAttribute('width', width);
                    cameraCanvas.setAttribute('height', height);
                    streaming = true;
                }
            },
            false);
    });
}

function startCameraCapture() {
    cameraJob = setInterval(() => {
        capturePhoto();
    }, cameraInterval);
}

function stopCameraCapture() {
    clearInterval(cameraJob);
}

function capturePhoto() {
    var context = cameraCanvas.getContext('2d');
    cameraCanvas.width = width;
    cameraCanvas.height = height;
    context.drawImage(cameraVideo, 0, 0, width, height);

    var data = canvas.toDataURL('image/png');
    photo.setAttribute('src', data);

    var model = {
        imageData: data.replace('data:image/png;base64,', ''),
        timeStamp: Math.floor(window.player.video_.currentTime)
    };

    $.ajax({
        type: 'POST',
        url: '/api/camera/capture/' + $("#locatorId").val(),
        data: JSON.stringify(model),
        contentType: 'application/json',
        success: function (result) {
            if (result.errorFound) {
                stopCameraCapture();
                window.player.video_.pause();

                if (result.noFaces) {
                    $('#warningModalBody').text('No face was detected. You must be present to continue watching.');
                } else if (result.multipleFaces) {
                    $('#warningModalBody').text('Multiple faces were detected. Only 1 person allowed to continue watching.');
                }
                else if (result.invalidObject) {
                    $('#warningModalBody').text('Recording device detected. Please remove to continue watching.');
                }

                $('#warningModal').modal('show');
            }
        }
    });
}

