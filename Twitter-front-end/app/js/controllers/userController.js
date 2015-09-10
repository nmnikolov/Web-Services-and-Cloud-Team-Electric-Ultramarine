app.controller('userController', function userController($scope, $location, $routeParams, userService, authentication, profileService, notifyService, PAGE_SIZE, FOLLOWING_SIZE, $timeout, usSpinnerService) {
    var feedStartPostId;
    var startUserId;
    $scope.posts = [];
    $scope.friendsList = [];
    $scope.busy = false;
    $scope.totalUsers;

    $scope.login = function(){
        if(!authentication.isLogged()){
            usSpinnerService.spin('spinner-1');
            userService().login($scope.loginData).$promise.then(
                function(data){
                    usSpinnerService.stop('spinner-1');
                    authentication.setCredentials(data);
                    notifyService.showInfo("Welcome, " + data.userName + "!");
                    $location.path('/');
                },
                function(error){
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Login failed!", error);
                }
            );
        }
    };

    $scope.register = function(){
        if(!authentication.isLogged()){
            usSpinnerService.spin('spinner-1');
            userService().register($scope.registerData).$promise.then(
                function(){
                    usSpinnerService.stop('spinner-1');
                    notifyService.showInfo("Registration successful!");
                    $location.path('/login');
                },
                function(error){
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Registration failed!", error);
                }
            );
        }
    };

    $scope.logout = function(){
        if(authentication.isLogged()){
            usSpinnerService.spin('spinner-1');
            userService(authentication.getAccessToken()).logout().$promise.then(
                function(){
                    usSpinnerService.stop('spinner-1');
                    notifyService.showInfo("Good bye, " + $scope.username + '!');
                    authentication.clearCredentials();
                    $location.path('/');
                },
                function(error){
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Logout failed!", error);
                }
            );
        }
    };

    $scope.loadUserWall = function(){
        if(authentication.isLogged()) {
            if ($scope.busy){
                return;
            }
            $scope.busy = true;
            usSpinnerService.spin('spinner-1');
            userService(authentication.getAccessToken()).getUserWall($routeParams['username'], PAGE_SIZE, feedStartPostId).$promise.then(
                function (data) {
                    $scope.posts = $scope.posts.concat(data);
                    if($scope.posts.length > 0){
                        feedStartPostId = $scope.posts[$scope.posts.length - 1].id;
                    }

                    $scope.busy = false;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to load user wall!", error);
                }
            );
        }
    };

    $scope.loadUserTweets = function(){
        if(authentication.isLogged()) {
            if ($scope.busy){
                return;
            }
            $scope.busy = true;
            usSpinnerService.spin('spinner-1');
            userService(authentication.getAccessToken()).getUserTweets($routeParams['username'], PAGE_SIZE, feedStartPostId).$promise.then(
                function (data) {
                    $scope.posts = $scope.posts.concat(data);
                    if($scope.posts.length > 0){
                        feedStartPostId = $scope.posts[$scope.posts.length - 1].id;
                    }

                    $scope.busy = false;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to load user tweets!", error);
                }
            );
        }
    };

    $scope.loadUserFavoriteTweets = function(){
        if(authentication.isLogged()) {
            if ($scope.busy){
                return;
            }
            $scope.busy = true;
            usSpinnerService.spin('spinner-1');
            userService(authentication.getAccessToken()).getUserFavoriteTweets($routeParams['username'], PAGE_SIZE, feedStartPostId).$promise.then(
                function (data) {
                    $scope.posts = $scope.posts.concat(data);
                    if($scope.posts.length > 0){
                        feedStartPostId = $scope.posts[$scope.posts.length - 1].id;
                    }

                    $scope.busy = false;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to load user tweets!", error);
                }
            );
        }
    };

    $scope.searchUser = function(){
        if(authentication.isLogged() && $scope.searchTerm.trim() !== ""){
            userService(authentication.getAccessToken()).searchUser($scope.searchTerm).$promise.then(
                function(data){
                    $scope.searchResults = data;
                }
            );
        } else {
            $scope.searchResults = undefined;
        }
    };

    $scope.getWallOwner = function(){
        if (authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            userService(authentication.getAccessToken()).getUserFullData($routeParams['username']).$promise.then(
                function(data){
                    $scope.wallOwner = data;
                    usSpinnerService.stop('spinner-1');
                },
                function(error){
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to load user data!", error);
                }
            );
        }
    };

    $scope.clearSearchResults = function(){
        $timeout(function() {
            $scope.searchResults = undefined;
            $scope.searchTerm = "";
        }, 300);
    };

    $scope.showUserPreview = function(username){
        $scope.previewData = {};
        if (authentication.isLogged()){
            usSpinnerService.spin('spinner-1');

            userService(authentication.getAccessToken()).getUserPreviewData(username).$promise.then(
                function(data){
                    $scope.previewData = {
                        image: data.profileImageData ? data.profileImageData : $scope.defaultImage,
                        fullname: data.fullname,
                        username: data.username,
                        isFollowing: data.isFollowing
                    };

                    usSpinnerService.stop('spinner-1');
                },
                function(error){
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to load user preview!", error);
                }
            );
        }
    };

    $scope.getUserFriendsListPreview = function(){
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            userService(authentication.getAccessToken()).getUserFriendsPreview($routeParams['username']).$promise.then(
                function (data) {
                    data.userFriendsUrl = '#/user/' + $routeParams['username'] + '/friends/';
                    $scope.friendsListPreview = data;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to load user friends!", error);
                }
            );
        }
    };

    $scope.getFollowingUsers = function(){
        if(authentication.isLogged()) {
            if ($scope.busy){
                return;
            }
            $scope.busy = true;
            usSpinnerService.spin('spinner-1');
            userService(authentication.getAccessToken()).getFollowingUsers($routeParams['username'], FOLLOWING_SIZE, startUserId).$promise.then(
                function (data) {
                    $scope.friendsList = $scope.friendsList.concat(data.users);
                    $scope.totalUsers = data.totalUsers;
                    if($scope.friendsList.length > 0){
                        startUserId = $scope.friendsList[$scope.friendsList.length - 1].id;
                    }
                    $scope.busy = false;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to load following users!", error);
                }
            );
        }
    };

    $scope.getFollowedUsers = function(){
        if(authentication.isLogged()) {
            if ($scope.busy){
                return;
            }
            $scope.busy = true;
            usSpinnerService.spin('spinner-1');
            userService(authentication.getAccessToken()).getFollowedUsers($routeParams['username'], FOLLOWING_SIZE, startUserId).$promise.then(
                function (data) {
                    $scope.friendsList = $scope.friendsList.concat(data.users);
                    $scope.totalUsers = data.totalUsers;
                    if($scope.friendsList.length > 0){
                        startUserId = $scope.friendsList[$scope.friendsList.length - 1].id;
                    }
                    $scope.busy = false;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to load followed users!", error);
                }
            );
        }
    };
});