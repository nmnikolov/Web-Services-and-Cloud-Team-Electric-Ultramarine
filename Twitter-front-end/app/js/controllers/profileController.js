app.controller('profileController', function ($scope, $location, userService, profileService, authentication, notifyService, PAGE_SIZE, usSpinnerService) {
    var feedStartPostId;
    $scope.posts = [];
    $scope.busy = false;

    $scope.getUserDetails = function(){
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            profileService(authentication.getAccessToken()).me().$promise.then(
                function (data) {
                    $scope.me = data;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to load user details!", error)
                }
            );
        }
    };

    $scope.editDetails = function(){
        usSpinnerService.spin('spinner-1');
        if(authentication.isLogged()) {
            profileService(authentication.getAccessToken()).update($scope.me, 'edit').$promise.then(
                function () {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showInfo('Profile successfully edited.');
                },
                function (error) {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError('Failed to edit profile!', error);
                }
            );
        }
    };

    $scope.editPassword = function(){
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            userService(authentication.getAccessToken()).changePassword($scope.passwordUpdate).$promise.then(
                function () {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showInfo('Password successfully changed.');
                    $location.path('/');
                },
                function (error) {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError('Failed to change password!', error);
                }
            );
        }
    };

    $scope.uploadImage = function(event){
        var previewElement,
            inputElement,
            file,
            reader,
            sizeLimit;

        switch(event.target.id){
            case 'profile-image':
                previewElement = $('.picture-preview');
                inputElement = $('#profile-image');
                sizeLimit = 131072;
                break;
            case 'cover-image':
                previewElement = $('.cover-preview');
                inputElement = $('#cover-image');
                sizeLimit = 1048576;
                break;
        }

        file = event.target.files[0];

        if(file && !file.type.match(/image\/.*/)){
            notifyService.showError("Invalid file format.");
        } else if(file && file.size > sizeLimit) {
            notifyService.showError("File size limit exceeded.");
        } else if(file) {
            reader = new FileReader();
            reader.onload = function() {
                previewElement.attr('src', reader.result);
                inputElement.attr('data-picture-data', reader.result);
                if(event.target.id === 'profile-image'){
                    $scope.me.profileImageData = reader.result;
                } else {
                    $scope.me.coverImageData = reader.result;
                }
            };
            reader.readAsDataURL(file);
        }
    };

    $scope.loadNewsFeed = function(){
        if(authentication.isLogged()) {
            if ($scope.busy){
                return;
            }
            $scope.busy = true;
            usSpinnerService.spin('spinner-1');

            profileService(authentication.getAccessToken()).getNewsFeed(PAGE_SIZE, feedStartPostId).$promise.then(
                function (data) {
                    $scope.posts = $scope.posts.concat(data);
                    if($scope.posts.length > 0){
                        feedStartPostId = $scope.posts[$scope.posts.length - 1].id;
                    }
                    $scope.busy = false;
                    $scope.isNewsFeed = true;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to load news feed.", error);
                }
            );
        }
    };

    $scope.getOwnFiendsList = function(){
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            profileService(authentication.getAccessToken()).getFriendsList().$promise.then(
                function (data) {
                    $scope.friendsList = data;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to load friends", error);
                }
            );
        }
    };

    $scope.getOwnFriendsListPreview = function(){
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            profileService(authentication.getAccessToken()).getFriendsListPreview().$promise.then(
                function (data) {
                    data.userFriendsUrl = '#/user/' + $scope.username + '/friends/';
                    $scope.friendsListPreview = data;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to load friends", error);
                }
            );
        }
    };



    $scope.sendFollowRequest = function(previewData){
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            userService(authentication.getAccessToken()).sendFollowRequest(previewData.username).$promise.then(
                function () {
                    notifyService.showInfo("Successfully followed " + previewData.fullname + '.');
                    previewData.isFollowing = true;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError("Failed to follow user " + previewData.fullname + "!", error);
                    usSpinnerService.stop('spinner-1');
                }
            );
        }
    };

    $scope.sendUnfollowRequest = function(previewData){
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            userService(authentication.getAccessToken()).sendUnfollowRequest(previewData.username).$promise.then(
                function () {
                    notifyService.showInfo("Successfully unfollowed " + previewData.fullname + '.');
                    previewData.isFollowing = false;
                    usSpinnerService.stop('spinner-1');
                },
                function (error) {
                    notifyService.showError("Failed to unfollow user " + previewData.fullname + "!", error);
                    usSpinnerService.stop('spinner-1');
                }
            );
        }
    };

    $scope.clickUpload = function(element){
        angular.element(element).trigger('click');
    };
});