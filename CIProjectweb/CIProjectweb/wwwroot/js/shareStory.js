function shareStory(userId) {
    alert("share");
    var dataUrls = FILE_LIST.map(file => file.url);
    alert(dataUrls);
    console.log(dataUrls);
    var missionId = document.getElementById("exampleFormControlSelect1").value;
	var title = document.getElementById("storytitle").value;
    var date = document.getElementById("releasedate").value;
    var videoUrls = $("#video-urls").val().split('\n');
	var uid = userId;
	console.log(uid);
	var editor = CKEDITOR.instances.editor1;
	var editordata = editor.getData();
	console.log(editor.getData());
	console.log(missionId);
	console.log(title);
	console.log(date);
	$.ajax({
		url: '/Home/Share_Story',
		type: 'POST',
        data: { "Image": dataUrls, "MissionId": missionId, "Title": title, "Date": date, "Description": editordata, "UserId": userId, "videoUrls": videoUrls },
		success: function (result) {
			if (result.success) {
				alert("Your Story Is Added");
				location.reload();
			}
			else {
				alert("You alreay share the story");
				location.reload();
			}
		}
	});
}
//CKEDITOR.replace('editor1', {
//	fullPage: true,
//	extraPlugins: 'docprops',
//	// Disable content filtering because if you use full page mode, you probably
//	// want to  freely enter any HTML content in source mode without any limitations.
	
//	height: 320,
//	removeButtons: 'PasteFromWord'
//});

CKEDITOR.replace('editor1');
const INPUT_FILE = document.querySelector("#upload-files");
const INPUT_CONTAINER = document.querySelector("#upload-container");
const FILES_LIST_CONTAINER = document.querySelector("#files-list-container");
const FILE_LIST = [];
let UPLOADED_FILES = [];

const multipleEvents = (element, eventNames, listener) => {
    const events = eventNames.split(" ");

    events.forEach((event) => {
        element.addEventListener(event, listener, false);

    });
};

const previewImages = () => {
    FILES_LIST_CONTAINER.innerHTML = "";
    if (FILE_LIST.length > 0) {
        FILE_LIST.forEach((addedFile, index) => {
            const content = `
            <div class="form__image-container js-remove-image" data-index="${index}">
              <img class="form__image" src="${addedFile.url}" alt="${addedFile.name}">
            </div>
          `;

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
                const fileURL = URL.createObjectURL(file);
                const fileName = file.name;
                if (!file.type.match("image/")) {
                    alert(file.name + " is not an image");
                    console.log(file.type);
                } else {
                    const uploadedFiles = {
                        name: fileName,
                        url: fileURL
                    };

                    FILE_LIST.push(uploadedFiles);
                }
            });

            console.log(FILE_LIST); //final list of uploaded files
            previewImages();
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