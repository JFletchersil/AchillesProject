import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@app/env';


/**
 * A collection of providers used throughout the application.
 * @module AppModule
 * @submodule Providers
 * @main Providers
 */

@Injectable()

/**
 * A class which provides a range of helpful methods for the Achilles application
 * @class AchillesServiceProvider
 * @module AppModule
 * @submodule Providers
 */
export class AchillesServiceProvider {

  /**
   * Creates an instance of AchillesServiceProvider.
   * @param {HttpClient} httpClient Used to perform HTTP requests.
   * @memberof AchillesServiceProvider
   * @method constructor
   */
  constructor(public http: HttpClient) {
    //console.log('Hello AchillesServiceProvider Provider');
  }

}
