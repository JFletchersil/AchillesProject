import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Exercise, ExerciseType } from '../../domain/exercise';
import { Exercises } from '../../domain/exercises';
import { EnvConfigurationProvider } from "gl-ionic2-env-configuration";
import { ITestAppEnvConfiguration } from "../../env-configuration/ITestAppEnvConfiguration";
import { AdditionalExercises } from '../../domain/additionalExercises';

@Injectable()
export class ExerciseServiceProvider {

  config: ITestAppEnvConfiguration;

  constructor(
    public http: HttpClient,
    private envConfiguration: EnvConfigurationProvider<ITestAppEnvConfiguration>) {

    this.config = envConfiguration.getConfig();
  }

  getAdditionalExercises(stage : number) : Promise<AdditionalExercises>{
    return new Promise(res => {
      this.http.get(this.config.api + 'api/Exercises/additional/' + stage).subscribe(exercises =>{
        res(exercises as AdditionalExercises);
      }, err => {console.log('Cannot find additional exercises"');});
    });
  }

  public getExercises(): Promise<Exercises> {
    return new Promise<Exercises>((resolve, reject) => {
      this.http.get(this.config.api + 'api/Exercises/GetDailyExercises').subscribe(exercises =>{
        console.log(exercises);
        resolve(exercises as Exercises);
      }, err => {console.log('Cannot find additional exercises"');});
    });
  }

  // Generates an array of numbers from 0 to maxRepsOrSeconds
  // For use in a select box to select the time taken
  public getSelectBoxIntervals(maxRepsOrSeconds: number) {
    let intervals = Array.from(Array(maxRepsOrSeconds+1).keys());
    return intervals;
  }

  // There's no way to simply use a for loop of numbers in *ngFor
  // So I've had to make an array here purely to display multiple boxes for the reps
  public getSetsArray(sets: number) {
    let intervals = Array.from(Array(sets).keys());
    return intervals;
  }

}
