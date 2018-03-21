import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Exercise, ExerciseType } from '../../domain/exercise';
import { Exercises } from '../../domain/exercises';
import {EnvConfigurationProvider} from "gl-ionic2-env-configuration";
import {ITestAppEnvConfiguration} from "../../env-configuration/ITestAppEnvConfiguration";

const address = 'http://localhost:5000/api/exercises/';

@Injectable()
export class ExerciseServiceProvider {

  constructor(public http: HttpClient, 
    private envConfiguration: EnvConfigurationProvider<ITestAppEnvConfiguration>) {
    let config: ITestAppEnvConfiguration = envConfiguration.getConfig();
    console.log('Hello ExerciseServiceProvider Provider');
  }

  //A promise of any additional exercises retrieved from the API.
  getAdditionalExercises(stage : number){
    return new Promise(res => {
      this.http.get(address + 'additional/' + stage).subscribe(exercises =>{
        console.log(exercises);
        res(exercises);
      }, err => {console.log('Cannot find additiona exercises"');});
    });
  }

  public getExercise(): Promise<Exercises> {
    let config: ITestAppEnvConfiguration = this.envConfiguration.getConfig();
    return new Promise<Exercises>((resolve, reject) => {
      this.http.get(config.api + 'api/Exercises/GetDailyExercises').subscribe(exercises =>{
        console.log(exercises);
        resolve(exercises as Exercises);
      }, err => {console.log('Cannot find additiona exercises"');});
    });
  }

  public getSingleExercise() {
    const exercise: Exercise = {
      id: '1',
      name: 'Standing Calf Stretch',
      videoLink: 'https://www.youtube.com/watch?v=f1HzSAuB-Vw',
      exerciseType: ExerciseType.Timed,
      time: 30,
      completed: true,
      reps: 5,
      sets: 3,
    }
    return exercise;
  }

  public getExercises() {
    const exercises = new Exercises();
    exercises.exercises = [
      {
        id: '1',
        name: 'Standing Calf Stretch',
        videoLink: 'https://www.youtube.com/watch?v=f1HzSAuB-Vw',
        exerciseType: ExerciseType.Timed,
        time: 20,
        completed: true,
      },
      {
        id: '2',
        name: 'Heel Raises',
        videoLink: 'https://www.youtube.com/watch?v=Y_R1CICW6Rw',
        exerciseType: ExerciseType.RepsSets,
        reps: 10,
        sets: 3,
        completed: true,
      },
      {
        id: '3',
        name: 'Step Ups',
        videoLink: 'https://www.youtube.com/watch?v=dQqApCGd5Ss',
        exerciseType: ExerciseType.RepsSets,
        reps: 10,
        sets: 3,
        completed: true,
      },
      {
        id: '4',
        name: 'Towel Stretch',
        videoLink: 'https://www.youtube.com/watch?v=F9QJqN9HYdw',
        exerciseType: ExerciseType.Timed,
        time: 20,
        completed: false,
      },
    ];

    return exercises;
  }

  // Generates an array of numbers from 0 to maxRepsOrSeconds
  // For use in a select box to select the time taken
  public getSelectBoxIntervals(maxRepsOrSeconds: number) {
    let intervals = Array.from(Array(maxRepsOrSeconds+1).keys());
    return intervals;
  }

  // There's no way to simply use a for loop of numbers in *ngFor
  // So I've had to make an array here purely to display multiple boxes
  // for the reps
  public getSetsArray(sets: number) {
    let intervals = Array.from(Array(sets).keys());
    return intervals;
  }

}
