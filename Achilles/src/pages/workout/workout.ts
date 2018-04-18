import { Component, Input, OnInit } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { WorkoutSummaryPage } from '../workout-summary/workout-summary';
import { Exercise, ExerciseType, completedResults, SaveExercise } from '../../domain/exercise';
import { ExerciseServiceProvider } from '../../providers/exercise-service/exercise-service';
import { DomSanitizer } from '@angular/platform-browser';
import { environment } from '@app/env';
import { LoginPage } from '../login/login';
import { LoginServiceProvider } from '../../providers/login-service/login-service';
import { Storage } from '@ionic/storage';
import { Exercises } from 'domain/exercises';

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
  
  constructor(
    public navCtrl: NavController, 
    public navParams: NavParams,
    private _exerciseService: ExerciseServiceProvider,
    public sanitizer: DomSanitizer,
    private storage: Storage,
    private _loginServiceProvider: LoginServiceProvider) {
  }

  goBack() {
    let saveModel = {"sessionId": this.sessionId, "resultViewModel": this.exercise.completedResults};
    this._exerciseService.saveExercise((saveModel as SaveExercise)).then((value) => {
      // console.log(value)
    });
  }

  customTrackBy(index: number, obj:any): any {
    return index;
  }
  onRepsKnownChange (value, index) {
    console.log(value+" "+index);
  }
  ngOnInit() {
    // Sets the exercise to the one clicked on the workout page.
    this.exercise = this.navParams.data;
    
    // This is needed to bypass security warnings of unsanitised urls in browsers.
    this.videoLink = this.sanitizer.bypassSecurityTrustResourceUrl(this.exercise.videoLink);

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
      }
    });
  }
}
