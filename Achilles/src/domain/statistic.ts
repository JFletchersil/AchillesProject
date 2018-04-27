import { DateTime } from "ionic-angular";
import { ExerciseType } from "./exercise";


/**
 * 
 * @export
 * @class statistic
 */
export class statistic{

  private date: Date;
  private exercise: string;
  private reps?: number;
  private sets?: number;
  private time?: number;
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
   * 
   * 
   * @returns {ExerciseType} 
   * @memberof statistic
   */
  public getExerciseType(): ExerciseType{
    if(this.sets) return ExerciseType.RepsSets;
    else return ExerciseType.Timed;
  }

  /**
   * 
   * 
   * @returns {Date} 
   * @memberof statistic
   */
  public getExerciseDate():Date{
    return this.date;
  }


  /**
   * 
   * 
   * @returns {string} 
   * @memberof statistic
   */
  public getExerciseName():string{
    return this.exercise;
  }

  /**
   * 
   * 
   * @returns {number} 
   * @memberof statistic
   */
  public getReps(): number{
    return this.reps;
  }

  /**
   * 
   * 
   * @returns {number} 
   * @memberof statistic
   */
  public getSets(): number{
    return this.sets;
  }

  /**
   * 
   * 
   * @returns {number} 
   * @memberof statistic
   */
  public getTime(): number{
    if(this.time) return this.time;
    else return -1;
  }

  /**
   * 
   * 
   * @returns {Array<number>} 
   * @memberof statistic
   */
  public getCompletedResults():Array<number>{
    return this.completedResults;
  }
}
