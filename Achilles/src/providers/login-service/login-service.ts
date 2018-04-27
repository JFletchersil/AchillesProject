import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@app/env';
import { Storage } from '@ionic/storage';
import { User } from '../../domain/user';

/**
 * 
 * 
 * @export
 * @class LoginServiceProvider
 */
@Injectable()
export class LoginServiceProvider {


  /**
   * Creates an instance of LoginServiceProvider.
   * @param {HttpClient} http Used to perform HTTP requests.
   * @param {Storage} storage Uses a variety of storage engines underneath, picking the best one available depending on the platform.
   * @memberof LoginServiceProvider
   */
  constructor(public http: HttpClient, private storage: Storage) {
    //console.log('Hello LoginServiceProvider Provider');
  }

  /**
   * Validates a user's session based on their current session Id.
   * 
   * @param {string} sessionId The user's session Id from local storage.
   * @returns A promise which indicates whether the given session is valid or not.
   * @memberof LoginServiceProvider
   */
  public validateSession (sessionId: string) {
    return new Promise(resolve => {
      this.http.get(environment.api + `api/Account/ValidateSession?sessionid=${sessionId}`)
        .subscribe(response => {
          resolve(response);
      }, err => {console.log('failed for some reason');});
    });
  }

  /**
   * Attempts to log the current user in using the provided email and password.
   * 
   * @param {string} email The email address used in the authentication attempt.
   * @param {string} password The password used in the authentication attempt.
   * @returns {Promise<string>} A promise which indicates whether the login was successful or not.
   * @memberof LoginServiceProvider
   */
  public login (email: string, password: string): Promise<string> {
    return new Promise(resolve => {
      this.http.post(environment.api + 'api/Account/Login', {
        "Email": email,
        "Password": password,
      }).subscribe(response =>{
        resolve(response as string);
      }, err => {resolve("failed")});
    });
  }

  /**
   * Attempts to register the current user using the provided email and password.
   * 
   * @param {string} email The email address used in the authentication attempt.
   * @param {string} password The password used in the authentication attempt.
   * @returns {Promise<string>} A promise which indicates whether the registration was successful or not.
   * @memberof LoginServiceProvider
   */
  public register (email: string, password: string) {
    return new Promise(resolve => {
      this.http.post(environment.api + 'api/Account/Register', {
        "Email": email,
        "Password": password,
        "ConfirmPassword": password,
      }).subscribe(response => {
        resolve(response as boolean);
      }, err => {
        resolve(false);
      });
    });
  }

  /**
   * 
   * Sets the session Id in local storage to the given session.
   * @param {string} sessionId A valid session Id which must correspond to a user account.
   * @memberof LoginServiceProvider
   */
  public setSessionId (sessionId: string) {
    this.storage.set('sessionId', sessionId);
  }

  /**
   * 
   * Gets a list of all users for display purposes in the admin section.
   * @param {string} sessionId A valid session Id which must correspond to an Admin account.
   * @returns {Promise<User[]>} A promise which holds details of users.
   * @memberof LoginServiceProvider
   */
  public getAllUsers (sessionId: string): Promise<User[]> {
    return new Promise(resolve => {
      this.http.get(environment.api + 'api/Account/GetAllUsers?sessionId='+sessionId)
        .subscribe(response =>{
          resolve(response as User[]);
        }, err => {console.log('failed for some reason');});
    });
  }

  /**
   * 
   * Makes a request to the API to update an existing user's details.
   * @param {User} user The user object to edit in the API.
   * @param {string} sessionId A valid session Id which must correspond to an admin account.
   * @returns 
   * @memberof LoginServiceProvider
   */
  public editUser (user: User, sessionId: string) {
    // console.log({
    //   sessionId: sessionId,
    //   userModel: user,
    // });
    return new Promise(resolve => {
      this.http.post(environment.api + 'api/Account/Edit', {
        sessionId: sessionId,
        userModel: user,
      }).subscribe(response =>{
       // console.log(response);
      }, err => {console.log('failed for some reason');});
    });
  }

  /**
   * Tests whether a given session Id belongs to a valid admin account.
   * 
   * @param {string} sessionId A valid session Id which must correspond to an admin account.
   * @returns {Promise<boolean>} A promise which returns true if the user is an admin account.
   * @memberof LoginServiceProvider
   */
  public isSuperUser(sessionId:string): Promise<boolean>{
    return new Promise(resolve =>{
      this.http.get(environment.api+'api/Account/IsAdmin?sessionId='+sessionId)
      .subscribe(response =>{
        if(response) resolve(true);
        else resolve(false);
      });
    });
  }
}
