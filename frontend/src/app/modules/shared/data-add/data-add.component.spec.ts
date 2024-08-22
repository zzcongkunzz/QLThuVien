import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataAddComponent } from './data-add.component';

describe('DataAddComponent', () => {
  let component: DataAddComponent;
  let fixture: ComponentFixture<DataAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DataAddComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DataAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
