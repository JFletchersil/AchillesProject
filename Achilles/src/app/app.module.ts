import { NgModule, ErrorHandler } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { IonicApp, IonicModule, IonicErrorHandler } from 'ionic-angular';
import { MyApp } from './app.component';

import { HomePage } from '../pages/home/home';
import { TabsPage } from '../pages/tabs/tabs';
import { WorkoutPage } from '../pages/workout/workout';
import { WorkoutSummaryPage } from '../pages/workout-summary/workout-summary';
import { StatisticsPage } from '../pages/statistics/statistics';

import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { AchillesServiceProvider } from '../providers/achilles-service/achilles-service';

import { HttpClientModule } from '@angular/common/http';
import { HomePageModule } from '../pages/home/home.module';
import { TabsPageModule } from '../pages/tabs/tabs.module';
import { WorkoutPageModule } from '../pages/workout/workout.module';
import { WorkoutSummaryPageModule } from '../pages/workout-summary/workout-summary.module';
import { StatisticsPageModule } from '../pages/statistics/statistics.module';
import { ExerciseServiceProvider } from '../providers/exercise-service/exercise-service';
import { AdditionalExerciseListComponent } from '../components/additional-exercise-list/additional-exercise-list';
import { environment } from '@app/env';

import { IonicStorageModule } from '@ionic/storage';
import { LoginServiceProvider } from '../providers/login-service/login-service';
import { LoginPage } from '../pages/login/login';
import { LoginPageModule } from '../pages/login/login.module';

@NgModule({
  declarations: [
    MyApp,
    AdditionalExerciseListComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    IonicModule.forRoot(MyApp),
    IonicStorageModule.forRoot(),
    HomePageModule,
    TabsPageModule,
    WorkoutPageModule,
    WorkoutSummaryPageModule,
    StatisticsPageModule,
    LoginPageModule,
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp,
    HomePage,
    TabsPage,
    WorkoutPage,
    WorkoutSummaryPage,
    StatisticsPage,
    LoginPage,
  ],
  providers: [
    StatusBar,
    SplashScreen,
    { provide: ErrorHandler, useClass: IonicErrorHandler },
    AchillesServiceProvider,
    ExerciseServiceProvider,
    LoginServiceProvider,
  ]
})
export class AppModule { }
