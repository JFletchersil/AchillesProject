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

  // I'm simply returning a hard coded object here as
  // a placeholder until the api is ready.
  public getExercise() {
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

  // I'm simply returning a hard coded object here as
  // a placeholder until the api is ready.
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
}
