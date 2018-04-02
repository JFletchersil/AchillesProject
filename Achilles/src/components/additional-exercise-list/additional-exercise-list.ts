import { Component } from '@angular/core';
import { ExerciseServiceProvider } from '../../providers/exercise-service/exercise-service';
import { AdditionalExercises } from '../../domain/additionalExercises';
import {AdditionalExercise} from '../../domain/additionalExercise';
import { AlertController } from 'ionic-angular';

/**
 * Generated class for the AdditionalExerciseListComponent component.
 *
 * See https://angular.io/api/core/Component for more info on Angular
 * Components.
 */
@Component({
  selector: 'additional-exercise-list',
  templateUrl: 'additional-exercise-list.html'
})
export class AdditionalExerciseListComponent {

  additionalExercises : AdditionalExercises;

  constructor(private alertCtrl: AlertController, private exerciseProvider: ExerciseServiceProvider) {
    this.setAdditionalExercises(1);
  }

  async setAdditionalExercises(stage : number){
    this.additionalExercises = await this.exerciseProvider.getAdditionalExercises(1);
  }

    spawnDescription(exercise : AdditionalExercise){
        let modal = this.alertCtrl.create({
          title: exercise.exercise,
          message: exercise.description,
          buttons: ['Dismiss']
        });
        modal.present();
      }

}
