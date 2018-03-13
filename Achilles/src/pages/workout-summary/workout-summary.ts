import { Component, OnInit } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { WorkoutPage } from '../workout/workout';
import { ExerciseServiceProvider } from '../../providers/exercise-service/exercise-service';
import { Exercises } from '../../domain/exercises';
import { ExerciseType, Exercise } from '../../domain/exercise';
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
    // this.todaysExercises = this._exerciseServiceProvider.getExercises();
    this.getExercises();
  }

  async getExercises(){
    this._exerciseServiceProvider.getExercise().then((value) => {
      this.todaysExercises = value;
    });
  }

  // Generates an array of numbers from 0 to maxRepsOrSeconds
  // For use in a select box to select the time taken
  getSelectBoxIntervals(maxRepsOrSeconds: number) {
    let intervals = Array.from(Array(maxRepsOrSeconds+1).keys());
    return intervals;
  }

  // There's no way to simply use a for loop of numbers in *ngFor
  // So I've had to make an array here purely to display multiple boxes
  // for the reps
  getSetsArray(sets: number) {
    let intervals = Array.from(Array(sets).keys());
    return intervals;
  }
}
