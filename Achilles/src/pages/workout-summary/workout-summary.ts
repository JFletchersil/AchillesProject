import { Component, OnInit } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { WorkoutPage } from '../workout/workout';
import { ExerciseServiceProvider } from '../../providers/exercise-service/exercise-service';
import { Exercises } from '../../domain/exercises';
import { ExerciseType } from '../../domain/exercise';

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
    private _exerciseServiceProvider: ExerciseServiceProvider) {
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad WorkoutSummaryPage');
  }

  ngOnInit() {
    this.todaysExercises = this._exerciseServiceProvider.getExercises();
  }

  // Generates an array of numbers from 0 to time
  // For use in a select box to select the time taken
  getTimeIntervals(time: number) {
    let intervals = Array.from(Array(time+1).keys());
    return intervals;
  }
}
