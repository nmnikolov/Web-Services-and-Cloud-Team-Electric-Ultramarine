app.factory('commentService', function($http, $q, $resource, BASE_URL){
    return function(token){
        $http.defaults.headers.common['Authorization'] = 'Bearer ' + token;

        var comment = {},
            resource = $resource(
                BASE_URL + 'tweets/:option1/replies/:option2/:option3',
                {
                    option1: '@option1',
                    option2: '@option2',
                    option3: '@option3'
                },
                {
                    edit: {
                        method: 'PUT'
                    }
                }
            );

        comment.addComment = function(postId, commentData){
            return resource.save({option1: postId}, commentData);
        };

        comment.getPostComments = function(postId){
            return resource.query({option1: postId});
        };

        comment.like = function(postId, commentId){
            return resource.save({option1: postId, option2: commentId, option3: "likes"})
        };

        comment.unlike = function(postId, commentId){
            return resource.remove({option1: postId, option2: commentId, option3: "likes"})
        };

        comment.removeComment = function(postId, commentId){
            return resource.remove({option1: postId, option2: commentId});
        };

        comment.editComment = function(postId, commentId, commentContent){
            var commentData = { 'content': commentContent};
            return resource.edit({option1: postId, option2: commentId}, commentData);
        };

        return comment;
    }
});