import {AuthService} from 'aurelia-authentication';
import {inject} from 'aurelia-framework';

@inject(AuthService)
export class Logout {

    authService: AuthService;

    constructor(authService) {
        this.authService = authService;
    };

    activate() {
        let self = this;

        this.authService.logout("#/")
		    .then(response=> {
		        console.log('Logged out from portal');
		    })
		    .catch(err => {
                console.log('Error logging out from portal');
                console.log(err);
		    });
    }
}