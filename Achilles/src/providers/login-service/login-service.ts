import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@app/env';
import { Storage } from '@ionic/storage';
import { User } from '../../domain/user';

/*
  Generated class for the LoginServiceProvider provider.

  See https://angular.io/guide/dependency-injection for more info on providers
  and Angular DI.
*/
@Injectable()
export class LoginServiceProvider {

  constructor(public http: HttpClient, private storage: Storage) {
    console.log('Hello LoginServiceProvider Provider');
  }

  public validateSession (sessionId: string) {
    return new Promise(resolve => {
      this.http.get(environment.api + `api/Account/ValidateSession?sessionid=${sessionId}`)
        .subscribe(response => {
          resolve(response);
      }, err => {console.log('failed for some reason');});
    });
  }

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

  public register (email: string, password: string) {
    debugger;
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

  public setSessionId (sessionId: string) {
    this.storage.set('sessionId', sessionId);
  }

  public getAllUsers (sessionId: string): Promise<User[]> {
    return new Promise(resolve => {
      this.http.get(environment.api + 'api/Account/GetAllUsers?sessionId='+sessionId)
        .subscribe(response =>{
          resolve(response as User[]);
        }, err => {console.log('failed for some reason');});
    });
  }

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
        console.log(response);
      }, err => {console.log('failed for some reason');});
    });
  }

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
