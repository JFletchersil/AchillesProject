import { DateTime } from "ionic-angular";
import { ExerciseType } from "./exercise";

/**
 * A class which holds a statistic for the user's progress
 * @class statistic
 * @module AppModule
 * @submodule Domain
 */
export class statistic{

  /**
   * The date of the statistic
   * @type {Date}
   * @memberof statistic
   * @property date
   */
  private date: Date;

  /**
   * The name of the exercise to show statistics for
   * @type {string}
   * @memberof statistic
   * @property exercise
   */
  private exercise: string;

  /**
   * The number of reps.
   * @type {number}
   * @memberof statistic
   * @property reps
   */
  private reps?: number;

  /**
   * The number of sets.
   * @type {number}
   * @memberof statistic
   * @property sets
   */
  private sets?: number;

  /**
   * The compeleted time in seconds.
   * @type {number}
   * @memberof statistic
   * @property time
   */
  private time?: number;

  /**
   * A collection holding the completed results.
   * @type {Array<number>}
   * @memberof statistic
   * @property completedResults
   */
  private completedResults: Array<number>

  /**
   * Creates an instance of statistic.
   * @param {Date} date 
   * @param {{}} exerciseResult a JSON object of the exercise results.
   * @param {string} exercise The exercise to apply statistics too.
   * @param {number} [reps] The number of reps completed.
   * @param {number} [sets] The number of sets.
   * @param {number} [time] The time managed.
   * @memberof statistic
   * @method constructor
   */
  constructor(date: Date, exerciseResult: {}, exercise:string, reps?: number, sets?:number, time?:number){
    this.date = date;
    this.exercise = exercise;
    this.sets = sets;
    this.reps = reps;
    this.time = time;
    if(sets){
      this.completedResults = exerciseResult['CompletedReps'];
    } else this.completedResults = exerciseResult["CompletedTimes"]
    if(!this.completedResults) this.completedResults = [];
  }

  /**
   * Gets the exercise type.
   * @returns {ExerciseType} 
   * @memberof statistic
   * @method getExerciseType
   */
  public getExerciseType(): ExerciseType{
    if(this.sets) return ExerciseType.RepsSets;
    else return ExerciseType.Timed;
  }

  /**
   * Gets the exercise date.
   * @returns {Date} 
   * @memberof statistic
   * @method getExerciseDate
   */
  public getExerciseDate():Date{
    return this.date;
  }

  /** 
   * gets the exercise name
   * @returns {string} 
   * @memberof statistic
   * @method getExerciseName
   */
  public getExerciseName():string{
    return this.exercise;
  }

  /**
   * Gets the reps.
   * @returns {number} 
   * @memberof statistic
   * @method getReps
   */
  public getReps(): number{
    return this.reps;
  }

  /**
   * Gets the sets.
   * @returns {number} 
   * @memberof statistic
   * @method getSets
   */
  public getSets(): number{
    return this.sets;
  }

  /**
   * Gets the time.
   * @returns {number} 
   * @memberof statistic
   * @method getTime
   */
  public getTime(): number{
    if(this.time) return this.time;
    else return -1;
  }

  /**
   * Gets the completed results
   * @returns {Array<number>} 
   * @memberof statistic
   */
  public getCompletedResults():Array<number>{
    return this.completedResults;
  }
}
