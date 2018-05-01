import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Exercise, ExerciseType, SaveExercise } from '../../domain/exercise';
import { Exercises } from '../../domain/exercises';
import { environment } from '@app/env';
import { AdditionalExercises } from '../../domain/additionalExercises';
import { success } from 'domain/success';
import { UnapprovedExercises } from 'domain/UnapprovedExercises';



@Injectable()

/**
 * A class which provides a range of helpful methods for exercises
 * @class ExerciseServiceProvider
 * @module AppModule
 * @submodule Providers
 */
export class ExerciseServiceProvider {

  /**
   * Creates an instance of ExerciseServiceProvider.
   * @param {HttpClient} http Used to perform HTTP requests.
   * @memberof ExerciseServiceProvider
   * @method constructor
   */
  constructor(public http: HttpClient) {
  }

  /**
   * Validates whether a given object is an exercise object.
   * @param {any} exercises An object to check whether it is a valid exercise object.
   * @returns true if the object is a valid exercise object.
   * @memberof ExerciseServiceProvider
   * @method validateExercises
   */
  validateExercises(exercises) {
    exercises.forEach((exercise) => {
      if (exercise.completedResults.completedReps == null) exercise.completedResults.completedReps = [];
      if (exercise.completedResults.completedTimes == null) exercise.completedResults.completedTimes = [];
    });
    return exercises;
  }

  /**
   * 
   * Gets the list of additional exercises from the API.
   * @param {number} stage the stage to get additional exercises for.
   * @returns {Promise<AdditionalExercises>} A promise which holds the additional exercises list.
   * @memberof ExerciseServiceProvider
   * @method getAdditionalExercises
   */
  public getAdditionalExercises(stage: number): Promise<AdditionalExercises> {
    return new Promise(res => {
      this.http.get(environment.api + 'api/Exercises/additional/' + stage).subscribe(exercises => {
        res(exercises as AdditionalExercises);
      }, err => { console.log('Cannot find additional exercises'); });
    });
  }

  /**
   * 
   * Gets the list of additional unapproved exercises from the API.
   * @returns {Promise<AdditionalExercises>} A promise which holds the unapproved additional exercises list.
   * @memberof ExerciseServiceProvider
   * @method getUnapprovedExercises
   */
  public getUnapprovedExercises(): Promise<UnapprovedExercises> {
    return new Promise(res => {
      this.http.get(environment.api + 'api/Exercises/GetUnapprovedExercises').subscribe(exercises => {
        res(exercises as UnapprovedExercises);
      }, err => { console.log('Cannot find unapproved exercises'); })
    });
  }

  /**
   * Gets the daily exercises the user must perform.
   * @param {string} sessionId a valid sessionId which is used to validate the user.
   * @returns {Promise<Exercises>} A promise which holds the exercises for the user.
   * @memberof ExerciseServiceProvider
   * @method getExercises
   */
  public getExercises(sessionId: string): Promise<Exercises> {
    return new Promise<Exercises>((resolve, reject) => {
      this.http.get(environment.api + `api/Exercises/GetDailyExercises?sessionId=${sessionId}`)
        .subscribe(exercises => {
          //console.log(exercises);
          let uncorrected = exercises as Exercises;
          //console.log(uncorrected)
          resolve(this.validateExercises(uncorrected));
        }, err => { console.log('Cannot find additional exercises'); });
    });
  }

  /**
   * Saves a single daily exercise to the API
   * @param {SaveExercise} exercise the exercise object to save the API.
   * @returns {Promise<success>} A promise which indicates whether the action was a success or not.
   * @memberof ExerciseServiceProvider
   * @method saveExercise
   */
  public saveExercise(exercise: SaveExercise): Promise<success> {
    return new Promise((resolve, reject) => {
      this.http.post(environment.api + `api/Exercises/SaveSingleDailyExercises`, exercise)
        .subscribe(result => {
          resolve(result as success)
        }), err => { console.log('has not saved'); }
    });
  }

  /**
   * Generates an array of numbers from 0 to maxRepsOrSeconds. For use in a select box to select the time taken
   * @param {number} maxRepsOrSeconds 
   * @returns intervals
   * @memberof ExerciseServiceProvider
   * @method getSelectBoxIntervals
   */
  public getSelectBoxIntervals(maxRepsOrSeconds: number) {
    let intervals = Array.from(Array(maxRepsOrSeconds + 1).keys());
    return intervals;
  }

  /**
   * Because there's no way to simply use a for loop of numbers in *ngFor, this function makes an array to display multiple boxes for the reps.
   * @param {number} sets The number of sets, which decides how many select boxes to create.
   * @returns An array of intervals for use in select boxes.
   * @memberof ExerciseServiceProvider
   * @method getSetsArray
   */
  public getSetsArray(sets: number) {
    let intervals = Array.from(Array(sets).keys());
    return intervals;
  }

  /**
   * compares the length of a completed array, with the length of the current array
   * 
   * @param {number[]} completed 
   * @param {number} length 
   * @param {number} mustMatch 
   * @memberof ExerciseServiceProvider
   * @method returnAreCompletedAndCurrentEqual
   */
  public returnAreCompletedAndCurrentEqual(completed: number[], length: number, mustMatch: number) {
    if (completed !== null && completed !== undefined) {
      let comLen = Object.keys(completed).length
      if (!(comLen === length)) return false;
      let areEqual = true;
      completed.forEach(element => {
        if (!(element === mustMatch)) {
          areEqual = false;
        }
      });
      return areEqual;
    } else {
      return false;
    }

  }
}
