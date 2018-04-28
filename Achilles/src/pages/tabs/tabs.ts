import { Component } from '@angular/core';

import { HomePage } from '../home/home';
import { WorkoutSummaryPage } from '../workout-summary/workout-summary';
import { StatisticsPage } from '../statistics/statistics';
import { environment } from '@app/env';


@Component({
  templateUrl: 'tabs.html'
})

/**
 * The page responsible for managing the TabsPage
 * @class TabsPage
 * @module AppModule
 * @submodule Pages
 */
export class TabsPage {

  /**
   * Holds a reference to the HomePage.
   * @type {HomePage}
   * @memberof TabsPage
   * @property tab1Root
   */
  tab1Root = HomePage;

  /**
   * Holds a reference to the WorkoutSummaryPage.
   * @type {WorkoutSummaryPage}
   * @memberof TabsPage
   * @property tab2Root
   */
  tab2Root = WorkoutSummaryPage;

  /**
   * Holds a reference to the StatisticsPage.
   * @type {StatisticsPage}
   * @memberof TabsPage
   * @property tab3Root
   */
  tab3Root = StatisticsPage;

  /**
   * Creates an instance of TabsPage.
   * @memberof TabsPage
   * @method constructor.
   */
  constructor() {
  }
}
