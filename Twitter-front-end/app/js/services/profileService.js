app.factory('profileService', function($http, $q, $resource, BASE_URL){    return function(token){        $http.defaults.headers.common['Authorization'] = 'Bearer ' + token;        $http.defaults.headers.common['Access-Control-Allow-Origin'] = '*';        var profile = {},            resource = $resource(                BASE_URL + 'profile/:option1/:option2',                { option1: '@option1', option2: '@option2' },                {                    edit: {                        method: 'PUT'                    }                }            );        profile.me = function(){            return resource.get();        };        profile.update = function(data, option1){            return resource.edit({option1: option1}, data);        };        profile.getNewsFeed = function(pageSize, startPostId){            var option1 = 'home?StartPostId' + (startPostId ? "=" + startPostId : "") + "&PageSize=" + pageSize;            return resource.query({ option1: option1});        };        return profile;    }});