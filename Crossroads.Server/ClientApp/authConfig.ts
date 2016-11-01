let config = {
    // SPA related options
    // ===================

    // The SPA url to which the user is redirected after a successful login
    loginRedirect: '#/',
    // The SPA url to which the user is redirected after a successful logout
    logoutRedirect: '#/',
    // The SPA route used when an unauthenticated user tries to access an SPA page that requires authentication
    loginRoute: '#/login',
    // Whether or not an authentication token is provided in the response to a successful signup
    //loginOnSignup: true,
    // If loginOnSignup == false: The SPA url to which the user is redirected after a successful signup (else loginRedirect is used)
    //signupRedirect:  '#/login',


    // API related options
    // ===================

    // The base url used for all authentication related requests, including provider.url below.
    // This appends to the httpClient/endpoint base url, it does not override it.
    //baseUrl = '',
    // The API endpoint to which login requests are sent
    loginUrl: 'Auth/Token/Exchange',
    // The API endpoint to which signup requests are sent
    //signupUrl: '/auth/signup',
    // The API endpoint used in profile requests (inc. `find/get` and `update`)
    profileUrl: 'Auth/UserInfo',
    // The API endpoint used with oAuth to unlink authentication
    //unlinkUrl: 'http://localhost:22530/ui/logout',
    // The HTTP method used for 'unlink' requests (Options: 'get' or 'post')
    unlinkMethod: 'get',


    // Token Options
    // =============

    // The header property used to contain the authToken in the header of API requests that require authentication
    authHeader: 'Authorization',
    // The token name used in the header of API requests that require authentication
    authTokenType: 'Bearer',
    // The the property from which to get the access token after a successful login or signup
    accessTokenProp: 'access_token',

    // If the property defined by `accessTokenProp` is an object:
    // ------------------------------------------------------------

    //This is the property from which to get the token `{ "accessTokenProp": { "accessTokenName" : '...' } }`
    accessTokenName: 'token',
    // This allows the token to be a further object deeper `{ "accessTokenProp": { "accessTokenRoot" : { "accessTokenName" : '...' } } }`
    accessTokenRoot: false,

    // Refresh Token Options
    // =====================

    // Option to turn refresh tokens On/Off
    useRefreshToken: true,
    // The option to enable/disable the automatic refresh of Auth tokens using Refresh Tokens
    autoUpdateToken: true,

    // The the property from which to get the refresh token after a successful token refresh
    refreshTokenProp: 'refresh_token',

    // Miscellaneous Options
    // =====================

    // Whether to enable the fetch interceptor which automatically adds the authentication headers
    // (or not... e.g. if using a session based API or you want to override the default behaviour)
    httpInterceptor: true,
    // For OAuth only: Tell the API whether or not to include token cookies in the response (for session based APIs)
    withCredentials: true,
    // Controls how the popup is shown for different devices (Options: 'browser' or 'mobile')
    platform: 'browser',
    // Determines the `window` property name upon which aurelia-authentication data is stored (Default: `window.localStorage`)
    storage: 'localStorage',
    // The key used for storing the authentication response locally
    storageKey: 'crossroads_authentication',

    providers: {
        identSrv: {
            name: 'identSrv',
            url: 'Auth/Token/Exchange',
            authorizationEndpoint: 'http://localhost:5000/connect/authorize', //if this ends with slash --> game over

            redirectUri: window.location.origin || window.location.protocol + '//' + window.location.host,
            scope: ['apiAccess', 'openid', 'profile', 'email', 'offline_access'],
            clientId: 'crossroads',
            responseType: 'code',
            scopePrefix: '',
            scopeDelimiter: ' ',
            oauthType: '2.0',
            requiredUrlParams: ['scope', 'nonce'],
            optionalUrlParams: ['display', 'state'],
            state: function () {
                var val = ((Date.now() + Math.random()) * Math.random()).toString().replace(".", "");
                return encodeURIComponent(val);
            },
            nonce: function () {
                var val = ((Date.now() + Math.random()) * Math.random()).toString().replace(".", "");
                return encodeURIComponent(val);
            },
            popupOptions: { width: 452, height: 633 },
            resource: 'aurelia-openiddict-resources aurelia-openiddict-server'
        }
    }
};

export default config;