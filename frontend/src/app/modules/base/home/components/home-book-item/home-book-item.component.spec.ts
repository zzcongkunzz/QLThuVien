import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeBookItemComponent } from './home-book-item.component';

describe('HomeBookItemComponent', () => {
  let component: HomeBookItemComponent;
  let fixture: ComponentFixture<HomeBookItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HomeBookItemComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HomeBookItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
