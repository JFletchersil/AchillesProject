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

@NgModule({
  declarations: [
    MyApp,
    HomePage,
    TabsPage,
    WorkoutPage,
    WorkoutSummaryPage,
    StatisticsPage,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    IonicModule.forRoot(MyApp),
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp,
    HomePage,
    TabsPage,
    WorkoutPage,
    WorkoutSummaryPage,
    StatisticsPage,
  ],
  providers: [
    StatusBar,
    SplashScreen,
    {provide: ErrorHandler, useClass: IonicErrorHandler},
    AchillesServiceProvider,
  ]
})
export class AppModule {}
