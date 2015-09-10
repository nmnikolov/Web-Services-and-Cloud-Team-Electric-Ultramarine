app.controller('mainController', function ($scope, $interval, $location, $routeParams, authentication, DEFAULT_PROFILE_IMAGE) {
    $scope.isLogged = function(){
        return authentication.isLogged();
    };

    $scope.username = authentication.getUsername();
    $scope.defaultImage = DEFAULT_PROFILE_IMAGE;
    $scope.showPendingRequest = false;
    $scope.isOwnWall = authentication.getUsername() === $routeParams['username'];
    $scope.isOwnFeed = $location.path() === '/';

    $scope.toLocalTimeZone = function(item){
        item.postedOn = new Date(item.postedOn);
    };

    $scope.notWallOwner = function(){
        $scope.isOwnWall = false;
    }
});