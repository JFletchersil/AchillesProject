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
export class StatisticsServiceProvider {

  constructor(public http: HttpClient) {}

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
