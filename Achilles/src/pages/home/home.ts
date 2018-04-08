import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
//import { WorkoutSummaryPage } from '../workout-summary/workout-summary';
import { environment } from '@app/env';
import { WorkoutSummaryPage } from '../workout-summary/workout-summary';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage {
  names : Observable<any>;

  constructor(public navCtrl: NavController, public httpClient: HttpClient) {
    this.names = this.httpClient.get(environment.api + "api/values");
    this.names.subscribe(data => {
      console.log("names: ", data)
    });
  }

  goToExercises(){
    this.navCtrl.parent.select(1);
  }

  goToProgress(){
    this.navCtrl.parent.select(2);
  }
}
