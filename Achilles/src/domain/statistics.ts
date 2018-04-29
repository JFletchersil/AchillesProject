import { statistic } from "./statistic";
import { DateTime } from "ionic-angular";
import { ExerciseType } from "./exercise";

export class Statistics{

  private statistics : Array<statistic>;

  constructor(){
    this.statistics = new Array<statistic>();
  }

  public addStatistics(statistic: statistic){
    this.statistics.push(statistic);
  }

  public getAllExercises(type: ExerciseType){
    return this.statistics.filter(x =>{
      x.getExerciseType() == type;
    })
  }

  public getEarlierstDate(): Date{
    let lowestDate = null;
    this.statistics.forEach(stat =>{
      if(!lowestDate) lowestDate = stat.getExerciseDate();
      else if(lowestDate < stat.getExerciseDate()) lowestDate = stat.getExerciseDate();
    });
    return new Date(lowestDate);
  }

  public getApproximateEndDate(): Date{
    let startDate = this.getEarlierstDate();
    startDate.setMonth(startDate.getDay()+42);
    return startDate as Date;
  }

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
    if(isNaN(total/count)) return 0;
    else return total/count;
  }


}
