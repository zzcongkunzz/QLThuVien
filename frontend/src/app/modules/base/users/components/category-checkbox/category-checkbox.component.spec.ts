import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryCheckboxComponent } from './category-checkbox.component';

describe('CategoryCheckboxComponent', () => {
  let component: CategoryCheckboxComponent;
  let fixture: ComponentFixture<CategoryCheckboxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryCheckboxComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryCheckboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
