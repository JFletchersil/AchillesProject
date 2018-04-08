import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Exercise, ExerciseType } from '../../domain/exercise';
import { Exercises } from '../../domain/exercises';
import { environment } from '@app/env';
import { AdditionalExercises } from '../../domain/additionalExercises';

@Injectable()
export class ExerciseServiceProvider {

  constructor(public http: HttpClient) {
  }

  getAdditionalExercises(stage : number) : Promise<AdditionalExercises>{
    return new Promise(res => {
      this.http.get(environment.api + 'api/Exercises/additional/' + stage).subscribe(exercises =>{
        res(exercises as AdditionalExercises);
      }, err => {console.log('Cannot find additional exercises"');});
    });
  }

  public getExercises(): Promise<Exercises> {
    return new Promise<Exercises>((resolve, reject) => {
      this.http.get(environment.api + 'api/Exercises/GetDailyExercises').subscribe(exercises =>{
        console.log(exercises);
        resolve(exercises as Exercises);
      }, err => {console.log('Cannot find additional exercises"');});
    });
  }

  // Generates an array of numbers from 0 to maxRepsOrSeconds
  // For use in a select box to select the time taken
  public getSelectBoxIntervals(maxRepsOrSeconds: number) {
    let intervals = Array.from(Array(maxRepsOrSeconds+1).keys());
    return intervals;
  }

  // There's no way to simply use a for loop of numbers in *ngFor
  // So I've had to make an array here purely to display multiple boxes for the reps
  public getSetsArray(sets: number) {
    let intervals = Array.from(Array(sets).keys());
    return intervals;
  }

}
