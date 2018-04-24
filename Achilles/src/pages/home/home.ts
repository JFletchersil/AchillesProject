import { Component, ViewChild, ElementRef } from '@angular/core';
import { NavController } from 'ionic-angular';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import { environment } from '@app/env';
import { LocalNotifications } from '@ionic-native/local-notifications';
import { Push, PushObject, PushOptions } from '@ionic-native/push';
import { WorkoutSummaryPage } from '../workout-summary/workout-summary';
import { LoginServiceProvider } from '../../providers/login-service/login-service';
import { Storage } from '@ionic/storage';
import { LoginPage } from '../login/login';
import { AdminPage } from '../admin/admin';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage {
  names: Observable<any>;
  sessionId: string = "";
  loginPage = LoginPage;
  superuser: boolean;
  @ViewChild('containerElement') containerElement: ElementRef;

  constructor(
    public navCtrl: NavController,
    public httpClient: HttpClient,
    private localNotifications: LocalNotifications,
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
          } else {
            this._loginServiceProvider.isSuperUser(sessionId).then(result => {
              this.superuser = result;
            });
          }
          console.log("valid session for: " + sessionId);
        });

        this.sessionId = sessionId;
      }

      // Do async calls here.
    });
  }

  ionViewDidEnter(){
    function requestPermission() {
      if (!('Notification' in window)) {
        alert('Notification API not supported!');
        return;
      }
      Notification.requestPermission(function (result) {
      });
    }
    
    function nonPersistentNotification() {
      if (!('Notification' in window)) {
        alert('Notification API not supported!');
        return;
      }
      
      try {
        var notification = new Notification("Hi there - non-persistent!");
      } catch (err) {
        alert('Notification API error: ' + err);
      }
    }
    requestPermission();
    nonPersistentNotification();
  }

  goToExercises() {
    this.navCtrl.parent.select(1);
  }

  goToProgress() {
    this.navCtrl.parent.select(2);
  }

  goToAdmin() {
    this.navCtrl.push(AdminPage);
  }

  logOut() {
    this._loginServiceProvider.setSessionId('');
    this.navCtrl.setRoot(LoginPage);
  }
}
