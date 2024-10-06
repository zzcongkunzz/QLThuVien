import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FavoriteCategoriesComponent } from './favorite-categories.component';

describe('FavoriteCategoriesComponent', () => {
  let component: FavoriteCategoriesComponent;
  let fixture: ComponentFixture<FavoriteCategoriesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FavoriteCategoriesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FavoriteCategoriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
