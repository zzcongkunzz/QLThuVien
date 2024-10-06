import {Component} from '@angular/core';
import {RecommenderService} from "../../../../../services/recommender/recommender.service";
import {JsonPipe, NgIf} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {TrainResult} from "../../../../../view-models/train-result";

@Component({
  selector: 'app-recommender-manage',
  standalone: true,
  imports: [
    NgIf,
    JsonPipe,
    FormsModule
  ],
  templateUrl: './recommender-manage.component.html',
  styleUrl: './recommender-manage.component.scss'
})
export class RecommenderManageComponent {
  synthSize: number = 2;
  trainResult: TrainResult | any;
  modelSize: number | undefined | string;

  constructor(private recommenderService: RecommenderService) {
  }

  loadModel() {
    this.recommenderService.loadModel().subscribe({
      next: data => {
        this.modelSize = data
      },
      error: err => {
        this.modelSize = JSON.stringify(err.message);
      }
    })
  }

  trainModel() {
    this.trainResult = "Đang huấn luyện...."
    this.recommenderService.trainModel(this.synthSize).subscribe({
      next: data => {
        this.trainResult = data
      },
      error: err => {
        this.trainResult = err
      },
    })
  }

  invalidLoad() {
    return typeof (this.modelSize) === "string" || this.modelSize === undefined;
  }
}
