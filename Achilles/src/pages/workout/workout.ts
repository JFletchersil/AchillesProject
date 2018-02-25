import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { WorkoutSummaryPage } from '../workout-summary/workout-summary';
import {EnvConfigurationProvider} from "gl-ionic2-env-configuration";
import {ITestAppEnvConfiguration} from "../../env-configuration/ITestAppEnvConfiguration";

@IonicPage()
@Component({
  selector: 'page-workout',
  templateUrl: 'workout.html',
})
export class WorkoutPage {

  workoutSummaryPage = WorkoutSummaryPage;

  goBack() {
    this.navCtrl.pop();
  }

  constructor(public navCtrl: NavController, public navParams: NavParams,
    private envConfiguration: EnvConfigurationProvider<ITestAppEnvConfiguration>) {
    let config: ITestAppEnvConfiguration = envConfiguration.getConfig();
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad WorkoutPage');
  }

}
