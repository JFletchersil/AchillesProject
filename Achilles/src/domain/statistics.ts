import { statistic } from "./statistic";
import { DateTime } from "ionic-angular";
import { ExerciseType } from "./exercise";

/**
 * 
 * @export
 * @class Statistics
 */
export class Statistics{

  private statistics : Array<statistic>;

  /**
   * Creates an instance of Statistics.
   * @memberof Statistics
   */
  constructor(){
    this.statistics = new Array<statistic>();
  }

  /**
   * Pushes a given statistic to the Statistics array.
   * @param {statistic} statistic 
   * @memberof Statistics
   */
  public addStatistics(statistic: statistic){
    this.statistics.push(statistic);
  }

  /**
   * Gets all exercises for a given exercise type inside the statistics object.
   * @param {ExerciseType} type the type of exercise to filter for.
   * @returns The filtered statistics object.
   * @memberof Statistics
   */
  public getAllExercises(type: ExerciseType){
    return this.statistics.filter(x =>{
      x.getExerciseType() == type;
    })
  }

  /**
   * Gets the earliest date that the user has performed exercises for.
   * @returns {Date} A DateTime which is the earliest date found.
   * @memberof Statistics
   */
  public getEarlierstDate(): Date{
    let lowestDate = null;
    this.statistics.forEach(stat =>{
      if(!lowestDate) lowestDate = stat.getExerciseDate();
      else if(lowestDate < stat.getExerciseDate()) lowestDate = stat.getExerciseDate();
    });
    return new Date(lowestDate);
  }

  /**
   * Gets the approximate end date for when the user is at the end of the current stage.
   * @returns {Date} the approximate end date.
   * @memberof Statistics
   */
  public getApproximateEndDate(): Date{
    let startDate = this.getEarlierstDate();
    startDate.setMonth(startDate.getMonth()+6);
    return startDate as Date;
  }


  /**
   * Gets the average success rate of exercises.
   * @param {string} exercise the exercise to measure the average success rate of.
   * @returns {number} the percentage of completion.
   * @memberof Statistics
   */
  public getAverageSuccessOfExercies(exercise:string) : number{
    let total = 0;
    let count = 0;
    this.statistics
        .filter(x => x.getExerciseName().indexOf(exercise) > -1)
        .forEach(x => {
          x.getCompletedResults().forEach(y =>{
            if(x.getExerciseType() == ExerciseType.RepsSets) total += (y/x.getReps())*100;
            else total += (y/x.getTime())*100;
            count +=1;
          });
        });
    return total/count;
  }

}
