import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  public categories: string[] = []
  constructor(private http: HttpClient) {
  }

  refreshCategories() {
    this.http.get<CategoryViewModel[]>("http://localhost:5228/api/categories/get-all-categories").subscribe({
      next: response => {
        this.categories = response.map(cat => cat.name);
      }
    , error: error => {
        alert(JSON.stringify(error));
      }
    });
  }
}

interface CategoryViewModel {
  id: string
  name: string
  description: string
}
