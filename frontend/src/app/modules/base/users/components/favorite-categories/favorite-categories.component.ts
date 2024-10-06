import {Component, OnInit} from '@angular/core';
import {CategoryService} from "../../../../../services/category/category.service";
import {Router} from "@angular/router";
import {Category} from "../../../../../view-models/category";
import {User} from "../../../../../view-models/user";
import {AuthService} from "../../../../../services/auth/auth.service";
import {CategoryCheckboxComponent} from "../category-checkbox/category-checkbox.component";

@Component({
  selector: 'app-favorite-categories',
  standalone: true,
  imports: [
    CategoryCheckboxComponent
  ],
  templateUrl: './favorite-categories.component.html',
  styleUrl: './favorite-categories.component.scss'
})
export class FavoriteCategoriesComponent implements OnInit {
  user!: User
  categories: Category[] = [];
  favCategories: Category[] = [];

  constructor(private categoryService: CategoryService,
              private router: Router, private authService: AuthService) {
  }

  ngOnInit() {
    var tmp = this.authService.getCurrentUser()
    if (tmp === undefined) {
      this.router.navigate(['/auth/login'])
      return
    }
    this.user = tmp
    this.loadFavCats()
    this.categoryService.getAllCategories().subscribe({
      next: response => {
        this.categories = response
      },
    })
  }

  loadFavCats() {
    this.categoryService.getFavoriteCategories(this.user.id).subscribe({
      next: response => {
        this.favCategories = response
      }
    })
  }

  remove(category: Category): void {
    this.favCategories.splice(
      this.favCategories.findIndex(c => c.id === category.id), 1);
  }

  add(category: Category): void {
    this.favCategories.push(category);
  }

  save() {
    this.categoryService.updateFavoriteCategories(this.user.id, this.favCategories.map(category => category.id));
  }

  selected(category: Category) {
    return this.favCategories.find((c) => c.id === category.id) !== undefined;
  }
}
