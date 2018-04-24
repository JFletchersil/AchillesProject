import { Component, OnInit } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { WorkoutPage } from '../workout/workout';
import { ExerciseServiceProvider } from '../../providers/exercise-service/exercise-service';
import { Exercises } from '../../domain/exercises';
import { ExerciseType, Exercise } from '../../domain/exercise';
import { environment } from '@app/env';
import { LoginPage } from '../login/login';
import { LoginServiceProvider } from '../../providers/login-service/login-service';
import { Storage } from '@ionic/storage';

@IonicPage()
@Component({
  selector: 'page-workout-summary',
  templateUrl: 'workout-summary.html',
})
export class WorkoutSummaryPage implements OnInit {

  loginPage = LoginPage;
  workoutPage = WorkoutPage;
  todaysExercises: Exercises;
  exerciseTimed = ExerciseType.Timed;
  exerciseRepsSets = ExerciseType.RepsSets;
  sessionId: string = "";

  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    private storage: Storage,
    private _loginServiceProvider: LoginServiceProvider,
    private _exerciseServiceProvider: ExerciseServiceProvider) {
  }

  evaluateCompletedStatus(exercise: Exercise){
    return this._exerciseServiceProvider.returnAreCompletedAndCurrentEqual(exercise.completedResults.completedReps, this._exerciseServiceProvider.getSetsArray(exercise.sets))
        || this._exerciseServiceProvider.returnAreCompletedAndCurrentEqual(exercise.completedResults.completedTimes, [exercise.time])
  }

  customTrackBy(index: number, obj:any): any {
    return index;
  }

  ngOnInit() {
    this.storage.get('sessionId').then((sessionId) => {
      if (!sessionId) {
        this.navCtrl.setRoot(this.loginPage);
      } else {
        this._loginServiceProvider.validateSession(sessionId).then((isValidSessionId) => {
          if (!isValidSessionId) {
            this.navCtrl.setRoot(this.loginPage);
          }
          console.log("valid session for: " + sessionId);
        });

        this.sessionId = sessionId;
        this.getExercises();
      }
    });
  }

  async getExercises(){
    this._exerciseServiceProvider.getExercises(this.sessionId).then((value) => {
      this.todaysExercises = value;
      console.log(this.todaysExercises);
    });
  }

  public loadExercisePage (exercise: Exercise) {
    this.navCtrl.push(this.workoutPage, exercise);
  }
}
