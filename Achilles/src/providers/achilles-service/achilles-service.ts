import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@app/env';


/**
 * 
 * 
 * @export
 * @class AchillesServiceProvider
 */
@Injectable()
export class AchillesServiceProvider {

  /**
   * Creates an instance of AchillesServiceProvider.
   * @param {HttpClient} httpClient Used to perform HTTP requests.
   * @memberof AchillesServiceProvider
   */
  constructor(public http: HttpClient) {
    //console.log('Hello AchillesServiceProvider Provider');
  }

}
