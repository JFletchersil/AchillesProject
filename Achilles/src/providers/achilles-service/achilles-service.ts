import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {EnvConfigurationProvider} from "gl-ionic2-env-configuration";
import {ITestAppEnvConfiguration} from "../../env-configuration/ITestAppEnvConfiguration";
/*
  Generated class for the AchillesServiceProvider provider.

  See https://angular.io/guide/dependency-injection for more info on providers
  and Angular DI.
*/
@Injectable()
export class AchillesServiceProvider {

  constructor(public http: HttpClient, 
    private envConfiguration: EnvConfigurationProvider<ITestAppEnvConfiguration>) {
    let config: ITestAppEnvConfiguration = envConfiguration.getConfig();
    console.log('Hello AchillesServiceProvider Provider');
  }

}
