export interface Exercise {
  id: string;
  name: string;
  videoLink: string;
  exerciseType: ExerciseType;
  reps?: number;
  sets?: number;
  time?: number; // seconds
  completed: boolean;
}

export enum ExerciseType {
  RepsSets = 0,
  Timed = 1,
}