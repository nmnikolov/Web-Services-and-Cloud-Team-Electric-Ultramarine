app.factory('authentication', function(){
    var authentication = {};

    authentication.isLogged = function(){
        return localStorage['accessToken'] !== undefined;
    };

    authentication.setCredentials = function (data) {
        localStorage.setItem('accessToken', data['access_token']);
        localStorage.setItem('username', data['userName']);
        localStorage.setItem('name', data['name']);
    };

    authentication.clearCredentials = function () {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('username');
        localStorage.removeItem('name');
    };

    authentication.getAccessToken = function(){
        return localStorage.getItem('accessToken');
    };

    authentication.getUsername = function(){
        return localStorage.getItem('username');
    };

    authentication.getName = function(){
        return localStorage.getItem('name');
    };

    return authentication;
});