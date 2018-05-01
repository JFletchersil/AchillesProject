import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { IonicPage, NavController, NavParams, AlertController, Navbar } from 'ionic-angular';
import { WorkoutSummaryPage } from '../workout-summary/workout-summary';
import { Exercise, ExerciseType, completedResults, SaveExercise } from '../../domain/exercise';
import { ExerciseServiceProvider } from '../../providers/exercise-service/exercise-service';
import { DomSanitizer } from '@angular/platform-browser';
import { environment } from '@app/env';
import { LoginPage } from '../login/login';
import { LoginServiceProvider } from '../../providers/login-service/login-service';
import { Storage } from '@ionic/storage';
import { Exercises } from 'domain/exercises';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/interval';
import { Subscription } from 'rxjs/Subscription';


@IonicPage()
@Component({
  selector: 'page-workout',
  templateUrl: 'workout.html',
})

/**
 * The page responsible for managing the WorkoutPage
 * @class WorkoutPage
 * @module AppModule
 * @submodule Pages
 */
export class WorkoutPage implements OnInit {

  /**
   * Input method for the workout page which holds the exercise object to display.
   * @type {Exercise}
   * @memberof WorkoutPage
   * @property exercise
   */
  @Input() exercise: Exercise;

  /**
   * Holds a reference to the LoginPage
   * @type {LoginPage}
   * @memberof WorkoutPage
   * @property loginPage
   */
  loginPage = LoginPage;

  /**
   * Holds a reference to the WorkoutSummaryPage
   * @type {WorkoutSummaryPage}
   * @memberof WorkoutPage
   * @property workoutSummaryPage
   */
  workoutSummaryPage = WorkoutSummaryPage;

  /**
   * Holds a copy of the ExerciseType enumerator value for timed exercises.
   * Needed for comparison purposes on the template page.
   * @type {ExerciseType}
   * @memberof WorkoutPage
   * @property exerciseTimed
   */
  exerciseTimed = ExerciseType.Timed;
  
  /**
   * Holds a copy of the ExerciseType enumerator value for reps based exercises.
   * Needed for comparison purposes on the template page.
   * @type {ExerciseType}
   * @memberof WorkoutPage
   * @property exerciseRepsSets
   */  
  exerciseRepsSets = ExerciseType.RepsSets;

  /**
   * Holds a link to the video URL where the example video is hosted.
   * @type {*}
   * @memberof WorkoutPage
   * @property videoLink
   */
  videoLink;

  /**
   * Holds a copy of the user's sessionId
   * @type {string}
   * @memberof WorkoutPage
   * @property sessionId
   */
  sessionId: string = "";

  /**
   * A boolean indicating whether to use the automatic timer.
   * If true, indicates to use the automatic timer.
   * @type {boolean}
   * @memberof WorkoutPage
   * @property useAutomatic
   */
  useAutomatic: boolean;

  /**
   * Holds a copy of the automatic timer.
   * @type {Subscription}
   * @memberof WorkoutPage
   * @property automaticTimer
   */
  automaticTimer: Subscription;

  /**
   * Holds the numerical value of the automatic timer.
   * @type {number}
   * @memberof WorkoutPage
   * @property automaticTimerValue
   */
  automaticTimerValue: number;

  /**
   * A boolean indicating whether the timer has been stopped.
   * @type {boolean}
   * @memberof WorkoutPage
   * @property timerStopped
   */
  timerStopped: boolean;

  /**
   * Creates an instance of WorkoutPage.
   * @param {NavController} navCtrl base class for navigation controller components like Nav and Tab.
   * @param {NavParams} navParams class for navigation controller parameters in Ionic.
   * @param {ExerciseServiceProvider} _exerciseService A dependency injected instance of the Exercise Service.
   * @param {DomSanitizer} sanitizer Used to sanitise URLs before being displayed.
   * @param {Storage} storage Uses a variety of storage engines underneath, picking the best one available depending on the platform.
   * @param {LoginServiceProvider} _loginServiceProvider A dependency injected instance of the login Service.
   * @param {AlertController} alertCtrl Ionic controller for handling alerts.
   * @memberof WorkoutPage
   * @method constructor
   */
  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    private _exerciseService: ExerciseServiceProvider,
    public sanitizer: DomSanitizer,
    private storage: Storage,
    private _loginServiceProvider: LoginServiceProvider,
    private alertCtrl: AlertController) {


        
  }

  ionViewDidLoad() {
    this.navBar.backButtonClick = (e:UIEvent)=>{
       this.goBack();
       this.navCtrl.pop();
    }
  }

  /**
   * Spawns a modal window which informs the user of saving changes.
   * @memberof WorkoutPage
   * @method goBack
   */
  goBack() {
    let saveModel = {"sessionId": this.sessionId, "resultViewModel": this.exercise.completedResults};
    this._exerciseService.saveExercise((saveModel as SaveExercise)).then((value) => {
       let modal = this.alertCtrl.create({
         title: `Save Successful`,
         message: `Your exercise has been saved.`,
         buttons: ["OK"]
       });
       modal.present();
    });
  }

  /**
   * Provides a custom implementation of the Angular trackBy method, which keeps the index of for loops consistent.
   * @param {number} index The index.
   * @param {Object} obj The track by object or event to manipulate.
   * @returns {number} The index.
   * @memberof WorkoutPage
   * @method customTrackBy
   */
  customTrackBy(index: number, obj:any): any {
    return index;
  }

  /**
   * Prints the value of reps and the for loop index for testing purposes. 
   * @param {*} value The selected rep value.
   * @param {*} index The relevant index.
   * @memberof WorkoutPage
   * @method onRepsKnownChange
   */
  onRepsKnownChange (value, index) {
    console.log(value+" "+index);
  }

  /**
   * Lifecycle hook that is called after data-bound properties of a directive are initialized. 
   * @memberof WorkoutPage
   * @method ngOnInit
   */
  ngOnInit() {
    // Sets the exercise to the one clicked on the workout page.
    this.exercise = this.navParams.data;

    // This is needed to bypass security warnings of unsanitised urls in browsers.
    this.videoLink = this.sanitizer.bypassSecurityTrustResourceUrl(this.exercise.videoLink);

    this.toggleAutomatic();
    this.timerStopped = false;

    this.storage.get('sessionId').then((sessionId) => {
      if (!sessionId) {
        this.navCtrl.setRoot(this.loginPage);
      } else {
        this._loginServiceProvider.validateSession(sessionId).then((isValidSessionId) => {
          if (!isValidSessionId) {
            this.navCtrl.setRoot(this.loginPage);
          }
         // console.log("valid session for: " + sessionId);
        });

        this.sessionId = sessionId;
      }
    });
  }

  /**
   * Toggles whether the automatic timer should be used instead of the manual timer.
   * @memberof WorkoutPage
   * @method toggleAutomatic
   */
  toggleAutomatic(){
    this.useAutomatic = !this.useAutomatic;
    this.timerStopped = false;
  }

  /**
   * Starts the observable count for the timer. 
   * @memberof WorkoutPage
   * @method startCounting
   */
  startCounting(){
    let automaticTimerStart = new Date().getTime();
    this.automaticTimerValue = 0;
    this.automaticTimer = Observable.interval(1000).subscribe(() =>{
      this.automaticTimerValue = Math.floor((new Date().getTime() - automaticTimerStart)/1000);
      if(this.automaticTimerValue >= this.exercise.time){
        this.automaticTimerValue = this.exercise.time;
        this.stopCounting();
      }
    });
  }

  /**
   * Stops the timer at the current second sets the result to the user's daily results.
   * @memberof WorkoutPage
   * @method stopCounting
   */
  stopCounting(){
    this.timerStopped = true;
    this.stopAutomaticTimer();
    this.exercise.completedResults.completedTimes[0] = this.automaticTimerValue;
  }

  /**
   * Unsubscribes the automatic timer observable which stops the automatic timer.
   * @memberof WorkoutPage
   * @method stopAutomaticTimer
   */
  stopAutomaticTimer(){
    try{
      this.automaticTimer.unsubscribe();
    } catch(error){
      //timer never began.
    }
  }

  /**
   * 
   * Lifecycle hook which is fired when you leave a page, before it stops being the active one.
   * @memberof WorkoutPage
   * @method ionViewDidLeave
   */
  ionViewDidLeave(){
   this.stopAutomaticTimer();
  }
}
