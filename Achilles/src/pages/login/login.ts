import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { LoginServiceProvider } from '../../providers/login-service/login-service';
import { HomePage } from '../home/home';

/**
 * Generated class for the LoginPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-login',
  templateUrl: 'login.html',
})
export class LoginPage {

  homePage = HomePage;

  constructor(
    public navCtrl: NavController, 
    public navParams: NavParams,
    private _loginServiceProvider: LoginServiceProvider) {
      

  }

  logIn() {
    this._loginServiceProvider.login("Gmandam@mykolab.com", "Test123!").then((value) => {
      this._loginServiceProvider.setSessionId(value);
    }).then(() => {
      this.navCtrl.setRoot(this.homePage);
    });
  }
  ionViewDidLoad() {
    console.log('ionViewDidLoad LoginPage');
  }

}
