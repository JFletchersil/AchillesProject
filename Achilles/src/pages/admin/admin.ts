import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { LoginServiceProvider } from '../../providers/login-service/login-service';
import { Storage } from '@ionic/storage';
import { LoginPage } from '../login/login';
import { User } from '../../domain/user';

@IonicPage()
@Component({
  selector: 'page-admin',
  templateUrl: 'admin.html',
})

/**
 * A collection of pages representing the view of the application
 * @module AppModule
 * @submodule Pages
 * @main Pages
 */

/**
 * The page responsible for managing the AdminPage.
 * @class AdminPage
 * @module AppModule
 * @submodule Pages
 */
export class AdminPage {

  /**
   * A collection of elements representing all the possible states of the bar.
   * @type {*}
   * @memberof AdminPage
   * @property tabBarElement
   */
  tabBarElement: any;

  /**
   * A reference to the Login Page.
   * @type {LoginPage}
   * @memberof AdminPage
   * @property loginPage
   */
  loginPage = LoginPage;

  /**
   * The users current sessionId.
   * @property sessionId
   * @type {string}
   * @memberof AdminPage
   */
  sessionId: string = "";

  /**
   * The users from the database that is being operated on.
   * @property users
   * @type {User[]}
   * @memberof AdminPage
   */
  users: User[] = [];


  /**
   * Creates an instance of AdminPage.
   * @param {NavController} navCtrl base class for navigation controller components like Nav and Tab.
   * @param {NavParams} navParams class for navigation controller parameters in Ionic.
   * @param {LoginServiceProvider} _loginServiceProvider A dependency injected instance of the login Service.
   * @param {Storage} storage Uses a variety of storage engines underneath, picking the best one available depending on the platform.
   * @param {NavController} navController base class for navigation controller components like Nav and Tab.
   * @method constructor
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
   * Provides a custom implementation of the Angular trackBy method, which keeps the index of for loops consistent.
   * @param {number} index The index.
   * @param {Object} obj The track by object or event to manipulate.
   * @returns {number} The index.
   * @memberof Adminpage
   * @method customTrackBy
   */
  customTrackBy(index: number, obj: Object): number {
    return index;
  }


  /**
   * Saves the edited user's details, assuming a valid sessionId for the logged in user.
   * @method saveData
   * @param {User} user the user in which to apply changes.
   * @memberof AdminPage
   */
  saveData(user: User) {
    this._loginServiceProvider.editUser(user, this.sessionId);
  }

  /**
   * Lifecycle hook which is fired when entering a page, before it becomes the active one.
   * @memberof AdminPage
   * @method ionViewWillEnter
   */
  ionViewWillEnter() {
    this.tabBarElement.style.display = 'none';
  }

  /**
   * Lifecycle hook which is fired when you leave a page, before it stops being the active one.
   * @memberof AdminPage
   * @method ionViewWillLeave
   */
  ionViewWillLeave() {
    this.tabBarElement.style.display = 'flex';
  }
}
