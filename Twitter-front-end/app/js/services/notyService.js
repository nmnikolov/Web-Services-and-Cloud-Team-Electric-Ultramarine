app.factory('notifyService', function () {
        return {
            showInfo: function(msg) {
                noty({
                        text: msg,
                        type: 'success',
                        layout: 'bottomLeft',
                        timeout: 2000}
                );
            },
            showError: function(msg, serverError) {
                var errors = [];
                if (serverError && serverError.data.error_description) {
                    errors.push(serverError.data.error_description);
                }
                if (serverError && serverError.data.message) {
                    errors.push(serverError.data.message);
                }
                if (serverError && serverError.data.modelState) {
                    var modelStateErrors = serverError.data.modelState;
                    for (var propertyName in modelStateErrors) {
                        var errorMessages = modelStateErrors[propertyName];
                        var trimmedName = propertyName.substr(propertyName.indexOf('.') + 1);
                        for (var i = 0; i < errorMessages.length; i++) {
                            var currentError = errorMessages[i];
                            errors.push(trimmedName + ' - ' + currentError);
                        }
                    }
                }
                if (errors.length > 0) {
                    msg = msg + "<br>" + errors.join("<br>");
                }

                noty({
                        text: msg,
                        type: 'error',
                        layout: 'bottomLeft',
                        timeout: 5000}
                );
            }
        }
    }
);