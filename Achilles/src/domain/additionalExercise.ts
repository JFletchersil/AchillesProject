
/**
 * A collection of domain classes used throughout the application.
 * @module AppModule
 * @submodule Domain
 * @main Domain
 */

/**
 * An interface which holds the name and description of a given Additional Exercise.
 * @class AdditionalExercise
 * @module AppModule
 * @submodule Domain
 */
export interface AdditionalExercise{
  
  /**
   * Holds the title of the exercise
   * @type {string}
   * @memberof AdditionalExercise
   * @property exercise
   */
  exercise: string,
  
  /**
   * Holds the description of the exercise
   * @type {string}
   * @memberof AdditionalExercise
   * @property description
   */
  description: string
}
