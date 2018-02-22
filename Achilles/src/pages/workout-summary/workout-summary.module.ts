import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { WorkoutSummaryPage } from './workout-summary';

@NgModule({
  declarations: [
    WorkoutSummaryPage,
  ],
  imports: [
    IonicPageModule.forChild(WorkoutSummaryPage),
  ],
})
export class WorkoutSummaryPageModule {}
