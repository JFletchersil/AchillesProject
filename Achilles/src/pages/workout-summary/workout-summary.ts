import { Component, OnInit } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { WorkoutPage } from '../workout/workout';
import { ExerciseServiceProvider } from '../../providers/exercise-service/exercise-service';
import { Exercises } from '../../domain/exercises';
import { ExerciseType } from '../../domain/exercise';
import {EnvConfigurationProvider} from "gl-ionic2-env-configuration";
import {ITestAppEnvConfiguration} from "../../env-configuration/ITestAppEnvConfiguration";

@IonicPage()
@Component({
  selector: 'page-workout-summary',
  templateUrl: 'workout-summary.html',
})
export class WorkoutSummaryPage implements OnInit {

  workoutPage = WorkoutPage;
  todaysExercises: Exercises;
  exerciseTimed = ExerciseType.Timed;
  exerciseRepsSets = ExerciseType.RepsSets;

  constructor(
    public navCtrl: NavController, 
    public navParams: NavParams,
    private _exerciseServiceProvider: ExerciseServiceProvider, 
    private envConfiguration: EnvConfigurationProvider<ITestAppEnvConfiguration>) {
      let config: ITestAppEnvConfiguration = envConfiguration.getConfig();
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad WorkoutSummaryPage');
  }

  ngOnInit() {
    this.todaysExercises = this._exerciseServiceProvider.getExercises();
  }


}
