import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecommenderManageComponent } from './recommender-manage.component';

describe('RecommenderManageComponent', () => {
  let component: RecommenderManageComponent;
  let fixture: ComponentFixture<RecommenderManageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecommenderManageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecommenderManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
