import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { LoginServiceProvider } from '../../providers/login-service/login-service';
import { Storage } from '@ionic/storage';
import { LoginPage } from '../login/login';
import { User } from '../../domain/user';
/**
 * Generated class for the AdminPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-admin',
  templateUrl: 'admin.html',
})
export class AdminPage {

  loginPage = LoginPage;
  sessionId: string = "";
  users: User[] = [];

  constructor(
    public navCtrl: NavController, 
    public navParams: NavParams,
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
        _loginServiceProvider.getAllUsers(this.sessionId).then((response) => {
          this.users = response;
          console.log(response);
          console.log(this.users);
        }) ;
      }


      // Do async calls here.
    });

    
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad AdminPage');
  }

  customTrackBy(index: number, obj:any): any {
    return index;
  }

  saveData(user: User) {
    this._loginServiceProvider.editUser(user, this.sessionId);
  }
}
