import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  public categoryNames: string[] = []
  public categories: Category[] = []

  constructor(private http: HttpClient) {
  }

  refreshCategories() {
    this.http.get<Category[]>("http://localhost:5228/api/categories/get-all-categories").subscribe({
      next: response => {
        this.categories = response
        this.categoryNames = response.map(cat => cat.name);
      }
    });
  }

  getAllCategories() {
    return this.http.get<Category[]>(`api/categories/get-all-categories`);
  }

  getFavoriteCategories(userId: string) {
    return this.http.get<Category[]>(`api/users/get-favorite-categories/${userId}`);
  }

  updateFavoriteCategories(userId: string, favoriteCategoryIds: string[]) {
    return this.http.put(`api/users/update-favorite-categories/${userId}`, favoriteCategoryIds);
  }
}

interface Category {
  id: string
  name: string
  description: string
}
