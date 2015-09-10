app.controller('commentController', function ($scope,authentication, commentService, notifyService, usSpinnerService) {

    $scope.addComment = function(post){
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            commentService(authentication.getAccessToken()).addComment(post.id, $scope.commentData).$promise.then(
                function(data){
                    $scope.commentData.commentContent = "";
                    post.replies.unshift(data);
                    post.repliesCount++;
                    notifyService.showInfo("Comment successfully added.");
                    usSpinnerService.stop('spinner-1');
                },
                function(error){
                    $scope.commentData.commentContent = "";
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to add comment!", error);
                }
            );
        }
    };

    $scope.getPostComments = function(post){
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');

            commentService(authentication.getAccessToken()).getPostComments(post.id).$promise.then(
                function(data){
                    post.replies = data;
                    usSpinnerService.stop('spinner-1');
                },
                function(error){
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to load comments!", error);
                }
            );
        }
    };

    $scope.editComment = function(post, comment){
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            commentService(authentication.getAccessToken()).editComment(post.id, comment.id, comment.newCommentContent).$promise.then(
                function(){
                    usSpinnerService.stop('spinner-1');
                    notifyService.showInfo("Comment successfully edited.");
                    comment.content = comment.newCommentContent;
                },
                function(error){
                    notifyService.showError("Failed to edit comment!", error);
                    usSpinnerService.stop('spinner-1');
                }
            );
        }
    };

    $scope.deleteComment = function(post, comment){
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            commentService(authentication.getAccessToken()).removeComment(post.id, comment.id).$promise.then(
                function(){
                    var index =  post.replies.indexOf(comment);
                    post.replies.splice(index, 1);
                    post.repliesCount--;
                    usSpinnerService.stop('spinner-1');
                    notifyService.showInfo("Comment successfully deleted.");
                },
                function(error){
                    notifyService.showError("Failed to delete comment!", error);
                    usSpinnerService.stop('spinner-1');
                }
            );
        }
    };

    $scope.likeComment= function(post, comment){
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            commentService(authentication.getAccessToken()).like(post.id, comment.id).$promise.then(
                function(){
                    notifyService.showInfo("Comment successfully liked.");
                    usSpinnerService.stop('spinner-1');
                    comment.liked = true;
                    comment.likesCount++;
                },
                function(error){
                    usSpinnerService.stop('spinner-1');
                    notifyService.showError("Failed to like comment!", error);
                }
            );
        }
    };

    $scope.unlikeComment = function(post, comment){
        if(authentication.isLogged()) {
            usSpinnerService.spin('spinner-1');
            commentService(authentication.getAccessToken()).unlike(post.id, comment.id).$promise.then(
                function(){
                    notifyService.showInfo("Comment successfully unliked.");
                    usSpinnerService.stop('spinner-1');
                    comment.liked = false;
                    comment.likesCount--;
                },
                function(error){
                    notifyService.showError("Failed to unlike comment!", error);
                    usSpinnerService.stop('spinner-1');
                }
            );
        }
    };
});