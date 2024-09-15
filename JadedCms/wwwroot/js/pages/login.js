document.addEventListener('DOMContentLoaded', function () {
    var form = document.getElementById('loginForm');
    form.addEventListener('submit', function (event) {
        event.preventDefault();
        var formData = new FormData(form);
        var data = {};
        formData.forEach(function (value, key) {
            data[key] = value;
        });
        console.log(data);
    });
});