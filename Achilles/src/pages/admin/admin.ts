import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { LoginServiceProvider } from '../../providers/login-service/login-service';
import { Storage } from '@ionic/storage';
import { LoginPage } from '../login/login';
import { User } from '../../domain/user';

/**
 * 
 * @export
 * @class AdminPage
 */
@IonicPage()
@Component({
  selector: 'page-admin',
  templateUrl: 'admin.html',
})
export class AdminPage {

  tabBarElement: any;

  loginPage = LoginPage;
  sessionId: string = "";
  users: User[] = [];


  /**
   * Creates an instance of AdminPage.
   * @param {NavController} navCtrl base class for navigation controller components like Nav and Tab.
   * @param {NavParams} navParams class for navigation controller parameters in Ionic.
   * @param {LoginServiceProvider} _loginServiceProvider A dependency injected instance of the login Service.
   * @param {Storage} storage Uses a variety of storage engines underneath, picking the best one available depending on the platform.
   * @param {NavController} navController base class for navigation controller components like Nav and Tab.
   * @memberof AdminPage
   */
  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    private _loginServiceProvider: LoginServiceProvider,
    private storage: Storage,
    private navController: NavController) {

    this.tabBarElement = document.querySelector('.tabbar.show-tabbar');

    storage.get('sessionId').then((sessionId) => {
      if (!sessionId) {
        this.navController.setRoot(this.loginPage);
      } else {
        this._loginServiceProvider.validateSession(sessionId).then((isValidSessionId) => {
          if (!isValidSessionId) {
            this.navController.setRoot(this.loginPage);
          }
          //console.log("valid session for: " + sessionId);
        });

        this.sessionId = sessionId;
        _loginServiceProvider.getAllUsers(this.sessionId).then((response) => {
          this.users = response;
          //console.log(response);
          //console.log(this.users);
        }) ;
      }


      // Do async calls here.
    });


  }


  /**
   * Lifecycle hook which is fired only when a view is stored in memory.
   * @memberof AdminPage
   */
  ionViewDidLoad() {
    //console.log('ionViewDidLoad AdminPage');
  }


  /**
   * Provides a custom implementation of the Angular trackBy method, which keeps the index of for loops consistent.
   * @param {number} index The index.
   * @param {*} obj The track by object or event to manipulate.
   * @returns {*} index.
   * @memberof Adminpage
   */
  customTrackBy(index: number, obj:any): any {
    return index;
  }


  /**
   * Saves the edited user's details, assuming a valid sessionId for the logged in user.
   * 
   * @param {User} user the user in which to apply changes.
   * @memberof AdminPage
   */
  saveData(user: User) {
    this._loginServiceProvider.editUser(user, this.sessionId);
  }

  /**
   * 
   * Lifecycle hook which is fired when entering a page, before it becomes the active one.
   * @memberof AdminPage
   */
  ionViewWillEnter() {
    this.tabBarElement.style.display = 'none';
  }

  /**
   * 
   * Lifecycle hook which is fired when you leave a page, before it stops being the active one.
   * @memberof AdminPage
   */
  ionViewWillLeave() {
    this.tabBarElement.style.display = 'flex';
  }
}
