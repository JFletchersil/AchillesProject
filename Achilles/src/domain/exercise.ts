export interface SaveExercise{
  sessionId: string;
  resultViewModel: completedResults;
}


/**
 * 
 * 
 * @export
 * @interface Exercise
 */
export interface Exercise {
  id: string;
  name: string;
  videoLink: string;
  exerciseType: ExerciseType;
  reps?: number;
  sets?: number;
  time?: number; // seconds
  completed: boolean;
  completedResults: completedResults;
}


/**
 * 
 * 
 * @export
 * @enum {number}
 */
export enum ExerciseType {
  RepsSets = 0,
  Timed = 1,
}


/**
 * 
 * 
 * @export
 * @interface completedResults
 */
export interface completedResults {
  exerciseId: string,
  completedReps: number[],
  completedTimes: number[],
}
