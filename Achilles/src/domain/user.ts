
/**
 * Holds details about a user.
 * @class User
 * @module AppModule
 * @submodule Domain
 */
export interface User {

  /**
   * The id of the user.
   * @type {string}
   * @memberof User
   * @property id
   */
  id: string,

  /**
   * The username of the user.
   * @type {string}
   * @memberof User
   * @property userName
   */
  userName: string,
  
  /**
   * The email of the user.
   * @type {string}
   * @memberof User
   * @property email
   */
  email: string,

  /**
   * The progress level of the user.
   * @type {string}
   * @memberof User
   * @property userLevel
   */
  userLevel: string,
}