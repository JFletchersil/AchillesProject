import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@app/env';
import { Storage } from '@ionic/storage';

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
      }, err => {console.log('failed for some reason');});
    });
  }

  public setSessionId (sessionId: string) {
    this.storage.set('sessionId', sessionId);
  }
}
