import { Component, OnInit } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { WorkoutPage } from '../workout/workout';
import { ExerciseServiceProvider } from '../../providers/exercise-service/exercise-service';
import { Exercises } from '../../domain/exercises';

@IonicPage()
@Component({
  selector: 'page-workout-summary',
  templateUrl: 'workout-summary.html',
})
export class WorkoutSummaryPage implements OnInit {

  workoutPage = WorkoutPage;
  todaysExercises: Exercises;

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
}
