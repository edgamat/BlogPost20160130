import { inject, Aurelia } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router';
import { AuthenticateStep, FetchConfig } from 'aurelia-authentication';
import {HttpClient} from 'aurelia-fetch-client';

@inject(HttpClient, FetchConfig)
export class App {
    router: Router;
    fetchConfig: FetchConfig;
    http: HttpClient;

    constructor(http: HttpClient, fetchConfig: FetchConfig) {
        this.http = http;
        this.fetchConfig = fetchConfig;
    }

    activate() {
        this.fetchConfig.configure(null);
        this.addLoginRedirect();
    }

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = 'Crossroads';
        config.addPipelineStep('authorize', AuthenticateStep); // Add a route filter to the authorize extensibility point.
        config.map([{
            route: [ '', 'home' ],
            name: 'home',
            settings: { icon: 'home' },
            moduleId: '../home/home',
            nav: true,
            title: 'Home'
        }, {
            route: 'counter',
            name: 'counter',
            settings: { icon: 'education' },
            moduleId: '../counter/counter',
            nav: true,
            title: 'Counter'
        }, {
            route: 'fetch-data',
            name: 'fetchdata',
            settings: { icon: 'th-list' },
            moduleId: '../fetchdata/fetchdata',
            nav: true,
            title: 'Fetch data'
        }, {
            route: 'login',
            moduleId: '../me/login',
            nav: false
        }, {
            route: 'logout',
            moduleId: '../me/logout',
            nav: false
        }

        ]);

        this.router = router;
    }

    addLoginRedirect() {
        // Add the progress bar for requests
        this.http.configure(config => {
            config
                .withInterceptor({
                    response: (response) => {

                        console.log(`Received ${response.status} ${response.url}`);

                        if (response.status == 401) {
                            this.router.navigate('login');
                            throw response;
                        }
                        else {
                            return response;
                        }
                    }
                });
        });
    }
}
