import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Exercise, ExerciseType, SaveExercise } from '../../domain/exercise';
import { Exercises } from '../../domain/exercises';
import { environment } from '@app/env';
import { AdditionalExercises } from '../../domain/additionalExercises';
import { success } from 'domain/success';

@Injectable()
export class ExerciseServiceProvider {

  constructor(public http: HttpClient) {
  }

  validateExercises(exercises){
    exercises.forEach((exercise) =>{
      if(exercise.completedResults.completedReps ==  null) exercise.completedResults.completedReps = [];
      if(exercise.completedResults.completedTimes == null) exercise.completedResults.completedTimes = [];
    });
    return exercises;
  }

  public getAdditionalExercises(stage : number) : Promise<AdditionalExercises>{
    return new Promise(res => {
      this.http.get(environment.api + 'api/Exercises/additional/' + stage).subscribe(exercises =>{
        res(exercises as AdditionalExercises);
      }, err => {console.log('Cannot find additional exercises"');});
    });
  }

  public getExercises(sessionId: string): Promise<Exercises> {
    return new Promise<Exercises>((resolve, reject) => {
      this.http.get(environment.api + `api/Exercises/GetDailyExercises?sessionId=${sessionId}`)
      .subscribe(exercises =>{
        console.log(exercises);
        let uncorrected = exercises as Exercises;
        console.log(uncorrected)
        resolve(this.validateExercises(uncorrected));
      }, err => {console.log('Cannot find additional exercises');});
    });
  }

  public saveExercise(exercise: SaveExercise) : Promise<success>{
    return new Promise((resolve, reject) =>{
      this.http.post(environment.api + `api/Exercises/SaveSingleDailyExercises`, exercise)
      .subscribe(result =>{
        resolve(result as success)
      }), err => {console.log('has not saved');}
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

  // I needed a method to compare the length of the completed array, with the length of the current array
  public returnAreCompletedAndCurrentEqual(completed : number[], current : number[]){
    if(completed !== null && completed !== undefined && current !== null && current !== undefined){
      let comLen = Object.keys(completed).length
      let tempLen = (typeof current === 'number') ? 1 : Object.keys(current).length;
      return comLen == tempLen;
    }else{
      return false;
    }

  }
}
