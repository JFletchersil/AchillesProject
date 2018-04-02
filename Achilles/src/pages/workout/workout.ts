import { Component, Input, OnInit } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { WorkoutSummaryPage } from '../workout-summary/workout-summary';
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
    private _exerciseService: ExerciseServiceProvider,
    public sanitizer: DomSanitizer) {
    
  }

  ngOnInit() {
    // Sets the exercise to the one clicked on the workout page.
    this.exercise = this.navParams.data;
    
    // This is needed to bypass security warnings of unsanitised urls in browsers.
    this.videoLink = this.sanitizer.bypassSecurityTrustResourceUrl(this.exercise.videoLink);
  }


}
