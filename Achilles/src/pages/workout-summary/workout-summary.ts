import { Component, OnInit } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { WorkoutPage } from '../workout/workout';
import { ExerciseServiceProvider } from '../../providers/exercise-service/exercise-service';
import { Exercises } from '../../domain/exercises';
import { ExerciseType, Exercise } from '../../domain/exercise';

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

  ngOnInit() {
    this.getExercises();
  }

  async getExercises(){
    this._exerciseServiceProvider.getExercises().then((value) => {
      this.todaysExercises = value;
    });
  }

  public loadExercisePage (exercise: Exercise) {
    this.navCtrl.push(this.workoutPage, exercise);
  }
}
