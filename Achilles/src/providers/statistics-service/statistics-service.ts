import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Exercise, ExerciseType, SaveExercise } from '../../domain/exercise';
import { Exercises } from '../../domain/exercises';
import { environment } from '@app/env';
import { AdditionalExercises } from '../../domain/additionalExercises';
import { success } from 'domain/success';
import { Statistics } from '../../domain/statistics';
import { statistic } from '../../domain/statistic';


@Injectable()
/**
 * A class which provides a range of helpful methods for statistics
 * @class StatisticsServiceProvider
 * @module AppModule
 * @submodule Providers
 */
export class StatisticsServiceProvider {

  /**
   * Creates an instance of StatisticsServiceProvider.
   * @param {HttpClient} http Used to perform HTTP requests.
   * @memberof StatisticsServiceProvider
   * @method constructor
   */
  constructor(public http: HttpClient) {}

  /**
   * Creates the full statistics object for the current user.
   * 
   * @param {string} sessionID A valid session Id corresponding to the logged in user.
   * @returns {Promise<Statistics>} A promise which holds the statistics for the logged in user.
   * @memberof StatisticsServiceProvider
   * @method getStatistics
   */
  public getStatistics(sessionID : string) : Promise<Statistics>{
    return new Promise(res =>{
      this.http.get(environment.api + 'api/Statistics/GetStatistics?sessionId='+sessionID)
        .subscribe(response =>{
          let allStatistics = new Statistics();
          for(let i in response){
            let result = response[i];
            allStatistics.addStatistics(
              new statistic(result['date'],
                            JSON.parse(result['results']),
                            result['exercise'],
                            result['reps'],
                            result['sets'],
                            result['time']
                          )
                        );
          }
          res(allStatistics);
        });
    });
  }

}
