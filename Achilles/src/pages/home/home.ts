import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
//import { WorkoutSummaryPage } from '../workout-summary/workout-summary';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage {
  names : Observable<any>;

  constructor(public navCtrl: NavController, public httpClient: HttpClient) {
    this.names = this.httpClient.get('http://localhost:5000/api/values');
    this.names.subscribe(data => {
      console.log("names: ", data)
    });
  }
}
