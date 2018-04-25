import { Component, Input, OnInit } from '@angular/core';
import { IonicPage, NavController, NavParams, AlertController } from 'ionic-angular';
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
export class WorkoutPage implements OnInit {

  @Input() exercise: Exercise;

  loginPage = LoginPage;
  workoutSummaryPage = WorkoutSummaryPage;
  exerciseTimed = ExerciseType.Timed;
  exerciseRepsSets = ExerciseType.RepsSets;
  videoLink;
  sessionId: string = "";
  useAutomatic: boolean;
  automaticTimer: Subscription;
  automaticTimerValue: number;
  timerStopped: boolean;

  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    private _exerciseService: ExerciseServiceProvider,
    public sanitizer: DomSanitizer,
    private storage: Storage,
    private _loginServiceProvider: LoginServiceProvider,
    private alertCtrl: AlertController) {
  }

  goBack() {
    let saveModel = {"sessionId": this.sessionId, "resultViewModel": this.exercise.completedResults};
    this._exerciseService.saveExercise((saveModel as SaveExercise)).then((value) => {
       //console.log(value)
       let modal = this.alertCtrl.create({
         title: `Save Succesfull`,
         message: `Your exercise has been saved.`,
         buttons: ["Dismiss"]
       });
       modal.present();
    });
  }

  customTrackBy(index: number, obj:any): any {
    return index;
  }
  onRepsKnownChange (value, index) {
    //console.log(value+" "+index);
  }
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

  toggleAutomatic(){
    this.useAutomatic = !this.useAutomatic;
    this.timerStopped = false;
  }

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

  stopCounting(){
    this.timerStopped = true;
    this.stopAutomaticTimer();
    this.exercise.completedResults.completedTimes[0] = this.automaticTimerValue;
    //console.log(this.exercise.completedResults);
  }

  stopAutomaticTimer(){
    try{
      this.automaticTimer.unsubscribe();
    } catch(error){
      //timer never began.
    }
  }

  ionViewDidLeave(){
   this.stopAutomaticTimer();
  }
}
