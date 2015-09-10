app.factory('authHttpResponseInterceptor',['$q','$location',function($q,$location){
    return {
        response: function(response){
            if (response.status === 401) {
                $location.path('/');
            }
            return response || $q.when(response);
        },
        responseError: function(rejection) {
            if (rejection.status === 400) {
                $location.path('/');
            } else if (rejection.status === 401) {
                localStorage.removeItem('accessToken');
                localStorage.removeItem('username');
                $location.path('/');
            } else if(rejection.status === 404){
                $location.path('/404/');
            }
            return $q.reject(rejection);
        }
    }
}]);