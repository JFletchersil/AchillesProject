import { Component, Input, OnInit } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { WorkoutSummaryPage } from '../workout-summary/workout-summary';
import {EnvConfigurationProvider} from "gl-ionic2-env-configuration";
import {ITestAppEnvConfiguration} from "../../env-configuration/ITestAppEnvConfiguration";
import { Exercise, ExerciseType } from '../../domain/exercise';
import { ExerciseServiceProvider } from '../../providers/exercise-service/exercise-service';
import { DomSanitizer } from '@angular/platform-browser';

@IonicPage()
@Component({
  selector: 'page-workout',
  templateUrl: 'workout.html',
})
export class WorkoutPage implements OnInit {

  @Input() exercise: Exercise;

  workoutSummaryPage = WorkoutSummaryPage;
  exerciseTimed = ExerciseType.Timed;
  exerciseRepsSets = ExerciseType.RepsSets;
  videoLink;

  goBack() {
    this.navCtrl.pop();
  }

  constructor(
    public navCtrl: NavController, 
    public navParams: NavParams,
    private envConfiguration: EnvConfigurationProvider<ITestAppEnvConfiguration>,
    private _exerciseService: ExerciseServiceProvider,
    public sanitizer: DomSanitizer) {
    
    let config: ITestAppEnvConfiguration = envConfiguration.getConfig();
    
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad WorkoutPage');
  }

  ngOnInit() {
    this.exercise = this._exerciseService.getExercise();
    this.videoLink = this.sanitizer.bypassSecurityTrustResourceUrl('https://www.youtube.com/embed/f1HzSAuB-Vw');
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
