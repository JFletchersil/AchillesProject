import { Component } from '@angular/core';
import { ExerciseServiceProvider } from '../../providers/exercise-service/exercise-service';

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

  additionalExercises : {};

  constructor(private exerciseProvider: ExerciseServiceProvider) {
    this.setAdditionalExercises(1);
  }

  async setAdditionalExercises(stage : number){
    this.additionalExercises = await this.exerciseProvider.getAdditionalExercises(1);
  }

}
