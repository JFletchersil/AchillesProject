import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
//import { WorkoutSummaryPage } from '../workout-summary/workout-summary';
import { environment } from '@app/env';
import { WorkoutSummaryPage } from '../workout-summary/workout-summary';
import { LoginServiceProvider } from '../../providers/login-service/login-service';
import { Storage } from '@ionic/storage';
import { LoginPage } from '../login/login';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage {
  names: Observable<any>;
  sessionId: string = "";
  loginPage = LoginPage;

  constructor(
    public navCtrl: NavController,
    public httpClient: HttpClient,
    private _loginServiceProvider: LoginServiceProvider,
    private storage: Storage,
    private navController: NavController) {
    
    storage.get('sessionId').then((sessionId) => {
      if (!sessionId) {
        this.navController.setRoot(this.loginPage);
      } else {
        this._loginServiceProvider.validateSession(sessionId).then((isValidSessionId) => {
          if (!isValidSessionId) {
            this.navController.setRoot(this.loginPage);
          }
          console.log("valid session for: " + sessionId);
        });
  
        this.sessionId = sessionId;
      }


      // Do async calls here.
    });


  }

  goToExercises() {
    this.navCtrl.parent.select(1);
  }

  goToProgress() {
    this.navCtrl.parent.select(2);
  }
}
