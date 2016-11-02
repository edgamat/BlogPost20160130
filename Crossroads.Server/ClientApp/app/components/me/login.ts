import {inject} from 'aurelia-framework';
import {AuthService} from 'aurelia-authentication';

@inject(AuthService)
export class Login {

    authService: AuthService;

    constructor(authService) {
        this.authService = authService;
    };

    heading = 'Login';

    authenticate(name) {
        return this.authService.authenticate(name, null)
            .then(response => {
                console.log(`BOOGA-Auth:${this.authService.isAuthenticated()}`);
            })
            .catch(error => {
                console.log("Authentication error");
                console.log(error);
            });
    }
}