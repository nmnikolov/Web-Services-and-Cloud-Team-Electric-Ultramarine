app.factory('userService', function($http, $q, $resource, BASE_URL, authentication){
    return function(token){
        $http.defaults.headers.common['Authorization'] = 'Bearer ' + token;
        $http.defaults.headers.common['Content-Type'] = 'applications/json';

        var user = {},
            resource = $resource(
                BASE_URL + 'users/:option1/:option2/:option3',
                {
                    option1: '@option1',
                    option2: '@option2',
                    option3: '@option3'
                },
                {
                    edit: {
                        method: 'PUT'
                    },
                    login:{
                        method: 'POST',
                        headers: {'Content-Type': 'application/x-www-form-urlencoded'},
                        transformRequest: function(obj) {
                            var str = [];
                            for (var p in obj)
                                str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                            return str.join("&");
                        }
                    }
                }
            );

        user.changePassword = function(data){
            return resource.save({option1: 'profile', option2: 'changepassword'}, data);
        };

        user.login = function(loginData){
            loginData['Grant_type'] = 'password';
            return resource.login({option1: 'profile', option2: 'login'}, loginData);
        };

        user.register = function(registerData){
            return resource.save({option1: 'profile', option2: 'register'}, registerData);
        };

        user.logout = function(){
            return resource.save({option1: 'profile', option2: 'logout'});
        };

        user.getUserWall = function(username, pageSize, startPostId){
            var option2 = 'wall?StartPostId' + (startPostId ? "=" + startPostId : "") + "&PageSize=" + pageSize;

            return resource.query({ option1: username, option2: option2});
        };

        user.getUserTweets = function(username, pageSize, startPostId){
            var option2 = 'tweets?StartPostId' + (startPostId ? "=" + startPostId : "") + "&PageSize=" + pageSize;

            return resource.query({ option1: username, option2: option2});
        };

        user.getFollowingUsers = function(username, pageSize, startUserId){
            var option2 = 'following?StartUserId' + (startUserId ? "=" + startUserId : "") + "&PageSize=" + pageSize;

            return resource.get({ option1: username, option2: option2});
        };

        user.getFollowedUsers = function(username, pageSize, startUserId){
            var option2 = 'followed?StartUserId' + (startUserId ? "=" + startUserId : "") + "&PageSize=" + pageSize;

            return resource.get({ option1: username, option2: option2});
        };

        user.getUserFavoriteTweets = function(username, pageSize, startPostId){
            var option2 = 'favorite?StartPostId' + (startPostId ? "=" + startPostId : "") + "&PageSize=" + pageSize;

            return resource.query({ option1: username, option2: option2});
        };

        user.searchUser = function(searchTerm){
            var option1 = "search?search=" + searchTerm;

            return resource.query({ option1: option1 });
        };

        user.sendFollowRequest = function(username){
            return resource.save({ option1: username, option2: 'follow'});
        };

        user.sendUnfollowRequest = function(username){
            return resource.save({ option1: username, option2: 'unfollow'});
        };

        user.getUserFullData = function(username){
            return resource.get({ option1: username });
        };

        user.getUserPreviewData = function(username){
            return resource.get({ option1: username, option2: 'preview' });
        };

        //user.getUserFriendsPreview = function(username){
        //    return resource.get({ option1: username, option2: 'friends', option3: 'preview' });
        //};
        //
        //user.getUserFriends = function(username){
        //    return resource.query({ option1: username, option2: 'friends' });
        //};

        user.isLogged = function(){
            return authentication.isLogged();
        };

        return user;
    }
});