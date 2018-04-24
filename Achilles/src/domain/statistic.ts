import { DateTime } from "ionic-angular";
import { ExerciseType } from "./exercise";

export class statistic{

  private date: Date;
  private exercise: string;
  private reps?: number;
  private sets?: number;
  private time?: number;
  private completedResults: Array<number>

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

  public getExerciseType(): ExerciseType{
    if(this.sets) return ExerciseType.RepsSets;
    else return ExerciseType.Timed;
  }

  public getExerciseDate():Date{
    return this.date;
  }

  public getExerciseName():string{
    return this.exercise;
  }

  public getReps(): number{
    return this.reps;
  }

  public getSets(): number{
    return this.sets;
  }

  public getTime(): number{
    if(this.time) return this.time;
    else return -1;
  }

  public getCompletedResults():Array<number>{
    return this.completedResults;
  }
}
