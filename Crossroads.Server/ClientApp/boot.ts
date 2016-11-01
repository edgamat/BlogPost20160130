import { Aurelia } from 'aurelia-framework';
import authConfig from './authConfig';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap';

declare const IS_DEV_BUILD: boolean; // The value is supplied by Webpack during the build

export function configure(aurelia: Aurelia) {
    aurelia.use.standardConfiguration();

    if (IS_DEV_BUILD) {
        aurelia.use.developmentLogging();
    }

    aurelia.use.plugin('aurelia-authentication', baseConfig => {
        baseConfig.configure(authConfig);
    });

    aurelia.start().then(() => aurelia.setRoot('app/components/app/app'));
}
