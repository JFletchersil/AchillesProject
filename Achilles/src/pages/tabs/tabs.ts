import { Component } from '@angular/core';

import { HomePage } from '../home/home';
import { WorkoutSummaryPage } from '../workout-summary/workout-summary';
import { StatisticsPage } from '../statistics/statistics';

@Component({
  templateUrl: 'tabs.html'
})
export class TabsPage {

  tab1Root = HomePage;
  tab2Root = WorkoutSummaryPage;
  tab3Root = StatisticsPage;

  constructor() {

  }
}
