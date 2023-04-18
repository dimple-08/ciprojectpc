function shareStory(userId,value) {

    var dataUrls = FILE_LIST.map(file => file.url);
    
    console.log(dataUrls);
    var missionId = document.getElementById("exampleFormControlSelect1").value;
    var title = document.getElementById("storytitle").value;
    var date = document.getElementById("releasedate").value;
    
    const regex = /^https?:\/\/(?:www\.)?(?:youtube\.com\/(?:watch\?.*?v=|embed\/)|youtu\.be\/|vimeo\.com\/)([a-zA-Z0-9_-]{11}|[0-9]{8,10})/;;
    var videoUrls = $("#video-urls").val().split('\n');
    let isValid = true;
    var uid = userId;
    var previewbtn = document.querySelector('.previewbtn');
    console.log(uid);
    console.log(missionId);
    console.log(title);
    console.log(date);
    console.log(videoUrls);
    var editor = CKEDITOR.instances.editor1;
    var editordata = editor.getData();

    if (title.trim() && missionId != "Select Mission" && date.trim() && videoUrls.length > 1 && videoUrls[0] != "" && dataUrls.length > 0) {
        for (let i = 0; i < videoUrls.length; i++) {
            if (!regex.test(videoUrls[i])) {
                swal.fire("Please enter a valid YouTube or Vimeo video URL.");
                isValid = false;
                break;
            }
        }

        if (!isValid) {

            event.preventDefault();
        }
        else {

            if (title.trim() && missionId != "Select Mission" && date.trim() && videoUrls.length != 0 && videoUrls[0] != "" && dataUrls.length > 0) {
                $.ajax({
                    url: '/Home/Share_Story',
                    type: 'POST',
                    data: { "Image": dataUrls, "MissionId": missionId, "Title": title, "Date": date, "Description": editordata, "UserId": userId, "videoUrls": videoUrls, "Value": value },
                    success: function (result) {
                        if (result.success) {
                            alert("Your Story Is Added");
                            previewbtn.classList.remove("d-none");
                            previewbtn.id = result.storyid;
                            const url = `/Home/StoryDetailPage?Storyid=${result.storyid}`;

                            // Set the "href" attribute of the "prw" element to the constructed URL
                            previewbtn.href = url;

                        }
                        else {
                            Swal.fire("You alreay share the story");


                        }
                    }
                });
            }
            else {
                Swal.fire("You have to fill every fields..");
            }
        }
    }
    else {
        Swal.fire("You have to fill every fields..");
    }
    

       
   
	
}
//CKEDITOR.replace('editor1', {
//	fullPage: true,
//	extraPlugins: 'docprops',
//	// Disable content filtering because if you use full page mode, you probably
//	// want to  freely enter any HTML content in source mode without any limitations.
	
//	height: 320,
//	removeButtons: 'PasteFromWord'
//});

function shareStorySubmit(userId) {

    var dataUrls = FILE_LIST.map(file => file.url);

    console.log(dataUrls);
    var missionId = document.getElementById("exampleFormControlSelect1").value;
    var title = document.getElementById("storytitle").value;
    var date = document.getElementById("releasedate").value;
    const value = $("#video-urls").val();
    const regex = /^https?:\/\/(?:www\.)?(?:youtube\.com\/(?:watch\?.*?v=|embed\/)|youtu\.be\/|vimeo\.com\/)([a-zA-Z0-9_-]{11}|[0-9]{8,10})/;;
    var videoUrls = $("#video-urls").val().split('\n');
    let isValid = true;
    var uid = userId;
    var previewbtn = document.querySelector('.previewbtn');
    console.log(uid);
    console.log(missionId);
    console.log(title);
    console.log(date);
    var editor = CKEDITOR.instances.editor1;
    var editordata = editor.getData();

    if (title.trim() && missionId != "Select Mission" && date.trim() && videoUrls.length != 0 && videoUrls[0] != "" && dataUrls.length > 0) {
        swal.fire("You have to fill every feilds");
    }
    for (let i = 0; i < videoUrls.length; i++) {
        if (!regex.test(videoUrls[i])) {
            swal.fire("Please enter a valid YouTube or Vimeo video URL.");
            isValid = false;
            break;
        }
    }

    if (!isValid) {

        event.preventDefault();
    }
    else {

        if (title.trim() && missionId != "Select Mission" && date.trim() && videoUrls.length != 0 && videoUrls[0] != "" && dataUrls.length > 0) {
            $.ajax({
                url: '/Home/Share_StorySubmit',
                type: 'POST',
                data: { "Image": dataUrls, "MissionId": missionId, "Title": title, "Date": date, "Description": editordata, "UserId": userId, "videoUrls": videoUrls },
                success: function (result) {
                    if (result.success) {
                        alert("Your Story Is Added");
                        previewbtn.classList.remove("d-none");
                        previewbtn.id = result.storyid;
                        const url = `/Home/StoryDetailPage?Storyid=${result.storyid}`;

                        // Set the "href" attribute of the "prw" element to the constructed URL
                        previewbtn.href = url;

                    }
                    else {
                        Swal.fire("You alreay share the story");

                    }
                }
            });
        }
        else {
            Swal.fire("You have to fill every fields..");
        }
    }




}
CKEDITOR.replace('editor1');
const INPUT_FILE = document.querySelector("#upload-files");
const INPUT_CONTAINER = document.querySelector("#upload-container");
const FILES_LIST_CONTAINER = document.querySelector("#files-list-container");
const FILE_LIST = [];
const MAX_FILE_SIZE = 4 * 1024 * 1024; // 4MB in bytes
const MAX_NUM_FILES = 20;

let UPLOADED_FILES = [];

const multipleEvents = (element, eventNames, listener) => {
    const events = eventNames.split(" ");

    events.forEach((event) => {
        element.addEventListener(event, listener, false);

    });
};

const previewImages = () => {
    FILES_LIST_CONTAINER.innerHTML = "";
    console.log(FILE_LIST);
    if (FILE_LIST.length >= 0)
    {
       FILE_LIST.forEach((addedFile, index) => {
            const content = `
            <div class="form__image-container js-remove-image" data-index="${index}">
              <img class="form__image" src="${addedFile.url}" alt="${addedFile.name}">
            </div>
          `;
           console.log(index, addedFile.url, addedFile.name);
            FILES_LIST_CONTAINER.insertAdjacentHTML("beforeEnd", content);
        });
    } else {
        console.log("empty");
        INPUT_FILE.value = "";
    }
};

const fileUpload = () => {
    if (FILES_LIST_CONTAINER) {
        multipleEvents(INPUT_FILE, "click dragstart dragover", () => {
            INPUT_CONTAINER.classList.add("active");
        });

        multipleEvents(INPUT_FILE, "dragleave dragend drop change blur", () => {
            INPUT_CONTAINER.classList.remove("active");
        });

        INPUT_FILE.addEventListener("change", () => {
            const files = [...INPUT_FILE.files];
            console.log("changed");
            files.forEach((file) => {

                const fileName = file.name;
                const fileSize = file.size;

                

                const fileURL = URL.createObjectURL(file);
                console.log(fileURL);
                
                if (!file.type.match("image/")) {
                    alert(file.name + " is not an image");
                    console.log(file.type);
                } else {
                    const reader = new FileReader();
                    reader.onload = (event) => {
                        const fileURL = event.target.result;
                        console.log(fileURL);
                        const uploadedFiles = {
                            name: fileName,
                            url: fileURL
                        };
                        // Check if file already uploaded
                        const isDuplicate = FILE_LIST.some(
                            (uploadedFile) =>
                                uploadedFile.name === fileName || uploadedFile.url === fileURL
                        );
                        if (isDuplicate) {
                            Swal.fire(`File "${fileName}" already uploaded.`);
                            return;
                        }

                        // Check if max number of files reached
                        if (FILE_LIST.length >= MAX_NUM_FILES) {
                            Swal.fire(`Maximum ${MAX_NUM_FILES} files can be uploaded.`);
                            return;
                        }

                        // Check file size
                        if (fileSize > MAX_FILE_SIZE) {
                            Swal.fire(`File "${fileName}" is too large. Maximum file size is 4MB.`);
                            return;
                        }
                        FILE_LIST.push(uploadedFiles);
                        previewImages();
                    };
                    reader.readAsDataURL(file);
                 

                }
            });

            console.log(FILE_LIST); //final list of uploaded files
            UPLOADED_FILES = document.querySelectorAll(".js-remove-image");
            removeFile();
        });
    }
};

const removeFile = () => {
    UPLOADED_FILES = document.querySelectorAll(".js-remove-image");

    if (UPLOADED_FILES) {
        UPLOADED_FILES.forEach((image) => {
            image.addEventListener("click", function () {
                const fileIndex = this.getAttribute("data-index");

                FILE_LIST.splice(fileIndex, 1);
                previewImages();
                removeFile();
            });
        });
    } else {
        [...INPUT_FILE.files] = [];
    }
};

fileUpload();
removeFile();

function searchStoryByMission() {
    var missionId = document.getElementById("exampleFormControlSelect1").value;
    while (FILE_LIST.length > 0) {
        FILE_LIST.pop();
    }
    $.ajax({
        url: '/Home/StoryPreview',
        type: 'POST',
        data: { MissionId: missionId },
        dataType: "json",
        success: function (result) {
            alert();
            if (result.success==true) {
                
                var story = result.storypreview;
                console.log(result.storypreview);
                $('#exampleFormControlSelect1').prop('disabled', true);
                $('#storytitle').val(story.title);
                var date = new Date(story.publichedAt);
              
                console.log(date);
              
                var formattedDate = date.getFullYear() + "-" + (date.getMonth() + 1).toString().padStart(2, '0') + "-" + date.getDate().toString().padStart(2, '0');
                $('#releasedate').val(formattedDate);
                CKEDITOR.instances['editor1'].setData(story.description
                );
              
                var media = result.pathList;
                console.log(media);
                var videoUrlsList = result.videoPath;
                console.log(result.videoPath);
                const videoUrlsString = videoUrlsList.join("\n");
                console.log(videoUrlsString);
                $("#video-urls").val(videoUrlsString);
                $('.form__image-container .js-remove-image').empty();
                media.forEach(function (media) {
                    const uploadedFiles = {
                        name: Date.now(),
                        url: media
                    };
                    console.log(uploadedFiles);
                    FILE_LIST.push(uploadedFiles);
                    previewImages();
                    removeFile();
                });
            }
            else if (result.success == "notadded") {


            }

            else {
                alert("For this mission your story is in pending");
            }
        },
        error: function () {
            alert("could not load your draft");
        }
    });
}