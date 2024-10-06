import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {TrainResult} from "../../view-models/train-result";

@Injectable({
  providedIn: 'root'
})
export class RecommenderService {
  constructor(private http: HttpClient) {
  }

  loadModel() {
    return this.http.put<number>('api/recommender/load', null);
  }

  trainModel(synthSize: number = 2): Observable<any> {
    return this.http.put<TrainResult>(`api/recommender/train?synthSize=${synthSize}`, null);
  }
}
