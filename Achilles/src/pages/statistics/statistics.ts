import { Component, ViewChild, ElementRef } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { Storage } from '@ionic/storage';
import { StatisticsServiceProvider } from '../../providers/statistics-service/statistics-service';
import { LoginPage } from '../login/login';
import { LoginServiceProvider } from '../../providers/login-service/login-service';
import { Statistics } from 'domain/statistics';
import { Chart } from 'chart.js'

/**
 * Generated class for the StatisticsPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-statistics',
  templateUrl: 'statistics.html',
})
export class StatisticsPage {

  @ViewChild('barCanvas') barCanvas: ElementRef;

  stats: Statistics;
  progress: number;

  barChart: any;
  loadedBar: boolean;
  loadRefresh: string = "Load";

  constructor(public navCtrl: NavController, public navParams: NavParams, 
    private loginProvider: LoginServiceProvider, private statisticsProvider: StatisticsServiceProvider, 
    private storage: Storage) {
  }

  ionViewDidEnter(){
    this.onStartReload();
  }

  updateProgressBar(){
    let completionDate = this.stats.getApproximateEndDate() as any;
    let startDate = this.stats.getEarlierstDate() as any;
    let currentDate = new Date() as any;
    this.progress = Math.round(
                      (Math.abs(currentDate - startDate)/ Math.abs(completionDate - startDate))*100
                    )
  }

  updateBarGraph(){
    this.loadedBar = true;
    let data = Array<number>();
    this.loadRefresh = "Refresh";
    data.push(this.stats.getAverageSuccessOfExercies('Heel Raises'));
    data.push(this.stats.getAverageSuccessOfExercies('Towel Stretch'));
    data.push(this.stats.getAverageSuccessOfExercies('Step Ups'));
    data.push(this.stats.getAverageSuccessOfExercies('Standing Calf Stretch'));
    this.barChart = new Chart(this.barCanvas.nativeElement, {
      type: 'bar',
      data: {
          labels: ["HR", "TS", "SU", "SCS"],
          datasets: [{
              label: 'Success Rate %',
              data: data,
              backgroundColor: [
                  'rgba(255, 99, 132, 0.2)',
                  'rgba(54, 162, 235, 0.2)',
                  'rgba(255, 206, 86, 0.2)',
                  'rgba(75, 192, 192, 0.2)'
              ],
              borderColor: [
                  'rgba(255,99,132,1)',
                  'rgba(54, 162, 235, 1)',
                  'rgba(255, 206, 86, 1)',
                  'rgba(75, 192, 192, 1)'
              ],
              borderWidth: 1
          }]
      },
      options: {
          scales: {
              yAxes: [{
                  ticks: {
                      beginAtZero:true
                  }
              }]
          },
          maintainAspectRatio: false,
          responsive: true
      }

    });
  }

  onStartReload(){
    this.storage.get('sessionId').then(sessionId =>{
      if (!sessionId) {
        this.navCtrl.setRoot(LoginPage);
      } else {
        this.loginProvider.validateSession(sessionId).then((isValidSessionId) => {
          if (!isValidSessionId) {
            this.navCtrl.setRoot(LoginPage);
          }else{
            this.statisticsProvider.getStatistics(sessionId).then(results =>{
              this.stats = results;
            }).then(() =>{
              this.updateProgressBar();
              this.updateBarGraph();
            });
          }
          console.log("valid session for: " + sessionId);
        })
        }
    });
  }
}
