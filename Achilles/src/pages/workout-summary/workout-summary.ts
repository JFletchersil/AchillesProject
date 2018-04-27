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


/**
 * 
 * 
 * @export
 * @class WorkoutSummaryPage
 * @implements {OnInit}
 */
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
  hasNotLoaded: boolean = false;

  /**
   * Creates an instance of WorkoutSummaryPage.
   * @param {NavController} navCtrl base class for navigation controller components like Nav and Tab.
   * @param {NavParams} navParams class for navigation controller parameters in Ionic.
   * @param {Storage} storage Uses a variety of storage engines underneath, picking the best one available depending on the platform.
   * @param {LoginServiceProvider} _loginServiceProvider A dependency injected instance of the login Service.
   * @param {ExerciseServiceProvider} _exerciseServiceProvider A dependency injected instance of the Exercise Service.
   * @memberof WorkoutSummaryPage
   */
  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    private storage: Storage,
    private _loginServiceProvider: LoginServiceProvider,
    private _exerciseServiceProvider: ExerciseServiceProvider) {
  }

  /** 
   * Checks whether the current exercise and the completed exercise are equal.
   * @param {Exercise} exercise a valid exercise object.
   * @returns True if the current exercise and completed exercise are equal.
   * @memberof WorkoutSummaryPage
   */
  evaluateCompletedStatus(exercise: Exercise) {
    if (exercise.reps !== null) {
      return this._exerciseServiceProvider.returnAreCompletedAndCurrentEqual(exercise.completedResults.completedReps, exercise.sets, exercise.reps);
    } else {
      return this._exerciseServiceProvider.returnAreCompletedAndCurrentEqual(exercise.completedResults.completedTimes, 1, exercise.time);
    }
  }

  /**
   * Checks whether the user has any completed results whatsoever.
   * @param {Exercise} exercise a valid exercise object.
   * @returns true if the user has any completed results whatsoever.
   * @memberof WorkoutSummaryPage
   */
  evaluateHasCompletedAnyExercises(exercise: Exercise) {
    return exercise.completedResults.completedReps.length >= 1 || exercise.completedResults.completedTimes.length >= 1;
  }

  /**
   * Provides a custom implementation of the Angular trackBy method, which keeps the index of for loops consistent.
   * @param {number} index The index.
   * @param {*} obj The track by object or event to manipulate.
   * @returns {*} index.
   * @memberof WorkoutSummaryPage
   */
  customTrackBy(index: number, obj: any): any {
    return index;
  }

  /**
   * 
   * Lifecycle hook which is fired when entering a page, before it becomes the active one.
   * @memberof WorkoutSummaryPage
   */
  ionViewDidEnter() {
    this.storage.get('sessionId').then((sessionId) => {
      if (!sessionId) {
        this.navCtrl.setRoot(this.loginPage);
      } else {
        this._loginServiceProvider.validateSession(sessionId).then((isValidSessionId) => {
          if (!isValidSessionId) {
            this.navCtrl.setRoot(this.loginPage);
          }
          //console.log("valid session for: " + sessionId);
        });

        this.sessionId = sessionId;
        this.getExercises();
      }
    });
  }

  /**
   * Lifecycle hook which is fired before class instantiation.
   * @memberof WorkoutSummaryPage
   */
  ngOnInit() {
  }

  /**
   * Retrieves a promise with the users daily exercises and sets todaysExercises to this value.
   * @memberof WorkoutSummaryPage
   */
  async getExercises() {
    this._exerciseServiceProvider.getExercises(this.sessionId).then((value) => {
      this.todaysExercises = value;
      this.hasNotLoaded = true;
    });
  }

  /**
   * Navigates the user to the workout page using the specified exercise.
   * @param {Exercise} exercise a valid exercise object to display on the workout page.
   * @memberof WorkoutSummaryPage
   */
  public loadExercisePage(exercise: Exercise) {
    this.navCtrl.push(this.workoutPage, exercise);
  }
}
