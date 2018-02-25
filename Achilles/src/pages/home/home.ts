import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
//import { WorkoutSummaryPage } from '../workout-summary/workout-summary';
import {EnvConfigurationProvider} from "gl-ionic2-env-configuration";
import {ITestAppEnvConfiguration} from "../../env-configuration/ITestAppEnvConfiguration";

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage {
  names : Observable<any>;

  constructor(public navCtrl: NavController, public httpClient: HttpClient, 
    private envConfiguration: EnvConfigurationProvider<ITestAppEnvConfiguration>) {
    let config: ITestAppEnvConfiguration = envConfiguration.getConfig();
    this.names = this.httpClient.get(config.api + "api/values");
    this.names.subscribe(data => {
      console.log("names: ", data)
    });
  }
}
