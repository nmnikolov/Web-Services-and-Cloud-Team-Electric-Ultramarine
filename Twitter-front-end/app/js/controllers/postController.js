app.controller('postController', function ($scope, $routeParams, userService, authentication, postService, notifyService, usSpinnerService) {

    $scope.addPost = function(){
        $scope.postData.WallOwnerUsername = $routeParams['username'];
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            postService(authentication.getAccessToken()).addPost($scope.postData).$promise.then(
                function(data){
                    $scope.postData.content = "";
                    $scope.posts.unshift(data);
                    notifyService.showInfo("Post successfully added.");
                    usSpinnerService.stop('spinner-1');
                },
                function(error){
                    $scope.postData.postContent = "";
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to add post!", error);
                }
            );
        }
    };

    $scope.editPost = function(post){
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            postService(authentication.getAccessToken()).editPost(post.id, post.newPostContent).$promise.then(
                function(){
                    post.content = post.newPostContent;
                    usSpinnerService.stop('spinner-1');
                    notifyService.showInfo("Post successfully edited.");
                },
                function(error){
                    notifyService.showError("Failed to edit post!", error);
                    usSpinnerService.stop('spinner-1');
                }
            );
        }
    };

    $scope.deletePost = function(post){
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            postService(authentication.getAccessToken()).removePost(post.id).$promise.then(
                function(){
                    var index =  $scope.posts.indexOf(post);
                    $scope.posts.splice(index, 1);
                    usSpinnerService.stop('spinner-1');
                    notifyService.showInfo("Post successfully deleted.");
                },
                function(error){
                    notifyService.showError("Failed to delete post!", error);
                    usSpinnerService.stop('spinner-1');
                }
            );
        }
    };

    $scope.favoritePost = function(post){
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            postService(authentication.getAccessToken()).favorite(post.id).$promise.then(
                function(){
                    notifyService.showInfo("Post successfully added to favorites.");
                    usSpinnerService.stop('spinner-1');
                    post.isFavorited = true;
                    post.favoritesCount++;
                },
                function(error){
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to add post to favorites!", error);
                }
            );
        }
    };

    $scope.unfavoritePost = function(post){
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            postService(authentication.getAccessToken()).unfavorite(post.id).$promise.then(
                function(){
                    notifyService.showInfo("Post successfully removed from favorites.");
                    usSpinnerService.stop('spinner-1');
                    post.isFavorited = false;
                    post.favoritesCount--;
                },
                function(error){
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to remove post favorites!", error);
                }
            );
        }
    };
});


