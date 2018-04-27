import { Component, ViewChild, ElementRef } from '@angular/core';
import { NavController, DateTime } from 'ionic-angular';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import { environment } from '@app/env';
import { LocalNotifications, ILocalNotificationActionType, ELocalNotificationTriggerUnit } from '@ionic-native/local-notifications';
import { Push, PushObject, PushOptions } from '@ionic-native/push';
import { Platform } from 'ionic-angular';
import { WorkoutSummaryPage } from '../workout-summary/workout-summary';
import { LoginServiceProvider } from '../../providers/login-service/login-service';
import { Storage } from '@ionic/storage';
import { LoginPage } from '../login/login';
import { AdminPage } from '../admin/admin';
import { dateDataSortValue } from 'ionic-angular/util/datetime-util';


/**
 * 
 * 
 * @export
 * @class HomePage
 */
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


  /**
   * Creates an instance of HomePage.
   * @param {NavController} navCtrl base class for navigation controller components like Nav and Tab.
   * @param {HttpClient} httpClient Used to perform HTTP requests.
   * @param {Platform} plt provides information about the current platform.
   * @param {LocalNotifications} localNotifications provides a means to send notifications to the user's device.
   * @param {LoginServiceProvider} _loginServiceProvider A dependency injected instance of the login Service.
   * @param {Storage} storage Uses a variety of storage engines underneath, picking the best one available depending on the platform.
   * @param {NavController} navController base class for navigation controller components like Nav and Tab.
   * @memberof HomePage
   */
  constructor(
    public navCtrl: NavController,
    public httpClient: HttpClient,
    public plt: Platform,
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
          //console.log("valid session for: " + sessionId);
        });

        this.sessionId = sessionId;
      }

      // Do async calls here.
    });
  }

  /**
   * Lifecycle hook which is fired when entering a page, before it becomes the active one.
   * @memberof HomePage
   */
  ionViewDidEnter() {

    
    /**
     * @returns Requests permissions to send notifications to the user's device.
     * @memberof HomePage
     */
    function requestPermission() {
      if (!('Notification' in window)) {
        alert('Notification API not supported!');
        return;
      }
      Notification.requestPermission(function (result) {
      });
    }

      /**
     * @returns Attempts to send a non persitent notification to the user's phone.
     * @memberof HomePage
     */
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

    let isAndroid = this.plt.is('android');
    if (isAndroid) {
      let d = new Date();
      d.setDate(d.getDate()+1);
      d.setHours(8);
      d.setMinutes(0);
      d.setSeconds(0);

      let e = new Date()
      e.setDate(e.getDate()+1)
      e.setHours(18)
      e.setMinutes(0)
      e.setSeconds(0)
      this.localNotifications.schedule([{
        id: 1,
        title: "Exercise Reminder!",
        trigger:  {every: ELocalNotificationTriggerUnit.DAY, count: 40, firstAt: d},
        text: "Your daily exercises are waiting!"
      },
      {
        id: 2,
        title: "Exercise Reminder!",
        trigger:  {every: ELocalNotificationTriggerUnit.DAY, count: 40, firstAt: e},
        text: "Your daily exercises are waiting!"
      }]);

      if(environment.noteTest){
        this.localNotifications.schedule({
          id: 3,
          title: "Exercise Reminder!",
          trigger:  {at: new Date()},
          text: "Your daily exercises are waiting! - This is a dev test, this checks to make sure that dev works!"
        });
      }
    } else if (this.plt.is('core')) {
      requestPermission();
      nonPersistentNotification();
    }
  }


  /**
   * Navigates the user to the exercise page.
   * @memberof HomePage
   */
  goToExercises() {
    this.navCtrl.parent.select(1);
  }

  /**
   * Navigates the user to the statistics page.
   * @memberof HomePage
   */
  goToProgress() {
    this.navCtrl.parent.select(2);
  }


  /**
   * Navigates the user to the administration page.
   * @memberof HomePage
   */
  goToAdmin() {
    this.navCtrl.push(AdminPage);
  }


  /**
   * Removes the session Id of the user and redirects them to the login page, essentially logging them out.
   * @memberof HomePage
   */
  logOut() {
    this._loginServiceProvider.setSessionId('');
    this.navCtrl.setRoot(LoginPage);
  }
}
