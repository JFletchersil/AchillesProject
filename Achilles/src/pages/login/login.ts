import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { LoginServiceProvider } from '../../providers/login-service/login-service';
import { HomePage } from '../home/home';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@IonicPage()
@Component({
  selector: 'page-login',
  templateUrl: 'login.html',
})

/**
 * The page responsible for managing the LoginPage.
 * @class LoginPage
 * @module AppModule
 * @submodule Pages
 */
export class LoginPage {

  /**
   * Holds a copy of the tab bar elements
   * @type {*}
   * @memberof LoginPage
   * @property tabBarElement
   */
  tabBarElement: any;
  
  /**
   * Holds a reference to the HomePage
   * @type {HomePage}
   * @memberof LoginPage
   * @property homePage
   */
  homePage = HomePage;

  /**
   * Indicates whether the register form should be displayed instead of the login form. 
   * Defaults to false, indicating the login form should be shown.
   * @type {boolean}
   * @memberof LoginPage
   * @property showRegister
   */
  showRegister = false;

  /**
   * A form group which holds all fields for the login form.
   * @type {FormGroup}
   * @memberof LoginPage
   * @property loginForm
   */
  loginForm: FormGroup;

  /**
   * A form group which holds all fields for the register form.
   * @type {FormGroup}
   * @memberof LoginPage
   * @property registerForm
   */
  registerForm: FormGroup;

  /**
   * A boolean indicating whether the last login attempt failed.
   * Defaults to false, indicating no failed attempt.
   * @type {boolean}
   * @memberof LoginPage
   * @property loginFailed
   */
  loginFailed = false;

  /**
   * A boolean indicating whether the last registration attempt failed.
   * Defaults to false, indicating no failed attempt.
   * @type {boolean}
   * @memberof LoginPage
   * @property registerFailed
   */
  registerFailed = false;

  /**
   * Creates an instance of LoginPage.
   * @param {NavController} navCtrl base class for navigation controller components like Nav and Tab.
   * @param {NavParams} navParams class for navigation controller parameters in Ionic.
   * @param {LoginServiceProvider} _loginServiceProvider A dependency injected instance of the login Service.
   * @param {FormBuilder} fb syntactic sugar that shortens the new FormGroup(), new FormControl(), and new FormArray() boilerplate in Angular
   * @memberof LoginPage
   * @method constructor
   */
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

  /**
   * Attempts to authorize the user, given a valid email and password. Redirects the user to the homepage on a successful login.
   * @memberof LoginPage
   * @method logIn
   */
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

  /**
   * Attempts to register a new user given an email and password matching set criteria.
   * Redirects the user to the login screen if successful.
   * @memberof LoginPage
   * @method register
   */
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

  /**
   * Toggles whether to show the registration form or the login form.
   * @memberof LoginPage
   * @method toggleRegister
   */
  toggleRegister() {
    this.showRegister = !this.showRegister;
  }

    // this._loginServiceProvider.login("Gmandam@mykolab.com", "Test123!").then((value) => {
    //   this._loginServiceProvider.setSessionId(value);
    // }).then(() => {
    //   this.navCtrl.setRoot(this.homePage);
    // });

  /**
   * Lifecycle hook which is fired only when a view is stored in memory.
   * @memberof LoginPage
   * @method ionViewDidLoad
   */
  ionViewDidLoad() {
    //console.log('ionViewDidLoad LoginPage');
  }

  /**
   * Lifecycle hook which is fired when entering a page, before it becomes the active one.
   * @memberof LoginPage
   * @method ionViewWillEnter
   */
  ionViewWillEnter() {
    this.tabBarElement.style.display = 'none';
  }

  /**
   * Lifecycle hook which is fired when you leave a page, before it stops being the active one.
   * @memberof LoginPage
   * @method ionViewWillLeave
   */
  ionViewWillLeave() {
    this.tabBarElement.style.display = 'flex';
  }

}
