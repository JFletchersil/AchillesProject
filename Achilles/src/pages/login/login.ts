import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { LoginServiceProvider } from '../../providers/login-service/login-service';
import { HomePage } from '../home/home';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

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

  tabBarElement: any;

  homePage = HomePage;
  showRegister = false;
  loginForm: FormGroup;
  registerForm: FormGroup;

  loginFailed = false;
  registerFailed = false;

  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    private _loginServiceProvider: LoginServiceProvider,
    private fb: FormBuilder) {

      this.tabBarElement = document.querySelector('.tabbar.show-tabbar');

      this.loginForm = this.fb.group({
        email: ['', Validators.required],
        password: ['', Validators.required],
      });

      this.registerForm = this.fb.group({
        email: ['', [Validators.required, Validators.email] ],
        password: ['', [Validators.required, Validators.minLength(6)] ],
      });
  }

  logIn() {
    this.loginFailed = false;

    if(this.loginForm.valid) {
      const email = this.loginForm.get('email').value;
      const password = this.loginForm.get('password').value;

      this._loginServiceProvider.login(email, password).then((value) => {
        if(value == 'failed') {
          this.loginFailed = true;
        } else {
          this._loginServiceProvider.setSessionId(value);
        }

      })
      .then(() => {
        if(!this.loginFailed) {
          this.navCtrl.setRoot(this.homePage);
        }
      });
    }
  }

  register() {
    this.registerFailed = false;

    if(this.registerForm.valid) {
      const email = this.registerForm.get('email').value;
      const password = this.registerForm.get('password').value;

      this._loginServiceProvider.register(email, password).then((value) => {
        if(!value) {
          this.registerFailed = true;
        } else {
          this.navCtrl.setRoot(LoginPage);
        }
      });
    }
  }

  toggleRegister() {
    this.showRegister = !this.showRegister;
  }

    // this._loginServiceProvider.login("Gmandam@mykolab.com", "Test123!").then((value) => {
    //   this._loginServiceProvider.setSessionId(value);
    // }).then(() => {
    //   this.navCtrl.setRoot(this.homePage);
    // });

  ionViewDidLoad() {
    //console.log('ionViewDidLoad LoginPage');
  }

  ionViewWillEnter() {
    this.tabBarElement.style.display = 'none';
  }

  ionViewWillLeave() {
    this.tabBarElement.style.display = 'flex';
  }

}
