import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Exercise, ExerciseType } from '../../domain/exercise';
import { Exercises } from '../../domain/exercises';

@Injectable()
export class ExerciseServiceProvider {

  constructor(public http: HttpClient) {
    console.log('Hello ExerciseServiceProvider Provider');
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
