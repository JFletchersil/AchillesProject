

/**
 * An class which holds details of an exercise to be performed.
 * @class Exercise
 * @module AppModule
 * @submodule Domain
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
 * Enum which stores the exercise type.
 * @module AppModule
 * @enum {number} ExerciseType
 */
export enum ExerciseType {
  RepsSets = 0,
  Timed = 1,
}

/**
 * An interface which holds an exercise in a format needed to communicate with the API.
 * @class completedResults
 * @module AppModule
 * @submodule Domain
 */
export interface completedResults {

  /**
   * The Id of the exercise to perform.
   * @type {string}
   * @memberof completedResults
   * @property exerciseId
   */
  exerciseId: string,

  /**
   * The number of completed reps.
   * @type {number[]}
   * @memberof completedResults
   * @property completedReps
   */
  completedReps: number[],

  /**
   * The number of completed times.
   * @type {number[]}
   * @memberof completedResults
   * @property completedTimes
   */
  completedTimes: number[],
}

/**
 * An interface which holds an exercise in a format needed to communicate with the API.
 * @class SaveExercise
 * @module AppModule
 * @submodule Domain
 */
export interface SaveExercise {
    
  /**
   * Holds a copy of the user's sessionId
   * @type {string}
   * @memberof SaveExercise
   * @property sessionId
   */
  sessionId: string;

  /**
   * Holds a copy of the result of saving an exercise.
   * @type {completedResults}
   * @memberof SaveExercise
   * @property resultViewModel
   */
  resultViewModel: completedResults;
}