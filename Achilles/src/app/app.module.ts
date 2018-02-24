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

@NgModule({
  declarations: [
    MyApp
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    IonicModule.forRoot(MyApp),
    HomePageModule,
    TabsPageModule,
    WorkoutPageModule,
    WorkoutSummaryPageModule,
    StatisticsPageModule
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
    { provide: ErrorHandler, useClass: IonicErrorHandler },
    AchillesServiceProvider,
    ExerciseServiceProvider,
  ]
})
export class AppModule { }
