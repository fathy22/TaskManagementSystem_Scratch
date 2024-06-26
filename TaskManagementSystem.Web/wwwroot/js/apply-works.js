(function ($) {
     
    const uploadButton = document.getElementById('uploadButton');
    if (uploadButton) {
        uploadButton.addEventListener('click', () => {
            const fileInput = document.getElementById('fileInput');
            if (fileInput) {
                fileInput.click();
            }
        });
    }

    const fileInput = document.getElementById('fileInput');
    if (fileInput) {
        fileInput.addEventListener('change', () => {
            const file = fileInput.files[0];
            if (file) {
                uploadAttachment(file);
            }
        });
    }
    function uploadAttachment(file) {

        const formData = new FormData();
        formData.append('file', file);

        $.ajax({
            url: '/api/attachment/upload',
            type: 'POST',
            processData: false,
            contentType: false,
            data: formData,
            success: function (result) {
                debugger;
                document.getElementById('attachmentId').value = result.id;
                document.getElementById('uploadedFileName').textContent = file.name;
                console.log('File uploaded successfully.');
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
})(jQuery);
function toggleDependentTaskDiv() {
  
    var checkbox = document.getElementById('toggleCheckbox');
    var dependentTaskDiv = document.getElementById('dependentTaskDiv');
    if (checkbox.checked) {
        dependentTaskDiv.style.display = 'block';
    } else {
        dependentTaskDiv.style.display = 'none';
    }
}
function toggleDependentTaskDivForEdit() {
  
    var checkbox = document.getElementById('editToggleCheckbox');
    var dependentTaskDiv = document.getElementById('editDependentTaskDiv');
    if (checkbox.checked) {
        dependentTaskDiv.style.display = 'block';
    } else {
        dependentTaskDiv.style.display = 'none';
    }
}
